using BulletParadise.Constants;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BulletParadise.UI
{
    public class MainMenu : MonoBehaviour, IWindowControl
    {
        public bool IsActive => background.activeSelf;

        [Header("Components")]
        [SerializeField] private CanvasHandle canvasHandle;
        [SerializeField] private GameObject background;

        [Header("Debug")]
        [ReadOnly, SerializeField] private bool isCanvasEnabled;


        private void Awake()
        {
            canvasHandle.AddWindowToControl(this);
        }

        public void ToggleWindow()
        {
            if (isCanvasEnabled && !IsActive) return;

            background.SetActive(!IsActive);
            ChangeTimeScale();
        }

        public void Open() { }
        public void Close() { }

        private void ChangeTimeScale()
        {
            isCanvasEnabled = !isCanvasEnabled;
            canvasHandle.isCanvasEnabled = isCanvasEnabled;
            Consts.IsFocusedOnMainMenu = !Consts.IsFocusedOnMainMenu;
            Time.timeScale = isCanvasEnabled ? 0 : 1;
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }

    }
}