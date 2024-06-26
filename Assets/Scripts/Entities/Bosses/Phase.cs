using BulletParadise.Components;
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
        public abstract void LogicUpdate(Vector2 targetDirection);
        public abstract void PhysicsUpdate(Rigidbody2D rb);
        public abstract void OnExit();

        public virtual void Draw() { }

        public virtual bool UpdatePhaseOnHit() => true;
        public virtual int CountAsRealPhase() => 1;
    }
}