using BulletParadise.DataManagement;
using System;
using TMPro;
using UnityEngine;

namespace BulletParadise.World
{
    public class DungeonManager : MonoBehaviour
    {
        public TextMeshProUGUI timerText;

        private TimeSpan timerSpan;

        [Header("Debug")]
        [SerializeField, ReadOnly] private float timer;
        [SerializeField, ReadOnly] private bool isWorking;


        private void Update()
        {
            if (!isWorking) return;

            timer += Time.deltaTime;
        }

        public void StartTimer()
        {
            isWorking = true;
        }
        public void StopTimer()
        {
            if (!isWorking) return;
            isWorking = false;

            PortalStats stats = GameManager.Instance.GetEnteredPortalStats();
            timerSpan = TimeSpan.FromSeconds(timer);

            if (stats.completions == 0)
            {
                if (stats.timer.totalMiliseconds > timerSpan.TotalMilliseconds) return;
            }
            else if (stats.completions > 1)
            {
                if (stats.timer.totalMiliseconds <= timerSpan.TotalMilliseconds) return;
            }

            stats.timer.minutes = timerSpan.Minutes + (timerSpan.Hours * 60);
            stats.timer.seconds = timerSpan.Seconds;
            stats.timer.milliseconds = timerSpan.Milliseconds;
            stats.timer.totalMiliseconds = timerSpan.TotalMilliseconds;
        }

        public TimeSpan GetTimer()
        {
            return TimeSpan.FromSeconds(timer);
        }
    }
}
