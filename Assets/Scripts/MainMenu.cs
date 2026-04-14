using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject MainMenuPanel;
    public GameObject OptionsPanel;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found in MainMenu.");
        }
    }
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
