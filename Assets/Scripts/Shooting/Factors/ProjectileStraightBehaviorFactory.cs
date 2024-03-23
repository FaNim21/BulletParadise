using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise
{
    [CreateAssetMenu(fileName = "new Projectile Factory", menuName = "Weapons/Projectile Factory/Straight")]
    public class ProjectileStraightBehaviorFactory : ProjectileBehaviorFactory
    {
        public ProjectileBehaviorData behaviourData;

        public override ProjectileBehavior GenerateBehavior(Rigidbody2D rb, Vector2 velocity)
        {
            return new ProjectileStraightBehavior(behaviourData, rb, velocity);
        }
    }
}
