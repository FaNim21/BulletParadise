using BulletParadise.Shooting.Projectiles;
using UnityEngine;

namespace BulletParadise.Shooting.Factors
{
    [CreateAssetMenu(fileName = "new Projectile Straight Factory", menuName = "Weapons/Projectile Factory/Straight")]
    public class ProjectileStraightBehaviorFactory : ProjectileBehaviorFactory
    {
        public override ProjectileBehavior GenerateBehavior(ProjectileData data, Rigidbody2D rb, Vector2 velocity)
        {
            return new ProjectileStraightBehavior(data, rb, velocity);
        }
    }
}
