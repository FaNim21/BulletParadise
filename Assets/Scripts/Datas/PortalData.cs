using UnityEngine;

namespace BulletParadise.Datas
{
    [CreateAssetMenu(fileName = "PortaData", menuName = "Configs/Portal")]
    public class PortalData : ScriptableObject
    {
        public string sceneName;
        public Sprite sprite;

        public BossConfig bossData;
    }
}
