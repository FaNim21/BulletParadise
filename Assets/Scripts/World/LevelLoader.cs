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
        private PlayerController player;

        [Header("Objects")]
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private Slider loadingSlider;
        [SerializeField] private GameObject background;


        private void Start()
        {
            player = PlayerController.Instance;
        }

        public void LoadScene(string sceneName)
        {
            background.SetActive(true);
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                loadingText.SetText(Mathf.RoundToInt(progress * 100) + "%");
                loadingSlider.value = progress;

                yield return new WaitForEndOfFrame();
            }

            //TODO: 0 Zrobic tutaj wyjscie z ladowania i zarazem jako aktywowanie sceny w formie press any to continue
            yield return new WaitForSeconds(0.25f);

            background.SetActive(false);
        }
    }
}