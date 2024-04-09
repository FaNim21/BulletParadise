using UnityEngine;

namespace BulletParadise.Shooting.Projectiles
{
    public class ProjectileWaveBehavior : ProjectileBehavior
    {
        public ProjectileWaveBehavior(ProjectileData data, Vector2 velocity) : base(data, velocity) { }

        public override void UpdatePhysics()
        {
            //troche przypomina flaila, ale chodzi o to ze robi poprostu kolo wokol entity z ktorego jest wystrzeliwany
            //Vector2 nextPosition = new(Mathf.Cos(timeAlive * (data.frequency * 13.75f)) * data.amplitude, Mathf.Sin(timeAlive * (data.frequency * 13.75f)) * data.amplitude);


            Vector2 nextPosition = new(data.speed * timeAlive, Mathf.Sin(timeAlive * (additionalData.frequency * 13.75f)) * additionalData.amplitude);
            _nextGlobalPosition = (Vector2)(_startRotation * nextPosition) + _startPosition;

            rb.MovePosition(_nextGlobalPosition);

            timeAlive += Time.deltaTime;
        }
    }
}
