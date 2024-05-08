using BulletParadise.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BulletParadise.World
{
    public class ProjectilePooler : MonoBehaviour
    {
        public static ProjectilePooler Instance { get; private set; }

        public int initializeCount = 200;

        private ObjectPool<Projectile> _pool;
        private List<Projectile> _activeProjectiles = new();


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
        private IEnumerator Start()
        {
            yield return null;
            /*Utils.LogWarning("Creating pool for projectiles");
            _pool = new ObjectPool<Projectile>(() =>
            {
                return Instantiate(GameManager.Projectile, transform);
            }, null, projectile =>
            {
                projectile.Restart();
            }, null, false, initializeCount, 1000);

            yield return null;*/

            /*List<Projectile> startupProjectiles = new();
            for (int i = 0; i < initializeCount; i++)
            {
                var projectile = _pool.Get();
                startupProjectiles.Add(projectile);
                yield return null;
            }

            foreach (var projectile in startupProjectiles)
            {
                _pool.Release(projectile);
                yield return null;
            }*/
        }

        public Projectile GetProjectile()
        {
            Projectile projectile = _pool.Get();
            _activeProjectiles.Add(projectile);
            return projectile;
        }

        public void Release(Projectile projectile)
        {
            _pool.Release(projectile);
            _activeProjectiles.Remove(projectile);
        }

        public void ReleaseAll()
        {
            for (int i = 0; i < _activeProjectiles.Count; i++)
            {
                _activeProjectiles[i].Destroy();
            }
        }

        public int GetProjectilesInPool() => _pool.CountAll;
        public int GetActiveProjectilesInPool() => _pool.CountActive;
    }
}
