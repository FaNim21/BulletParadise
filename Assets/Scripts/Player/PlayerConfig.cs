using UnityEngine;

namespace BulletParadise.Player
{
    [CreateAssetMenu(fileName = "new PlayerConfig", menuName = "Configs/Player")]
    public class PlayerConfig : ScriptableObject
    {
        public int maxHealth;
        public float speed;
    }
}
