using BulletParadise.Entities.Bosses;
using BulletParadise.Player;
using UnityEngine;

namespace BulletParadise
{
    [CreateAssetMenu(fileName = "ChasePhase", menuName = "Boss/Phase/Chase")]
    public class ChasePhase : Phase
    {
        private Transform target;
        private Vector2 direction;
        private float distanceToTarget;

        private const float speed = 3f;


        public override void OnEnter()
        {

        }

        public override void LogicUpdate(Vector2 targetDirection)
        {
            target = PlayerController.Instance.transform;
            direction = ((Vector2)target.position - boss.entity.position).normalized;
        }

        public override void PhysicsUpdate(Rigidbody2D rb)
        {
            rb.MovePosition(rb.position + speed * Time.deltaTime * direction);
        }

        public override void OnExit()
        {
        }

        public override void Draw()
        {
            //GLDraw.DrawCircle(boss.position, chaseRange, Color.red);
        }
    }
}