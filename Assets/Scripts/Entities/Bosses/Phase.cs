using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.Entities.Bosses
{
    public abstract class Phase : ScriptableObject
    {
        protected Boss boss;
        protected Weapon weapon;


        public void Initialize(Boss boss, Weapon weapon)
        {
            this.boss = boss;
            this.weapon = weapon;
        }

        public abstract void OnEnter();
        public abstract void LogicUpdate(Vector2 targetDirection);
        public abstract void PhysicsUpdate(Rigidbody2D rb);
        public abstract void OnExit();

        public abstract void Draw();

        public virtual void OnReseting() { }
    }
}