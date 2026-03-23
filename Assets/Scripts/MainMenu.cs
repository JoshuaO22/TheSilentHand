using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject OptionsPanel;

    public void OnPlayButton()
    {
        Debug.Log("Play button clicked");
        SceneManager.LoadScene(1); // Go to main game scene
    }

    public void OnOptionsButton()
    {
        Debug.Log("Options button clicked");
        // MainMenuPanel.SetActive(false);
        // OptionsPanel.SetActive(true);
    }

    public void OnQuitButton()
    {
        Debug.Log("Quit game.");
        Application.Quit();
    }
}
