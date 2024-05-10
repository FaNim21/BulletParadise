using BulletParadise.Visual.Drawing;
using UnityEngine;

namespace BulletParadise.Entities.Bosses.Phases
{
    [CreateAssetMenu(fileName = "ResetToMiddlePhase", menuName = "Boss/Phase/ResetToMiddle")]
    public class ResetToMiddlePhase : Phase
    {
        public float speed;

        private Vector2 _target;
        private float _timer;

        private readonly float _timeToRestart = 2.25f;
        private readonly float _timeToVulnerability = 1f;
        private readonly float _tolerance = 0.1f;


        public override void OnEnter()
        {
            boss.entity.healthManager.SetInvulnerability(true);
            _timer = 0f;
            _target = boss.arenaCenter;

            boss.SetSpeedAnim(speed);
        }

        public override void LogicUpdate(Vector2 targetDirection)
        {
            if (Vector2.Distance(boss.transform.position, _target) <= _tolerance)
            {
                boss.SetSpeedAnim(0f);

                if (_timer >= _timeToVulnerability && boss.entity.healthManager.IsInvulnerable())
                    boss.entity.healthManager.SetInvulnerability(false);

                if (_timer >= _timeToRestart)
                    boss.GoToNextPhase();

                _timer += Time.deltaTime;
            }
        }

        public override void PhysicsUpdate(Rigidbody2D rb)
        {
            Vector2 position = Vector2.MoveTowards(rb.position, _target, speed * Time.deltaTime);
            rb.MovePosition(position);
        }

        public override void OnExit() { }

        public override void Draw()
        {
            GLDraw.DrawLine(boss.position, boss.arenaCenter, Color.yellow, 0.01f);
        }

        public override bool UpdatePhaseOnHit() => false;
        public override int CountAsRealPhase() => 0;
    }
}