using BulletParadise.DataManagement;
using System;
using UnityEngine;

namespace BulletParadise.World
{
    public class DungeonManager : MonoBehaviour
    {
        private TimeSpan timerSpan;

        [Header("Debug")]
        [SerializeField, ReadOnly] private float timer;
        [SerializeField, ReadOnly] private bool wasCheated;
        [SerializeField, ReadOnly] private bool isWorking;
        [SerializeField, ReadOnly] private bool isCompleted;

        private void Start()
        {
            wasCheated = GameManager.Instance.drawDebug.AreDebugLinesVisible();
        }
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

            if (GameManager.Instance.worldManager.WasRunCheated()) return;

            bool canSaveTimer = (stats.completions == 0 && stats.timerMiliseconds <= timerSpan.TotalMilliseconds) ||
                (isCompleted && (stats.completions == 1 || stats.timerMiliseconds > timerSpan.TotalMilliseconds));

            if (canSaveTimer) stats.timerMiliseconds = timerSpan.TotalMilliseconds;
        }

        public TimeSpan GetTimer()
        {
            return TimeSpan.FromSeconds(timer);
        }

        public void Cheated() => wasCheated = true;
        public bool WasCheated() => wasCheated;

        public void Completed() => isCompleted = true;

    }
}
