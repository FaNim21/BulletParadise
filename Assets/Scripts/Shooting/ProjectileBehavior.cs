using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class ProjectileBehavior
    {
        public ProjectileBehaviorData data;
        protected Rigidbody2D rb;

        protected Vector2 velocity;

        public ProjectileBehavior(ProjectileBehaviorData data, Rigidbody2D rb, Vector2 velocity)
        {
            this.data = data;
            this.rb = rb;
            this.velocity = velocity;
        }

        public virtual void UpdateLogic() { }
        public abstract void UpdatePhysics();
    }
}