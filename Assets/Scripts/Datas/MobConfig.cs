using UnityEngine;

namespace BulletParadise.Datas
{
    [CreateAssetMenu(fileName = "new MobConfig", menuName = "Configs/Mob")]
    public class MobConfig : ScriptableObject
    {
        public int maxHealth;
    }
}