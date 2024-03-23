using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class ProjectileBehaviorFactory : ScriptableObject
    {
        public ProjectileBehaviorData behaviourData;

        public abstract ProjectileBehavior GenerateBehavior(Rigidbody2D rb, Vector2 velocity);
    }
}