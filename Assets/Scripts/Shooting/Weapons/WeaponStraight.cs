using UnityEngine;

namespace BulletParadise.Shooting.Weapons
{
    [CreateAssetMenu(fileName = "new WeaponStraight", menuName = "Weapons/WeaponType/Straight")]
    public class WeaponStraight : Weapon
    {
        public override void Shoot(string layerMask, Vector2 shootingPosition, float shootingAngle)
        {
            //throw new System.NotImplementedException();
        }
    }
}