using BulletParadise.Shooting.Projectiles;
using UnityEngine;

namespace BulletParadise.Shooting.Factors
{
    [CreateAssetMenu(fileName = "new Projectile Wave Factory", menuName = "Weapons/Projectile Factory/Wave")]
    public class ProjectileWaveBehaviorFactory : ProjectileBehaviorFactory
    {
        public override ProjectileBehavior GenerateBehavior(ProjectileData data, Rigidbody2D rb, Vector2 velocity)
        {
            return new ProjectileWaveBehavior(data, rb, velocity);
        }
    }
}