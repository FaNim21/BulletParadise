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
        private PlayerController player;

        [Header("Components")]
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private Slider loadingSlider;


        private void Start()
        {
            player = PlayerController.Instance;
        }

        public void LoadScene(int sceneNumber)
        {
            StartCoroutine(LoadSceneAsync(sceneNumber));
        }

        private IEnumerator LoadSceneAsync(int sceneIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                loadingText.SetText(Mathf.RoundToInt(progress * 100) + "%");
                loadingSlider.value = progress;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}