using UnityEngine;

namespace BulletParadise.Shooting
{
    [CreateAssetMenu(fileName = "new Projectile Data", menuName = "Weapons/Projectile/Data")]
    public class ProjectileData : ScriptableObject
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


        public static ProjectileData operator *(ProjectileData a, ProjectileDataMultiplier b)
        {
            ProjectileData newData = (ProjectileData)CreateInstance("ProjectileData");

            newData.sprite = a.sprite;

            newData.damage = a.damage * b.damageMultiplier;
            newData.lifeTime = a.lifeTime * b.lifeTimeMultiplier;
            newData.speed = a.speed * b.speedMultiplier;

            newData.frequency = a.frequency * b.frequencyMultiplier;
            newData.amplitude = a.amplitude * b.amplitudeMultiplier;
            newData.magnitude = a.magnitude * b.magnitudeMultiplier;
            return newData;
        }
    }
}