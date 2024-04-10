using BulletParadise.Shooting.Logic;
using BulletParadise.Shooting.Physics;
using UnityEngine;

namespace BulletParadise.Shooting
{
    public enum ProjectileLogicType
    {
        Default,
        Rotating,
    }

    public enum ProjectilePhysicsType
    {
        Straight,
        Wave,
    }

    [System.Serializable]
    public struct ProjectileDataMultiplier
    {
        public float damageMultiplier;
        public float lifeTimeMultiplier;
        public float speedMultiplier;
    }

    [System.Serializable]
    public struct ProjectileAdditionalData
    {
        public float frequency;
        public float amplitude;
        public float magnitude;
    }

    [System.Serializable]
    public class ProjectileBehaviorData
    {
        public string name = "ELEMENT";

        public ProjectileData data;
        public ProjectileDataMultiplier dataMultiplier;

        public ProjectileLogicType logicType;
        public IProjectileUpdater logicUpdate;

        public ProjectilePhysicsType physicsType;
        public IProjectileUpdater physicsUpdate;

        public float angle;
        public ProjectileAdditionalData additionalData;


        public void Initialize()
        {
            UpdateLogicFromEnum();
            UpdatePhysicsFromEnum();
        }

        public ProjectileBehavior GetBehavior(Vector2 velocity)
        {
            ProjectileBehavior behavior = new(data, logicUpdate, physicsUpdate, velocity);
            behavior.data *= dataMultiplier;
            behavior.additionalData = additionalData;
            return behavior;
        }

        public void UpdateLogicFromEnum()
        {
            switch (logicType)
            {
                case ProjectileLogicType.Default: logicUpdate = new ProjectileDefaultLogic(); break;
            }
        }

        public void UpdatePhysicsFromEnum()
        {
            switch (physicsType)
            {
                case ProjectilePhysicsType.Straight: physicsUpdate = new ProjectileStraightPhysics(); break;
                case ProjectilePhysicsType.Wave: physicsUpdate = new ProjectileWavePhysics(); break;
            }
        }
    }
}