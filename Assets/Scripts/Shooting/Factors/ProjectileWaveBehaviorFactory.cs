using BulletParadise.Shooting.Projectiles;
using UnityEngine;

namespace BulletParadise.Shooting.Factors
{
    [CreateAssetMenu(fileName = "new Projectile Wave Factory", menuName = "Weapons/Projectile Factory/Wave")]
    public class ProjectileWaveBehaviorFactory : ProjectileBehaviorFactory
    {
        public override ProjectileBehavior GenerateBehavior(Rigidbody2D rb, Vector2 velocity)
        {
            return new ProjectileWaveBehaviour(behaviourData, rb, velocity);
        }
    }
}