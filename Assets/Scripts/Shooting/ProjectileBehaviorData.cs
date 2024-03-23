using UnityEngine;

namespace BulletParadise.Shooting
{
    //[CreateAssetMenu(fileName = "new Projectile Behavior Data", menuName = "Weapons/Projectile Data/Straight")]
    [System.Serializable]
    public class ProjectileBehaviorData// : ScriptableObject
    {
        public Sprite sprite;

        public int damage;
        public int timeAlive;
        public int speed;
    }
}