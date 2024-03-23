using UnityEngine;

namespace BulletParadise.Shooting.Projectiles
{
    public class ProjectileStraightBehavior : ProjectileBehavior
    {
        public ProjectileStraightBehavior(ProjectileBehaviorData data, Rigidbody2D rb, Vector2 velocity) : base(data, rb, velocity) { }

        public override void UpdatePhysics()
        {
            rb.MovePosition(rb.position + data.speed * Time.deltaTime * _velocity);
        }
    }
}