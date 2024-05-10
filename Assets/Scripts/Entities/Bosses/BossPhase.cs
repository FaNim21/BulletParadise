using UnityEngine;

namespace BulletParadise.Entities.Bosses
{
    [System.Serializable]
    public class BossPhase
    {
        public string name;
        public Phase phase;

        [MinMaxRange(0f, 1f)] public Vector2 percentage;


        public void Initialize(Boss bossBehavior)
        {
            phase.Initialize(bossBehavior);
        }
    }
}