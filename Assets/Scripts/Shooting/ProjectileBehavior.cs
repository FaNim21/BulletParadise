using UnityEngine;

namespace BulletParadise.Shooting
{
    public abstract class ProjectileBehavior
    {
        public ProjectileData data;
        protected Rigidbody2D rb;

        protected Vector2 _velocity;
        protected Vector2 _nextGlobalPosition;

        protected Vector2 _startPosition;
        protected Quaternion _startRotation;

        protected float timeAlive = 0.04f;

        public ProjectileAdditionalData additionalData;

        public ProjectileBehavior(ProjectileData data, Vector2 velocity)
        {
            this.data = data;
            this.rb = rb;
            _velocity = velocity;

            _startPosition = rb.position;
            _startRotation = Quaternion.Euler(0, 0, rb.rotation);
        }

        public virtual void OnInitialize(Rigidbody2D rb)
        {
            this.rb = rb;
        }
        public virtual void UpdateLogic() { }
        public abstract void UpdatePhysics();
    }
}