using UnityEngine;

namespace BulletParadise.Shooting.Weapons
{
    [CreateAssetMenu(fileName = "new WeaponSpawnEntity", menuName = "Weapons/WeaponType/EntitySpawn")]
    public class WeaponSpawnEntity : Weapon
    {
        public override void Shoot(int layerMask, Vector2 shootingPosition, float shootingAngle)
        {
            int length = projectiles.Length;

            for (int i = 0; i < length; i++)
            {
                var current = projectiles[i];

                SendProjectile(current, layerMask, shootingPosition, 0);
            }
        }

        protected override void SendProjectile(ProjectileBehaviorData current, int layerMask, Vector2 shootingPosition, float degree)
        {
            var entity = Instantiate(current.entity, shootingPosition + Random.insideUnitCircle * 2, Quaternion.identity);
        }
    }
}
