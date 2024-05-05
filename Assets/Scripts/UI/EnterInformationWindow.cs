using BulletParadise.Datas;
using BulletParadise.Entities;
using BulletParadise.Player;
using BulletParadise.World;
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
            background.SetActive(false);
            PlayerController.Instance.isInLobby = false;
            levelLoader.LoadScene(sceneName, currentPortal.data.bossData);
        }

        private void LoadUpWindow()
        {
            sceneName = currentPortal.data.sceneName;
            titleText.SetText(sceneName);

            BossConfig boss = currentPortal.data.bossData;
            bossNameText.SetText(boss.name);
            bossImage.sprite = boss.sprite;

            bossDescriptionText.SetText($"Health: {boss.maxHealth}\n" +
                                        $"Phases amount: {boss.GetRealPhaseCount()}\n" +
                                        $"");

            //TODO: 0 Ladowac tu wszystkie informacje o bosie i achievementach zrobionych i do zrobienia w tym portalu
        }
    }
}
