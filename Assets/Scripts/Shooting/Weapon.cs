using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class Weapon : ScriptableObject
    {
        public ProjectileData[] projectiles;

        public int frequency = 1;

        public abstract void Shoot(string layerMask, Vector2 shootingPosition, float shootingAngle);
    }
}
