using BulletParadise.Entities;
using BulletParadise.Player;
using BulletParadise.World;
using TMPro;
using UnityEngine;

public class EnterInformationWindow : MonoBehaviour
{
    [Header("Componenets")]
    public LevelLoader levelLoader;

    [Header("Windows components")]
    public GameObject background;
    public TextMeshProUGUI titleText;

    [Header("Values")]
    public string sceneName;


    private void Start()
    {
        levelLoader = FindFirstObjectByType<LevelLoader>();
    }
    public void Setup(Portal portal)
    {
        sceneName = portal.name;
        LoadUpWindow(portal);
        background.SetActive(true);
    }

    public void Exit()
    {
        background.SetActive(false);
    }
    public void OnEnter()
    {
        background.SetActive(false);
        PlayerController.Instance.isInLobby = false;
        levelLoader.LoadScene(sceneName);
    }

    private void LoadUpWindow(Portal portal)
    {
        titleText.SetText(sceneName);
        //TODO: 0 Ladowac tu wszystkie informacje o bosie i achievementach zrobionych i do zrobienia w tym portalu
    }
}
