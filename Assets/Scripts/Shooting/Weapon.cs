using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class Weapon : ScriptableObject
    {
        public ProjectileBehaviorFactory[] projectiles;


        public abstract void Shoot(string layerMask, Vector2 shootingPosition, float shootingAngle);
    }
}
