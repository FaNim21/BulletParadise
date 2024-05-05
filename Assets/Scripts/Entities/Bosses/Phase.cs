using BulletParadise.Components;
using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.Entities.Bosses
{
    public abstract class Phase : ScriptableObject
    {
        protected Boss boss;
        protected ShootingManager shootingManager;


        public void Initialize(Boss boss)
        {
            this.boss = boss;
            shootingManager = boss.GetComponent<ShootingManager>();
        }

        public abstract void OnEnter();
        public abstract void LogicUpdate(Weapon weapon, Vector2 targetDirection);
        public abstract void PhysicsUpdate(Rigidbody2D rb);
        public abstract void OnExit();

        public abstract void Draw();

        public abstract int CountAsRealPhase();
    }
}