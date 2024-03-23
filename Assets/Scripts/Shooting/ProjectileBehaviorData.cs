using UnityEngine;

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
    }
}