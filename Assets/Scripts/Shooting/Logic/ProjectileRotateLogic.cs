using UnityEngine;

namespace BulletParadise.Shooting.Logic
{
    [System.Serializable]
    public class ProjectileRotateLogic : IProjectileUpdater
    {
        public void OnUpdate(ProjectileBehavior behavior)
        {
            Vector3 childAngle = behavior.body.eulerAngles;
            childAngle.z += behavior.additionalData.rotationSpeed * Time.deltaTime;
            behavior.body.eulerAngles = childAngle;
        }
    }
}
