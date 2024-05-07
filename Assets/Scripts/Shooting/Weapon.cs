﻿using BulletParadise.Entities.Items;
using BulletParadise.World;
using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class Weapon : Item
    {
        public ProjectileBehaviorData[] projectiles;

        public float frequency = 1;


        public virtual void Initialize()
        {
            foreach (var item in projectiles)
                item.Initialize();
        }

        public abstract void Shoot(int layerMask, Vector2 shootingPosition, float shootingAngle);

        protected virtual void SendProjectile(ProjectileBehaviorData current, int layerMask, Vector2 shootingPosition, float degree)
        {
            degree += current.GetAdditionalData().angle;
            Quaternion quaternionAngle = Quaternion.Euler(0, 0, degree);
            var projectile = ProjectilePooler.Instance.GetProjectile();
            ProjectileBehavior behavior = current.GetBehavior(quaternionAngle * Vector2.right);
            projectile.Setup(layerMask, behavior, shootingPosition, quaternionAngle);
        }

        public override void OnEnable()
        {
            Initialize();
        }
    }
}
