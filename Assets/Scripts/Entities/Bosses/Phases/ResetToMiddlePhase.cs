using BulletParadise.Entities.Bosses;
using UnityEngine;

namespace BulletParadise
{
    [CreateAssetMenu(fileName = "ResetToMiddlePhase", menuName = "Boss/Phase/ResetToMiddle")]
    public class ResetToMiddlePhase : Phase
    {
        private Vector2 target;
        private Vector2 direction;
        private float angle;
        private float timer;

        private const float speed = 7f;
        private const float timeForRestart = 2f;


        public override void OnEnter()
        {
            bossBehavior.entity.healthManager.SetInvunerability(true);
            timer = 0f;
            target = bossBehavior.arenaCenter;
        }

        public override void Update()
        {
            direction = (target - bossBehavior.entity.position).normalized;
            //angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (Vector2.Distance(bossBehavior.transform.position, target) <= 0.001f)
            {
                bossBehavior.transform.position = target;

                if (timer >= timeForRestart)
                    bossBehavior.GoToNextPhase();

                timer += Time.deltaTime;
            }
        }

        public override void FixedUpdate(Rigidbody2D rb)
        {
            rb.MovePosition(rb.position + speed * Time.deltaTime * direction);
        }

        public override void OnExit()
        {
            bossBehavior.entity.healthManager.SetInvunerability(false);
        }
    }
}