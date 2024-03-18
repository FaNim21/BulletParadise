using BulletParadise.World;
using TMPro;
using UnityEngine;

public class EnterInformationWindow : MonoBehaviour
{
    [Header("Componenets")]
    public LevelLoader levelLoader;
    public GameObject background;
    public TextMeshProUGUI titleText;

    [Header("Values")]
    public string sceneName;


    public void Setup(string sceneName)
    {
        this.sceneName = sceneName;
        titleText.SetText(sceneName);
        background.SetActive(true);
    }

    public void Exit()
    {
        background.SetActive(false);
    }

    public void OnEnter()
    {
        background.SetActive(false);
        levelLoader.LoadScene(sceneName);
    }
}
