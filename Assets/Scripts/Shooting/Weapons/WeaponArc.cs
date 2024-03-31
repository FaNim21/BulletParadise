using UnityEngine;

namespace BulletParadise.Shooting.Weapons
{
    [CreateAssetMenu(fileName = "new WeaponArc", menuName = "Weapons/WeaponType/Arc")]
    public class WeaponArc : Weapon
    {
        [SerializeField] private bool useShootingAngle = true;
        [SerializeField] private float angle;


        public override void Shoot(string layerMask, Vector2 shootingPosition, float shootingAngle)
        {
            float degree;
            int length = projectiles.Length;
            int halfLength = length / 2;
            float halfAngle = angle / 2;
            float baseAngle = useShootingAngle ? shootingAngle : 0f;

            for (int i = 1; i <= length; i++)
            {
                var current = projectiles[i - 1];

                degree = baseAngle + (angle * (halfLength - (i - 1))) - halfAngle - (halfAngle * (length % 2 * -1));

                SendProjectile(current, layerMask, shootingPosition, degree);
            }
        }
    }
}