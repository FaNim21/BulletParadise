using System.Collections;
using UnityEngine;

namespace BulletParadise.Shooting.Weapons
{
    [CreateAssetMenu(fileName = "new WeaponArc", menuName = "Weapons/WeaponType/Arc")]
    public class WeaponArc : Weapon
    {
        public bool useShootingAngle = true;
        [SerializeField] private float angle;


        public override IEnumerator Shoot(int layerMask, Transform shootingPosition, float shootingAngle)
        {
            float degree;
            int length = projectiles.Length;
            int halfLength = length / 2;
            float halfAngle = angle / 2;
            float baseAngle = useShootingAngle ? shootingAngle : 0f;

            for (int i = 0; i < length; i++)
            {
                var current = projectiles[i];

                degree = baseAngle + (angle * (halfLength - i)) - halfAngle - (halfAngle * (length % 2 * -1));

                SendProjectile(current, layerMask, shootingPosition.position, degree);
            }

            yield break;
        }
    }
}