using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class ProjectileBehaviorFactory : ScriptableObject
    {
        public abstract ProjectileBehavior GenerateBehavior(Rigidbody2D rb, Vector2 velocity);
    }
}