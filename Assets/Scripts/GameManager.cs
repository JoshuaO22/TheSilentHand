using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// TODO: Implement RestartLevel
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public CharacterController PlayerController { get; private set; }
    public bool IsPaused;
    public bool IsGameOver;
    [SerializeField] private float _timeScale = 1f;

    public event UnityAction OnPlayerChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Debug.LogWarning("Multiple instances of GameManager detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("GameManager Instance set");
        FindPlayer();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("GameManager Start");
        InitializeGame();
    }

    // private void Update()
    // {
    //     if (PlayerController == null) {
    //         PlayerController = FindAnyObjectByType<CharacterController>();
    //     }
    // }

    public void SetPlayer(CharacterController newPlayer)
    {
        PlayerController = newPlayer;
        OnPlayerChanged?.Invoke();
    }

    private void FindPlayer()
    {
        PlayerController = FindAnyObjectByType<CharacterController>();
        if (PlayerController == null)
        {
            Debug.LogError("PlayerController not found in GameManager.");
        }
        else
        {
            Debug.Log("PlayerController found in GameManager.");
            OnPlayerChanged?.Invoke();
        }
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
        Debug.Log("Game paused.");
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = _timeScale;
        Debug.Log("Resuming game.");
        // PlayerController = FindAnyObjectByType<CharacterController>();
        // if (PlayerController == null)
        // {
        //     Debug.LogError("PlayerController not found when resuming game.");
        // }
    }

    public void TogglePause()
    {
        TogglePause(!IsPaused);
    }
    public void TogglePause(bool IsPaused)
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
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        ResumeGame();
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        ResumeGame();
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

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        FindPlayer();
    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}