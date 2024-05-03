using BulletParadise.Player;
using BulletParadise.Visual.Drawing;
using UnityEngine;

namespace BulletParadise.Entities.Bosses
{
    public class Boss : Entity
    {
        public Phase CurrentPhase { get { return phases[currentPhaseIndex].phase; } }

        public Transform target;

        [HideInInspector] public Entity entity;

        public BossPhase[] phases;

        [Header("Debug")]
        [SerializeField, ReadOnly] private byte currentPhaseIndex;
        [ReadOnly] public Vector2 direction;
        [ReadOnly] public Vector2 arenaCenter;


        public override void Awake()
        {
            base.Awake();

            entity = GetComponent<Entity>();
            rb = GetComponent<Rigidbody2D>();

            arenaCenter = transform.position;

            for (int i = 0; i < phases.Length; i++)
                phases[i].Initialize(this);
        }
        public override void Start()
        {
            base.Start();
            CurrentPhase.OnEnter();
            GameManager.AddDrawable(this);

            target = PlayerController.Instance.transform;
        }
        public void OnDestroy()
        {
            GameManager.RemoveDrawable(this);
        }

        public override void Update()
        {
            base.Update();

            direction = ((Vector2)target.position - position).normalized;
            CurrentPhase.LogicUpdate(direction);
        }
        public override void FixedUpdate()
        {
            CurrentPhase.PhysicsUpdate(rb);
        }

        public override void Draw()
        {
            GLDraw.DrawRay(position, direction * 3, Color.cyan);
            CurrentPhase.Draw();
        }

        public void UpdatePhase(float healthRatio)
        {
            for (byte i = currentPhaseIndex; i < phases.Length; i++)
            {
                var phase = phases[i];

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
            currentPhaseIndex++;
            CurrentPhase.OnEnter();
        }

        private void NewPhase(byte index)
        {
            CurrentPhase.OnExit();
            currentPhaseIndex = index;
            CurrentPhase.OnEnter();
        }
    }
}