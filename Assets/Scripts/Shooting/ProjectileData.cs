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

        public static ProjectileData operator *(ProjectileData a, ProjectileDataMultiplier b)
        {
            ProjectileData newData = (ProjectileData)CreateInstance("ProjectileData");

            newData.sprite = a.sprite;

            newData.damage = (int)(a.damage * b.damageMultiplier);
            newData.lifeTime = a.lifeTime * b.lifeTimeMultiplier;
            newData.speed = a.speed * b.speedMultiplier;

            return newData;
        }
    }
}