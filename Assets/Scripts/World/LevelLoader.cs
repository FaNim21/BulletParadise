using BulletParadise.Misc;
using BulletParadise.Player;
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
            PlayerController.Instance.isResponding = false;
            background.SetActive(true);
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            //TODO: 0 before loading new scene clear the drawdebug sceneDrawables as from previous scene objects draws

            Time.timeScale = 0f;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress);

                loadingText.SetText(Mathf.RoundToInt(progress * 100) + "%");
                loadingSlider.value = progress;

                yield return new WaitForEndOfFrame();
            }
            Utils.Log("Szukanie world managera");
            GameManager.Instance.FindWorldManager();

            //TODO: 0 Zrobic tutaj wyjscie z ladowania i zarazem jako aktywowanie sceny w formie press any to continue
            loadingText.SetText("100%");
            loadingSlider.value = 1;
            yield return new WaitForSecondsRealtime(0.25f);

            Utils.Log("Resetowanie gracza");
            PlayerController.Instance.Restart();
            PlayerController.Instance.isResponding = true;

            background.SetActive(false);
            yield return new WaitForSecondsRealtime(0.1f);
            Utils.Log("Startowanie sceny");
            Time.timeScale = 1f;
        }
    }
}