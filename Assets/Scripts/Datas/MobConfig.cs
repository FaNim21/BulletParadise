using UnityEngine;

namespace BulletParadise.Datas
{
    [CreateAssetMenu(fileName = "new MobConfig", menuName = "Configs/Mob")]
    public class MobConfig : ScriptableObject
    {
        public Sprite sprite;

        public int maxHealth;
        public float speed;

        public float chaseSpeed;
        public Vector2 roamDistance;
    }
}