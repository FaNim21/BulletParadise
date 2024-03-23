using BulletParadise.Shooting.Projectiles;
using UnityEngine;

namespace BulletParadise.Shooting.Factors
{
    [CreateAssetMenu(fileName = "new Projectile Factory", menuName = "Weapons/Projectile Factory/Straight")]
    public class ProjectileStraightBehaviorFactory : ProjectileBehaviorFactory
    {
        public override ProjectileBehavior GenerateBehavior(Rigidbody2D rb, Vector2 velocity)
        {
            return new ProjectileStraightBehavior(behaviourData, rb, velocity);
        }
    }
}
