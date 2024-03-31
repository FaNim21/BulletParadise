using UnityEngine;

namespace BulletParadise.Shooting
{
    [System.Serializable]
    public struct ProjectileDataMultiplier
    {
        [Header("General")]
        public float damageMultiplier;
        public float lifeTimeMultiplier;
        public float speedMultiplier;

        [Header("Trigonometry")]
        public float frequencyMultiplier;
        public float amplitudeMultiplier;
        public float magnitudeMultiplier;
    }

    [System.Serializable]
    public class ProjectileBehaviorData
    {
        [Header("Components")]
        public ProjectileData data;
        public ProjectileBehaviorFactory behaviorFactory;

        [Header("Multiplier")]
        public ProjectileDataMultiplier dataMultiplier;

        [Header("Additional Data")]
        public float angle;

        public ProjectileBehavior GetBehavior(Rigidbody2D rb, Vector2 velocity)
        {
            ProjectileBehavior behavior = behaviorFactory.GenerateBehavior(data, rb, velocity);
            behavior.data *= dataMultiplier;
            return behavior;
        }
    }
}