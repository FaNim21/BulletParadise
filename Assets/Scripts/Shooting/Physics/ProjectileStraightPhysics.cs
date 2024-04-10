using UnityEngine;

namespace BulletParadise.Shooting.Physics
{
    //[CreateAssetMenu(fileName = "new Projectile Straight Factory", menuName = "Weapons/Projectile Physics/Straight")]
    [System.Serializable]
    public class ProjectileStraightPhysics : IProjectileUpdater
    {
        public void OnUpdate(ProjectileBehavior behavior)
        {
            behavior.rb.MovePosition(behavior.rb.position + behavior.data.speed * Time.deltaTime * behavior.velocity);
        }
    }
}