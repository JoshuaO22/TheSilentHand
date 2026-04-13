using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager = GameManager.Instance;
    public GameObject pauseMenu;
    private InputAction pauseAction;

    void Start()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        pauseAction = InputSystem.actions.FindAction("PauseMenu");
        pauseAction.Enable();
        pauseAction.performed += ctx => TogglePauseMenu();
    }

    void Update()
    {
        
    }

    public void TogglePauseMenu()
    {
        Debug.Log("Toggle pause menu");
        gameManager.TogglePause();
        Cursor.lockState = gameManager.IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = gameManager.IsPaused;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(gameManager.IsPaused);
        }
    }

    public void OnResumeButton()
    {
        TogglePauseMenu();
    }

    public void OnRestartButton()
    {
        Debug.Log("Restart button clicked");
        pauseAction.performed -= ctx => TogglePauseMenu();
        gameManager.RestartGame(); // TODO: Implement RestartLevel
    }

    public void OnMainMenuButton()
    {
        Debug.Log("Main Menu button clicked");
        pauseAction.performed -= ctx => TogglePauseMenu();
        gameManager.LoadScene(0); // Go to main menu scene
    }

    public void OnOptionsButton()
    {
        Debug.Log("Options button clicked");
        // Implement options menu logic here
    }

    public void OnQuitButton()
    {
        gameManager.QuitGame();
    }
}