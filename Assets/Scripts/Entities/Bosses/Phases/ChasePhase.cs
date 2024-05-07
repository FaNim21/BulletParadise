using BulletParadise.Player;
using BulletParadise.Shooting;
using BulletParadise.Visual.Drawing;
using UnityEngine;

namespace BulletParadise.Entities.Bosses.Phases
{
    [CreateAssetMenu(fileName = "ChasePhase", menuName = "Boss/Phase/Chase")]
    public class ChasePhase : Phase
    {
        public float chaseSpeed;

        public string primaryWeaponAnimName;
        public float primaryWeaponAnimTime;
        public float primaryWeaponAnimDelay;

        public string secondaryWeaponAnimName;
        public float secondaryWeaponAnimRestTime;
        public float secondaryWeaponAnimDelay;

        [Header("On Enter")]
        public MobController chaseMobs;
        public bool SpawnMobsChase;
        public int mobsChaseAmount;

        private PlayerController target;
        private Vector2 direction;


        public override void OnEnter()
        {
            boss.SetSpeedAnim(chaseSpeed);
            target = PlayerController.Instance;

            if (!SpawnMobsChase) return;
            for (int i = 0; i < mobsChaseAmount; i++)
                Instantiate(chaseMobs, (Vector2)boss.transform.position + Random.insideUnitCircle * 2, Quaternion.identity);
        }

        public override void LogicUpdate(Weapon weapon, Vector2 targetDirection)
        {
            direction = (target.GetToAimPosition() - (Vector2)shootingManager.shootingOffset.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            shootingManager.Shoot(weapon, angle, primaryWeaponAnimName, primaryWeaponAnimDelay, primaryWeaponAnimTime);
        }

        public override void PhysicsUpdate(Rigidbody2D rb)
        {
            if (boss.IsTargetInDistance(target.position, 0.5f))
            {
                if (boss.GetSpeedAnim() != 0f) boss.SetSpeedAnim(0f);
                return;
            }
            if (boss.GetSpeedAnim() != chaseSpeed) boss.SetSpeedAnim(chaseSpeed);
            //it's not worth it to make it better

            rb.MovePosition(rb.position + chaseSpeed * Time.deltaTime * direction);
        }

        public override void OnExit()
        {

        }

        public override void Draw()
        {
            GLDraw.DrawLine(boss.position, target.position, Color.red);
        }

        public override int CountAsRealPhase() => 1;
    }
}