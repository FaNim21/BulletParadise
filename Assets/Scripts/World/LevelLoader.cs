﻿using BulletParadise.Datas;
using BulletParadise.Entities.Bosses;
using BulletParadise.Player;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BulletParadise.World
{
    public sealed class LevelLoader : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private Slider loadingSlider;
        [SerializeField] private GameObject background;

        [Header("Entering Scene vars")]
        private BossConfig bossConfig;


        public void LoadScene(string sceneName, BossConfig bossConfig = null)
        {
            this.bossConfig = bossConfig;
            GameManager.Instance.saveManager.SaveGame();
            PlayerController.Instance.SetResponding(false);
            PlayerController.Instance.healthManager.SetInvulnerability(false);
            background.SetActive(true);
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            Time.timeScale = 0f;
            ProjectilePooler.Instance.ReleaseAll();
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress);

                loadingText.SetText(Mathf.RoundToInt(progress * 100) + "%");
                loadingSlider.value = progress;

                yield return new WaitForEndOfFrame();
            }
            loadingText.SetText("100%");
            loadingSlider.value = 1;

            yield return null;
            SetUpOnNewScene();
            yield return null;

            if (bossConfig != null)
            {
                Boss boss = FindAnyObjectByType<Boss>();
                boss.SetupConfig(bossConfig);
            }

            background.SetActive(false);

            Time.timeScale = 1f;
            GameManager.Instance.worldManager.StartTimer();
        }

        private void SetUpOnNewScene()
        {
            GameManager.Instance.FindWorldManager();
            PlayerController.Instance.Restart();
            PlayerController.Instance.SetResponding(true);
        }
    }
}