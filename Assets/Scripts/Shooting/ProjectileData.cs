using UnityEngine;

namespace BulletParadise.Shooting
{
    [System.Serializable]
    public class ProjectileDataMultiplier
    {
        [Header("General")]
        public int damageMultiplier = 1;
        public float lifeTimeMultiplier = 1;
        public float speedMultiplier = 1;

        [Header("Trigonometry")]
        public float frequencyMultiplier = 1;
        public float amplitudeMultiplier = 1;
        public float magnitudeMultiplier = 1;
    }

    [System.Serializable]
    public class ProjectileData
    {
        public ProjectileBehaviorFactory behaviorFactory;
        public ProjectileDataMultiplier dataMultiplier;

        public ProjectileBehavior GetBehaviorWithMultiplier(Rigidbody2D rb, Vector2 velocity)
        {
            ProjectileBehavior behavior = behaviorFactory.GenerateBehavior(rb, velocity);
            behavior.data *= dataMultiplier;
            return behavior;
        }
    }
}