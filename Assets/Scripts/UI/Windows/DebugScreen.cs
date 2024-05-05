using UnityEngine;

namespace BulletParadise.UI.Windows
{
    public class DebugScreen : MonoBehaviour
    {
        private GameObject background;


        private void Awake()
        {
            background = transform.GetChild(0).gameObject;
        }

        public void SwitchVisibility()
        {
            GameManager.Instance.drawDebug.SwitchDebugMode();
            background.SetActive(!background.activeSelf);
        }
    }
}
