using BulletParadise.Datas;
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
            background.SetActive(true);

            this.bossConfig = bossConfig;
            BeforeLoadingScene();
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
            AfterSceneLoaded();
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

        private void BeforeLoadingScene()
        {
            GameManager.Instance.saveManager.SaveGame();
            PlayerController.Instance.SetResponding(false);
            PlayerController.Instance.healthManager.SetInvulnerability(false);
        }
        private void AfterSceneLoaded()
        {
            GameManager.Instance.FindWorldManager();
            PlayerController.Instance.Restart();
        }
    }
}