using UnityEngine;
using UnityEngine.UIElements;

namespace BulletParadise.Shooting
{
    [System.Serializable]
    public class ProjectileBehaviorData
    {
        public Sprite sprite;

        [Header("General")]
        public int damage;
        public float lifeTime;
        public float speed;

        [Header("Trigonometry")]
        public float frequency;
        public float amplitude;
        public float magnitude;


        public static ProjectileBehaviorData operator *(ProjectileBehaviorData a, ProjectileDataMultiplier b)
        {
            ProjectileBehaviorData newData = new()
            {
                sprite = a.sprite,

                damage = a.damage * b.damageMultiplier,
                lifeTime = a.lifeTime * b.lifeTimeMultiplier,
                speed = a.speed * b.speedMultiplier,

                frequency = a.frequency * b.frequencyMultiplier,
                amplitude = a.amplitude * b.amplitudeMultiplier,
                magnitude = a.magnitude * b.magnitudeMultiplier
            };
            return newData;
        }
    }
}