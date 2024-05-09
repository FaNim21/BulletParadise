using UnityEngine;

namespace BulletParadise.Datas
{
    [CreateAssetMenu(fileName = "new BossShootAnimData", menuName = "Boss/ShootAnimData")]
    public class BossShootAnimData : ScriptableObject
    {
        public AnimationClip clip;
        public float shootDelay;
    }
}
