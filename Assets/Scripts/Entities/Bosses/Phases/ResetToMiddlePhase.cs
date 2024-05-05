using BulletParadise.Shooting;
using BulletParadise.Visual.Drawing;
using UnityEngine;

namespace BulletParadise.Entities.Bosses.Phases
{
    [CreateAssetMenu(fileName = "ResetToMiddlePhase", menuName = "Boss/Phase/ResetToMiddle")]
    public class ResetToMiddlePhase : Phase
    {
        public float speed;

        private Vector2 target;
        private float timer;

        private const float timeForRestart = 2f;
        private const float tolerance = 0.1f;


        public override void OnEnter()
        {
            boss.entity.healthManager.SetInvulnerability(true);
            timer = 0f;
            target = boss.arenaCenter;
        }

        public override void LogicUpdate(Weapon weapon, Vector2 targetDirection)
        {
            boss.SetSpeedAnim(speed);

            if (Vector2.Distance(boss.transform.position, target) <= tolerance)
            {
                if (timer >= timeForRestart)
                    boss.GoToNextPhase();

                timer += Time.deltaTime;
            }
        }

        public override void PhysicsUpdate(Rigidbody2D rb)
        {
            Vector2 position = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
            rb.MovePosition(position);
        }

        public override void OnExit()
        {
            boss.entity.healthManager.SetInvulnerability(false);
        }

        public override void Draw()
        {
            GLDraw.DrawLine(boss.position, boss.arenaCenter, Color.yellow, 0.01f);
        }

        public override int CountAsRealPhase() => 0;
    }
}