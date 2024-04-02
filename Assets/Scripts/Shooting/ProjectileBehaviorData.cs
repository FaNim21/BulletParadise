using UnityEngine;

namespace BulletParadise.Shooting
{
    [System.Serializable]
    public struct ProjectileDataMultiplier
    {
        public float damageMultiplier;
        public float lifeTimeMultiplier;
        public float speedMultiplier;
    }

    [System.Serializable]
    public struct ProjectileAdditionalData
    {
        public float frequency;
        public float amplitude;
        public float magnitude;
    }

    [System.Serializable]
    public class ProjectileBehaviorData
    {
        public string name = "ELEMENT";

        public ProjectileData data;
        public ProjectileBehaviorFactory behaviorFactory;
        public ProjectileDataMultiplier dataMultiplier;

        public float angle;
        public ProjectileAdditionalData additionalData;


        public ProjectileBehavior GetBehavior(Rigidbody2D rb, Vector2 velocity)
        {
            ProjectileBehavior behavior = behaviorFactory.GenerateBehavior(data, rb, velocity);
            behavior.data *= dataMultiplier;
            behavior.additionalData = additionalData;
            return behavior;
        }
    }
}