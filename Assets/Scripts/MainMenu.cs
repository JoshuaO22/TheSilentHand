using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager gameManager = GameManager.Instance;
    public GameObject MainMenuPanel;
    public GameObject OptionsPanel;

    public void OnPlayButton()
    {
        Debug.Log("Play button clicked");
        gameManager.LoadScene(1); // Go to main game scene
    }

    public void OnOptionsButton()
    {
        Debug.Log("Options button clicked");
        // MainMenuPanel.SetActive(false);
        // OptionsPanel.SetActive(true);
    }

    public void OnQuitButton()
    {
        gameManager.QuitGame();
    }
}
