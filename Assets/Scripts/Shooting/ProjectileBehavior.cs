using UnityEngine;

namespace BulletParadise.Shooting
{
    public class ProjectileBehavior
    {
        public ProjectileData data;
        public Rigidbody2D rb;
        public Transform body;

        public Vector2 velocity;
        public Vector2 nextGlobalPosition;

        public Vector2 startPosition;
        public Quaternion startRotation;

        public float timeAlive = 0.04f;

        public ProjectileAdditionalData additionalData;

        public IProjectileUpdater logic;
        public IProjectileUpdater physics;


        public ProjectileBehavior(ProjectileData data, IProjectileUpdater logic, IProjectileUpdater physics, Vector2 velocity)
        {
            this.data = data;
            this.logic = logic;
            this.physics = physics;
            this.velocity = velocity;
        }

        public void OnInitialize(Rigidbody2D rb, Transform body)
        {
            this.rb = rb;
            this.body = body;

            startPosition = rb.position;
            startRotation = Quaternion.Euler(0, 0, rb.rotation);
        }
    }
}