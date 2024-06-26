using BulletParadise.Entities;
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
        public float angle;

        public float frequency;
        public float amplitude;
        public float magnitude;

        public float rotationSpeed;
    }

    [System.Serializable]
    public class ProjectileBehaviorData
    {
        public string name = "ELEMENT";

        public ProjectileData data;
        public Entity entity;
        public ProjectileDataMultiplier dataMultiplier;

        public ProjectileLogicType logicType;
        public IProjectileUpdater logicUpdate;

        public ProjectilePhysicsType physicsType;
        public IProjectileUpdater physicsUpdate;

        public ProjectileAdditionalData additionalData;


        public void Initialize()
        {
            UpdateLogicFromEnum();
            UpdatePhysicsFromEnum();
        }

        public ProjectileAdditionalData GetAdditionalData() => additionalData;

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
                case ProjectileLogicType.Rotating: logicUpdate = new ProjectileRotateLogic(); break;
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