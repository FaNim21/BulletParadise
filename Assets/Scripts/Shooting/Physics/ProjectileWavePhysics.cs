using UnityEngine;

namespace BulletParadise.Shooting.Physics
{
    [System.Serializable]
    public class ProjectileWavePhysics : IProjectileUpdater
    {
        public void OnUpdate(ProjectileBehavior behavior)
        {
            //troche przypomina flaila, ale chodzi o to ze robi poprostu kolo wokol entity z ktorego jest wystrzeliwany
            //Vector2 nextPosition = new(Mathf.Cos(timeAlive * (data.frequency * 13.75f)) * data.amplitude, Mathf.Sin(timeAlive * (data.frequency * 13.75f)) * data.amplitude);


            Vector2 nextPosition = new(behavior.data.speed * behavior.timeAlive, Mathf.Sin(behavior.timeAlive * (behavior.additionalData.frequency * 13.75f)) * behavior.additionalData.amplitude);
            behavior.nextGlobalPosition = (Vector2)(behavior.startRotation * nextPosition) + behavior.startPosition;

            behavior.rb.MovePosition(behavior.nextGlobalPosition);

            behavior.timeAlive += Time.deltaTime;
        }
    }
}
