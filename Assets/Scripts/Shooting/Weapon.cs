using BulletParadise.Entities.Items;
using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class Weapon : Item
    {
        public ProjectileBehaviorData[] projectiles;

        public int frequency = 1;

        public abstract void Shoot(string layerMask, Vector2 shootingPosition, float shootingAngle);

        protected void SendProjectile(ProjectileBehaviorData current, string layerMask, Vector2 shootingPosition, float degree)
        {
            Quaternion quaternionAngle = Quaternion.Euler(0, 0, degree);
            var projectile = Instantiate(GameManager.Projectile, shootingPosition, quaternionAngle);
            ProjectileBehavior behavior = current.GetBehavior(projectile.rb, quaternionAngle * Vector2.right);
            projectile.Setup(layerMask, behavior);
        }
    }
}
