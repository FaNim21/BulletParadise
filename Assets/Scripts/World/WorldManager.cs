using BulletParadise.Misc;
using System;
using System.Collections;
using UnityEngine;

namespace BulletParadise.World
{
    public class WorldManager : MonoBehaviour
    {
        public Transform playerSpawnPosition;

        private DungeonManager dungeonManager;


        private void Awake()
        {
            dungeonManager = GetComponent<DungeonManager>();
        }
        private void Start()
        {
            StartCoroutine(FindAfterEndOfFrame());
        }
        private IEnumerator FindAfterEndOfFrame()
        {
            yield return null;
            Utils.LogWarning("Loading in world manager");
            GameManager.Instance.saveManager.FindAllSavableObjects();
        }

        public void StartTimer()
        {
            if (dungeonManager == null) return;

            dungeonManager.StartTimer();
        }
        public void StopTimer()
        {
            if (dungeonManager == null) return;

            dungeonManager.StopTimer();
        }

        public TimeSpan GetTimer()
        {
            return dungeonManager.GetTimer();
        }

        public void RunCheated()
        {
            if (dungeonManager == null) return;

            dungeonManager.Cheated();
        }
        public bool WasRunCheated()
        {
            if (dungeonManager == null) return false;

            return dungeonManager.WasCheated();
        }
    }
}
