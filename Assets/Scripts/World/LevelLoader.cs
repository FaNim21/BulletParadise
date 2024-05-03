﻿using BulletParadise.Player;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BulletParadise.World
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private Slider loadingSlider;
        [SerializeField] private GameObject background;


        public void LoadScene(string sceneName)
        {
            PlayerController.Instance.SetResponding(false);
            background.SetActive(true);
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            Time.timeScale = 0f;
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

            //TODO: 0 Zrobic tutaj wyjscie z ladowania i zarazem jako aktywowanie sceny w formie press any to continue
            background.SetActive(false);
        }

        private void SetUpOnNewScene()
        {
            GameManager.Instance.FindWorldManager();

            PlayerController.Instance.Restart();
            PlayerController.Instance.SetResponding(true);

            Time.timeScale = 1f;
        }
    }
}