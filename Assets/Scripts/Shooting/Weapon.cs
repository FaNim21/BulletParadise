using BulletParadise.Entities.Items;
using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class Weapon : Item
    {
        public ProjectileBehaviorData[] projectiles;

        public float frequency = 1;


        public virtual void Initialize()
        {
            if (projectiles == null || projectiles.Length == 0) return;

            foreach (var item in projectiles)
                item.Initialize();
        }

        public abstract void Shoot(int layerMask, Transform shootingPosition, float shootingAngle);

        protected virtual void SendProjectile(ProjectileBehaviorData current, int layerMask, Vector2 shootingPosition, float degree)
        {
            degree += current.GetAdditionalData().angle;
            Quaternion quaternionAngle = Quaternion.Euler(0, 0, degree);
            ProjectileBehavior behavior = current.GetBehavior(quaternionAngle * Vector2.right);

            //ProjectilePooler.Instance.GetProjectile();
            var projectile = Instantiate(GameManager.Projectile, shootingPosition, quaternionAngle);
            projectile.Setup(layerMask, behavior, shootingPosition, quaternionAngle);
        }

        public override void OnEnable()
        {
            Initialize();
        }
    }
}
