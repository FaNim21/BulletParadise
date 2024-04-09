using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class ProjectileBehaviorFactory : ScriptableObject
    {
        public abstract ProjectileBehavior GenerateBehavior(ProjectileData data, Vector2 velocity);
    }
}