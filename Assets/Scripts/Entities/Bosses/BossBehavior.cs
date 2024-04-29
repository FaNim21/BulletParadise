using UnityEngine;

namespace BulletParadise.Entities.Bosses
{
    public class BossBehavior : MonoBehaviour
    {
        public Phase CurrentPhase { get { return phases[currentPhaseIndex].phase; } }

        [HideInInspector] public Entity entity;
        [HideInInspector] private Rigidbody2D rb;

        public BossPhase[] phases;

        [Header("Debug")]
        [SerializeField, ReadOnly] private byte currentPhaseIndex;
        [ReadOnly] public Vector2 arenaCenter;


        private void Awake()
        {
            entity = GetComponent<Entity>();
            rb = GetComponent<Rigidbody2D>();

            arenaCenter = transform.position;

            for (int i = 0; i < phases.Length; i++)
                phases[i].Initialize(this);
        }
        private void Start()
        {
            CurrentPhase.OnEnter();
        }

        private void Update()
        {
            CurrentPhase.Update();
        }
        private void FixedUpdate()
        {
            CurrentPhase.FixedUpdate(rb);
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