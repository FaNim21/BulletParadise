using UnityEngine;

namespace BulletParadise.Datas
{
    [CreateAssetMenu(fileName = "new BossConfig", menuName = "Configs/Boss")]
    public class BossConfig : ScriptableObject
    {
        public int maxHealth;
    }
}