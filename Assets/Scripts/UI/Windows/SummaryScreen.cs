using BulletParadise.Player;
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
        [SerializeField] private GameObject cheatedText;


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
            PlayerController.Instance.SetResponding(false);
            PlayerController.Instance.healthManager.SetInvulnerability(true);
            GameManager.Instance.worldManager.StopTimer();
            SetSummaryTimer(GameManager.Instance.worldManager.GetTimer());
            if (GameManager.Instance.worldManager.WasRunCheated()) cheatedText.SetActive(true);

            background.SetActive(true);
        }
        public void Close()
        {
            background.SetActive(false);
        }
    }
}
