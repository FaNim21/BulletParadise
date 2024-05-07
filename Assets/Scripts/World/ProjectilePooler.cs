using BulletParadise.Entities;
using BulletParadise.Misc;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BulletParadise.World
{
    public class ProjectilePooler : MonoBehaviour
    {
        public static ProjectilePooler Instance { get; private set; }

        private ObjectPool<Projectile> _pool;
        private List<Projectile> _activeProjectiles = new();


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
        private void Start()
        {
            Utils.LogWarning("Creating pool for projectiles");
            _pool = new ObjectPool<Projectile>(() =>
            {
                return Instantiate(GameManager.Projectile, transform);
            }, null, projectile =>
            {
                projectile.gameObject.SetActive(false);
            }, null, false, 200, 1000);

            List<Projectile> startupProjectiles = new();
            for (int i = 0; i < 200; i++)
            {
                var projectile = _pool.Get();
                startupProjectiles.Add(projectile);
            }

            foreach (var projectile in startupProjectiles)
            {
                _pool.Release(projectile);
            }
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
                _pool.Release(_activeProjectiles[i]);
            }
        }

        public int GetProjectilesInPool() => _pool.CountAll;
        public int GetActiveProjectilesInPool() => _pool.CountActive;
    }
}
