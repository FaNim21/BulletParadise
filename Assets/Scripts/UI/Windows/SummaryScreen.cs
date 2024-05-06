using System;
using TMPro;
using UnityEngine;

namespace BulletParadise.UI.Windows
{
    public class SummaryScreen : MonoBehaviour, IWindowControl
    {
        public bool IsActive => background.activeSelf;

        [Header("Components")]
        [SerializeField] private CanvasHandle canvasHandle;
        [SerializeField] private GameObject background;
        [SerializeField] private TextMeshProUGUI timerSpendText;


        private void Awake()
        {
            canvasHandle.AddWindowToControl(this);
        }

        public void SetSummaryTimer(TimeSpan timer)
        {
            timerSpendText.SetText($"time spend: {timer:mm':'ss'.'ff}");
        }

        public void ToggleWindow()
        {
            background.SetActive(!IsActive);
        }

        public void Open()
        {
            Time.timeScale = 0f;
            GameManager.Instance.worldManager.StopTimer();
            SetSummaryTimer(GameManager.Instance.worldManager.GetTimer());
            background.SetActive(true);
        }
        public void Close()
        {
            background.SetActive(false);
        }
    }
}
