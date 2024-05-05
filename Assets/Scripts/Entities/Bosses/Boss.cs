using BulletParadise.Components;
using BulletParadise.Datas;
using BulletParadise.Player;
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
            animator.runtimeAnimatorController = config.animatorController;

            isWorking = true;
            target = PlayerController.Instance.transform;
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
            CurrentPhase.LogicUpdate(config.phases[currentPhaseIndex].weapon, direction);
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

            //TODO: 0 zatrzymanie wszystkich timerow itp itd z zakonczeniem dungeona i podsumowaniem
            Destroy(gameObject);
        }

        public void UpdatePhase(float healthRatio)
        {
            if (!isWorking) return;

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

            animator.SetFloat("Speed", speed);
        }
    }
}