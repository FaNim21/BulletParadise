using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.Entities.Bosses.Phases
{
    [CreateAssetMenu(fileName = "new StationaryPhase", menuName = "Boss/Phase/Stationary")]
    public class StationaryPhase : Phase
    {
        public Weapon primaryWeapon;
        public string primaryWeaponAnimName;
        public float primaryWeaponAnimTime;
        public float primaryWeaponAnimDelay;

        public float angleIncrease;
        private float _angle = 0;

        public override void OnEnter()
        {
            _angle = 0;
        }

        public override void LogicUpdate(Weapon weapon, Vector2 targetDirection)
        {
            if (primaryWeapon != null)
            {
                shootingManager.Shoot(primaryWeapon, _angle, IncreaseAngle, primaryWeaponAnimName, primaryWeaponAnimDelay, primaryWeaponAnimTime);
            }
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
