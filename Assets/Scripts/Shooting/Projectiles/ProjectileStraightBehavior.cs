using UnityEngine;

namespace BulletParadise.Shooting.Projectiles
{
    public class ProjectileStraightBehavior : ProjectileBehavior
    {
        public ProjectileStraightBehavior(ProjectileData data, Vector2 velocity) : base(data, velocity) { }

        public override void UpdatePhysics()
        {
            rb.MovePosition(rb.position + data.speed * Time.deltaTime * _velocity);
        }
    }
}