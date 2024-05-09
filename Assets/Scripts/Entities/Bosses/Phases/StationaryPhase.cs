using BulletParadise.Components;
using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.Entities.Bosses.Phases
{
    [CreateAssetMenu(fileName = "new StationaryPhase", menuName = "Boss/Phase/Stationary")]
    public class StationaryPhase : Phase
    {
        public WeaponShootBossData weaponShootingData;

        public float angleIncrease;
        private float _angle = 0;

        public override void OnEnter()
        {
            _angle = 0;
        }

        public override void LogicUpdate(Weapon weapon, Vector2 targetDirection)
        {
            shootingManager.Shoot(weaponShootingData, _angle, IncreaseAngle);
        }

        public override void PhysicsUpdate(Rigidbody2D rb)
        {

        }

        public override void OnExit()
        {

        }

        private void IncreaseAngle()
        {
            _angle += angleIncrease;
            _angle %= 360;
        }
    }
}
