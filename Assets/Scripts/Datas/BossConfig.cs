using BulletParadise.Entities.Bosses;
using UnityEngine;

namespace BulletParadise.Datas
{
    [CreateAssetMenu(fileName = "new BossConfig", menuName = "Configs/Boss")]
    public class BossConfig : ScriptableObject
    {
        public new string name;
        public Sprite sprite;
        public Vector2 bodyOffset;
        public float colliderRadius = 0.35f;

        public RuntimeAnimatorController animatorController;

        public int maxHealth;
        public float speed;

        public BossPhase[] phases;


        public void Initialize(Boss boss)
        {
            for (int i = 0; i < phases.Length; i++)
                phases[i].Initialize(boss);
        }

        public int GetRealPhaseCount()
        {
            int count = 0;
            for (int i = 0; i < phases.Length; i++)
                count += phases[i].phase.CountAsRealPhase();
            return count;
        }
    }
}