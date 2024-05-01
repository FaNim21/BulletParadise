using UnityEngine;

namespace BulletParadise.Shooting.Physics
{
    [System.Serializable]
    public class ProjectileStraightPhysics : IProjectileUpdater
    {
        public void OnUpdate(ProjectileBehavior behavior)
        {
            behavior.rb.MovePosition(behavior.rb.position + behavior.data.speed * Time.deltaTime * behavior.velocity);
        }
    }
}