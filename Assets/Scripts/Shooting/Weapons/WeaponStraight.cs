using UnityEngine;

namespace BulletParadise.Shooting.Weapons
{
    [CreateAssetMenu(fileName = "new WeaponStraight", menuName = "Weapons/WeaponType/Straight")]
    public class WeaponStraight : Weapon
    {
        public override void Shoot(string layerMask, Vector2 shootingPosition, float shootingAngle)
        {
            //TODO: 0 nie wiem co tu zrobic, bo mozna niby arc wykorzystac jako strzelanie prosto bez ustalania kata strzalu
        }
    }
}