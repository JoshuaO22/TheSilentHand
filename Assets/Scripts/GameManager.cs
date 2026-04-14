using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Implement RestartLevel
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public CharacterController PlayerController { get; private set; }
    public bool IsPaused;
    public bool IsGameOver;
    [SerializeField] private float _timeScale = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Debug.LogWarning("Multiple instances of GameManager detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        PlayerController = FindAnyObjectByType<CharacterController>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("GameManager Start");
        InitializeGame();
    }

    private void InitializeGame()
    {
        IsPaused = false;
        IsGameOver = false;
        Time.timeScale = _timeScale;
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = _timeScale;
    }

    public void TogglePause()
    {
        if (IsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void GameOver()
    {
        IsGameOver = true;
        PauseGame();
        Debug.Log("Game Over!");
    }

    public void RestartGame()
    {
        IsGameOver = false;
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        ResumeGame();
        SceneManager.LoadScene(sceneName);
    }
    public void LoadScene(int sceneIndex)
    {
        ResumeGame();
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game.");
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}