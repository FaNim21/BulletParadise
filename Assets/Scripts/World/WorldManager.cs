using BulletParadise.Entities;
using BulletParadise.Visual.Drawing;
using System.Collections.Generic;
using UnityEngine;

namespace BulletParadise.World
{
    public class WorldManager : MonoBehaviour
    {
        public List<SceneObject> staticObjects;
        public IReadOnlyList<SceneObject> ObjectsReadOnly => staticObjects;

        public List<SceneObject> dynamicObjects;
        public IReadOnlyList<SceneObject> DynamicObjectsReadOnly => dynamicObjects;

        public List<Entity> entities;
        public IReadOnlyList<Entity> EntitiesReadOnly => entities;


        [SerializeField] private DrawDebug _drawDebug;

        [Header("Values")]
        public float renderingRadius;
        public float mobsRenderingRadius;
        public bool isWorldUpdating;

        [Header("Debug")]
        [ReadOnly, SerializeField] private float _sqrDistance;


        /*private void Update()
        {
            if (!isWorldUpdating) return;
        }*/

        public void AddObjectToWorld(SceneObject sceneObject)
        {
            staticObjects.Add(sceneObject);
            _drawDebug.AddSceneDrawable(sceneObject);
        }
        public void RemoveObjectToWorld(SceneObject sceneObject)
        {
            staticObjects.Remove(sceneObject);
            _drawDebug.RemoveSceneDrawable(sceneObject);
        }

        public void AddDynamicObjectToWorld(SceneObject sceneObject)
        {
            dynamicObjects.Add(sceneObject);
            _drawDebug.AddSceneDrawable(sceneObject);
        }
        public void RemoveDynamicObjectToWorld(SceneObject sceneObject)
        {
            dynamicObjects.Remove(sceneObject);
            _drawDebug.RemoveSceneDrawable(sceneObject);
        }

        public void AddEntityToWorld(Entity entity)
        {
            entities.Add(entity);
            _drawDebug.AddSceneDrawable(entity);
        }
        public void RemoveEntityToWorld(Entity entity)
        {
            entities.Remove(entity);
            _drawDebug.RemoveSceneDrawable(entity);
        }
    }
}
