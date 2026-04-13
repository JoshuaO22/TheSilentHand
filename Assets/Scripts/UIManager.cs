using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;
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
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }

    public void OnMainMenuButton()
    {
        Debug.Log("Main Menu button clicked");
        pauseAction.performed -= ctx => TogglePauseMenu();
        SceneManager.LoadScene(0); // Go to main menu scene
    }

    public void OnOptionsButton()
    {
        Debug.Log("Options button clicked");
        // Implement options menu logic here
    }

    public void OnQuitButton()
    {
        Debug.Log("Quit game.");
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}