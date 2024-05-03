using BulletParadise.Entities.Bosses;
using BulletParadise.Visual.Drawing;
using UnityEngine;

namespace BulletParadise
{
    [CreateAssetMenu(fileName = "ResetToMiddlePhase", menuName = "Boss/Phase/ResetToMiddle")]
    public class ResetToMiddlePhase : Phase
    {
        private Vector2 target;
        private Vector2 direction;
        private float timer;

        private const float speed = 7f;
        private const float timeForRestart = 2f;


        public override void OnEnter()
        {
            boss.entity.healthManager.SetInvunerability(true);
            timer = 0f;
            target = boss.arenaCenter;
        }

        public override void LogicUpdate(Vector2 targetDirection)
        {
            direction = (target - boss.entity.position).normalized;

            if (Vector2.Distance(boss.transform.position, target) <= 0.001f)
            {
                boss.transform.position = target;

                if (timer >= timeForRestart)
                    boss.GoToNextPhase();

                timer += Time.deltaTime;
            }
        }

        public override void PhysicsUpdate(Rigidbody2D rb)
        {
            rb.MovePosition(rb.position + speed * Time.deltaTime * direction);
        }

        public override void OnExit()
        {
            boss.entity.healthManager.SetInvunerability(false);
        }

        public override void Draw()
        {
            GLDraw.DrawLine(boss.position, boss.arenaCenter, Color.yellow, 0.01f);
        }
    }
}