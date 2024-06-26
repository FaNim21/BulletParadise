using BulletParadise.Datas;
using BulletParadise.Entities;
using BulletParadise.Player;
using BulletParadise.World;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletParadise.UI
{
    public class EnterInformationWindow : MonoBehaviour
    {
        [Header("Componenets")]
        public LevelLoader levelLoader;

        [Header("Windows components")]
        public GameObject background;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI bossNameText;
        public Image bossImage;
        public TextMeshProUGUI bossDescriptionText;
        public TextMeshProUGUI statsText;

        [Header("Values")]
        [SerializeField] private string sceneName;
        [SerializeField] private Portal currentPortal;


        private void Start()
        {
            levelLoader = FindFirstObjectByType<LevelLoader>();
        }
        public void Setup(Portal portal)
        {
            currentPortal = portal;
            LoadUpWindow();
            background.SetActive(true);
        }

        public void Exit()
        {
            background.SetActive(false);
        }
        public void OnEnter()
        {
            if (!currentPortal.canEnter) return;
            currentPortal.stats.attempts++;
            GameManager.Instance.currentEnteredPortalID = currentPortal.data.id;
            background.SetActive(false);
            PlayerController.Instance.isInLobby = false;
            levelLoader.LoadScene(sceneName, currentPortal.data.bossData);
        }

        private void LoadUpWindow()
        {
            sceneName = currentPortal.data.sceneName;
            titleText.SetText(currentPortal.data.name);

            BossConfig boss = currentPortal.data.bossData;
            bossNameText.SetText(boss.name);
            bossImage.sprite = boss.sprite;

            bossDescriptionText.SetText($"Health: {boss.maxHealth}\n" +
                                        $"Phases amount: {boss.GetRealPhaseCount()}\n" +
                                        $"");

            TimeSpan timer = TimeSpan.FromMilliseconds(currentPortal.stats.timerMiliseconds);
            string timerText = string.Format("{0:D2}:{1:D2}:{2:D3}", timer.Minutes, timer.Seconds, timer.Milliseconds);
            if (currentPortal.stats.completions == 0)
                timerText = $"<color=red>UNFINISHED</color>({timerText})";

            statsText.SetText($"Attempts: {currentPortal.stats.attempts}\n" +
                              $"Deaths: {currentPortal.stats.deaths}\n" +
                              $"Completions: {currentPortal.stats.completions}\n" +
                              $"Best time: {timerText}\n");
        }
    }
}
