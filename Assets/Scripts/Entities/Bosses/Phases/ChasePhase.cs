using BulletParadise.Player;
using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.Entities.Bosses.Phases
{
    [CreateAssetMenu(fileName = "ChasePhase", menuName = "Boss/Phase/Chase")]
    public class ChasePhase : Phase
    {
        public float chaseSpeed;

        [Header("On Enter")]
        public MobController chaseMobs;
        public bool SpawnMobsChase;
        public int mobsChaseAmount;

        private Transform target;
        private Vector2 direction;


        public override void OnEnter()
        {
            if (!SpawnMobsChase) return;

            for (int i = 0; i < mobsChaseAmount; i++)
                Instantiate(chaseMobs, (Vector2)boss.transform.position + Random.insideUnitCircle * 2, Quaternion.identity);
        }

        public override void LogicUpdate(Weapon weapon, Vector2 targetDirection)
        {
            target = PlayerController.Instance.transform;
            direction = ((Vector2)target.position - boss.entity.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            shootingManager.Shoot(weapon, angle);
            boss.SetSpeedAnim(chaseSpeed);
        }

        public override void PhysicsUpdate(Rigidbody2D rb)
        {
            rb.MovePosition(rb.position + chaseSpeed * Time.deltaTime * direction);
        }

        public override void OnExit()
        {

        }

        public override void Draw()
        {
            //GLDraw.DrawCircle(boss.position, chaseRange, Color.red);
        }

        public override int CountAsRealPhase() => 1;
    }
}