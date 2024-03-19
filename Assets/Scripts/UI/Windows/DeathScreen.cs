using BulletParadise.Player;
using UnityEngine;

namespace BulletParadise.UI.Windows
{
    public class DeathScreen : MonoBehaviour, IWindowControl
    {
        public bool IsActive => background.activeSelf;

        [Header("Components")]
        [SerializeField] private CanvasHandle canvasHandle;
        [SerializeField] private GameObject background;


        private void Awake()
        {
            canvasHandle.AddWindowToControl(this);
        }

        public void ToggleWindow()
        {
            background.SetActive(!IsActive);
        }

        public void Open()
        {
            background.SetActive(true);
        }
        public void Close()
        {
            background.SetActive(false);
        }
    }
}
