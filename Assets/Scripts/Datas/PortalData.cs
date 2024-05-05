using UnityEngine;

namespace BulletParadise.Datas
{
    [CreateAssetMenu(fileName = "PortaData", menuName = "Configs/Portal")]
    public class PortalData : ScriptableObject
    {
        public new string name;
        public string sceneName;
        public Sprite sprite;

        public BossConfig bossData;
    }
}
