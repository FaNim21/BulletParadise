using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.Entities.Bosses
{
    public abstract class Phase : ScriptableObject
    {
        protected BossBehavior bossBehavior;
        protected Weapon weapon;


        public void Initialize(BossBehavior bossBehavior, Weapon weapon)
        {
            this.bossBehavior = bossBehavior;
            this.weapon = weapon;
        }

        public abstract void OnEnter();
        public abstract void Update();
        public abstract void FixedUpdate(Rigidbody2D rb);
        public abstract void OnExit();

        public virtual void OnReseting() { }
    }
}