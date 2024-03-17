using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterInformationWindow : MonoBehaviour
{
    [Header("Componenets")]
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
        SceneManager.LoadScene(sceneName);
    }
}
