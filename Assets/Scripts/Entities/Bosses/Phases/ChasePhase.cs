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

        public override void Update()
        {
            target = PlayerController.Instance.transform;
            direction = ((Vector2)target.position - bossBehavior.entity.position).normalized;
        }

        public override void FixedUpdate(Rigidbody2D rb)
        {
            rb.MovePosition(rb.position + speed * Time.deltaTime * direction);
        }

        public override void OnExit()
        {

        }

    }
}