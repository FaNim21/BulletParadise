using BulletParadise.Components;
using BulletParadise.Datas;
using BulletParadise.Misc;
using BulletParadise.Player;
using BulletParadise.UI.Windows;
using BulletParadise.Visual.Drawing;
using System.Collections;
using UnityEngine;

namespace BulletParadise.Entities.Bosses
{
    public class Boss : Entity
    {
        public Phase CurrentPhase { get { return config.phases[currentPhaseIndex].phase; } }

        [HideInInspector] public Entity entity;
        public BossConfig config;

        [HideInInspector] public Animator animator;
        private SpriteRenderer _spriteRenderer;
        private Transform _body;
        private CircleCollider2D _circleCollider;
        private ShootingManager _shootingManager;

        [Header("Debug")]
        [SerializeField, ReadOnly] private byte currentPhaseIndex;
        [SerializeField, ReadOnly] private bool isWorking;
        [ReadOnly] public Vector2 direction;
        [ReadOnly] public Vector2 arenaCenter;
        [ReadOnly] public Transform target;


        public override void Awake()
        {
            base.Awake();

            entity = GetComponent<Entity>();
            rb = GetComponent<Rigidbody2D>();
            _body = transform.Find("Body");
            animator = _body.GetComponent<Animator>();
            _circleCollider = _body.GetComponent<CircleCollider2D>();
            _spriteRenderer = _body.GetComponent<SpriteRenderer>();
            _shootingManager = GetComponent<ShootingManager>();

            arenaCenter = transform.position;
            //if (config != null) StartCoroutine(SetupConfigWithDelay(config, 4f));
        }
        public override void Start()
        {
            base.Start();
            GameManager.AddDrawable(this);
        }
        public void SetupConfig(BossConfig config)
        {
            this.config = config;
            config.Initialize(this);
            _spriteRenderer.sprite = config.sprite;
            maxHealth = config.maxHealth;
            health = maxHealth;
            animator.runtimeAnimatorController = config.animatorController;
            if (config.bodyOffset != Vector2.zero)
            {
                _shootingManager.ChangeShootingOffset(config.bodyOffset);
            }
            _circleCollider.radius = config.colliderRadius;

            target = PlayerController.Instance.transform;
            isWorking = true;
            healthManager.Initialize();
            CurrentPhase.OnEnter();
        }
        public void OnDestroy()
        {
            GameManager.RemoveDrawable(this);
        }

        public override void Update()
        {
            if (!isWorking) return;
            base.Update();

            direction = ((Vector2)target.position - position).normalized;
            CurrentPhase.LogicUpdate(direction);
        }
        public override void FixedUpdate()
        {
            if (!isWorking) return;
            CurrentPhase.PhysicsUpdate(rb);
        }

        public override void Draw()
        {
            if (!isWorking) return;

            GLDraw.DrawRay(position, direction * 3, Color.cyan);
            GLDraw.DrawCircle((Vector2)_body.position + _circleCollider.offset, _circleCollider.radius, Color.green);

            CurrentPhase.Draw();
        }

        public override void OnDeath()
        {
            base.OnDeath();

            GameManager.Instance.worldManager.CompleteDungeon();
            if (!GameManager.Instance.worldManager.WasRunCheated())
                GameManager.Instance.GetEnteredPortalStats().completions++;
            PlayerController.Instance.canvasHandle.OpenWindow<SummaryScreen>();
            Destroy(gameObject);
        }

        public void UpdatePhase(float healthRatio)
        {
            if (!isWorking || !CurrentPhase.UpdatePhaseOnHit()) return;

            for (byte i = (byte)(currentPhaseIndex + 1); i < config.phases.Length; i++)
            {
                var phase = config.phases[i];

                if (healthRatio >= phase.percentage.x && healthRatio < phase.percentage.y)
                {
                    NewPhase(i);
                    return;
                }
            }
        }
        public void GoToNextPhase()
        {
            CurrentPhase.OnExit();
            if ((currentPhaseIndex + 1) >= config.phases.Length) return;
            currentPhaseIndex++;
            CurrentPhase.OnEnter();
        }
        private void NewPhase(byte index)
        {
            CurrentPhase.OnExit();
            if (index >= config.phases.Length) return;
            currentPhaseIndex = index;
            CurrentPhase.OnEnter();
        }

        private IEnumerator SetupConfigWithDelay(BossConfig config, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (isWorking) yield break;
            SetupConfig(config);
        }

        public void SetSpeedAnim(float speed)
        {
            if (animator == null) return;

            animator.SetBool("moving", speed != 0f);
            animator.SetFloat("Speed", speed);
        }
        public float GetSpeedAnim()
        {
            return animator.GetFloat("Speed");
        }
    }
}

/*
----- Spoon King (Ka¿dy atak fazy tego bossa jak i przysz³ego jest symbolizowany czerwon¹ lampk¹, dla zilustrowania zagro¿enia)

- 100% - pierwsza faza: King strzela z siebie potrójnymi pociskami z odstêpem 1sek. pod¹¿a za graczem 
(TO JEST PODSTAWOWY ATAK, KTÓRY JEST W KA¯DEJ FAZIE)

- 75% - druga faza: King wraca na œrodek i nastepnie, Stoj¹c w miejscu uderza lask¹ (attack2) 
i 10-20 pocisków wokó³ niego zaraz po uderzeniu w ziemiê, (powtarza 3 razy z rzêdu i mo¿e powtarzaæ ten atak ale nie czêœæiej ni¿ co 10 sekund). 
Po skoñczeniu fazy wraca do pocz¹tku (podst. atak)

- 50% - trzecia faza: King wraca na œrodek areny i zostaje tam do rozpoczêcia ostatniej fazy. 
King zaczyna respiæ moby[500hp] (attack1), ka¿de wykonanie ataku respi 3 moby raz na 6sek.

- 25% - czwarta faza: King ostatni raz respi 3 moby (atak 1)*/