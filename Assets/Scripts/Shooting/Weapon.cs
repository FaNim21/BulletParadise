using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class Weapon : ScriptableObject
    {
        public ProjectileBehaviorData[] projectiles;

        public Sprite sprite;
        public int frequency = 1;

        public abstract void Shoot(string layerMask, Vector2 shootingPosition, float shootingAngle);
    }
}
