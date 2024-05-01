using BulletParadise.Shooting;
using UnityEngine;

namespace BulletParadise.Entities.Bosses
{
    [System.Serializable]
    public class BossPhase
    {
        public Phase phase;
        public Weapon weapon;

        [MinMaxRange(0f, 1f)] public Vector2 percentage;


        public void Initialize(Boss bossBehavior)
        {
            phase.Initialize(bossBehavior, weapon);
        }
    }
}