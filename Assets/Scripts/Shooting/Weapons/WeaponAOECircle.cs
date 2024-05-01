using UnityEngine;

namespace BulletParadise.Shooting.Weapons
{
    [CreateAssetMenu(fileName = "new WeaponCircle", menuName = "Weapons/WeaponType/Circle")]
    public class WeaponAOECircle : Weapon
    {
        public override void Shoot(int layerMask, Vector2 shootingPosition, float shootingAngle)
        {
            int length = projectiles.Length;
            float degree;
            float arcDegree = 360 / length;

            for (int i = 0; i < length; i++)
            {
                var current = projectiles[i];

                degree = shootingAngle + (i * arcDegree);

                SendProjectile(current, layerMask, shootingPosition, degree);
            }
        }
    }
}