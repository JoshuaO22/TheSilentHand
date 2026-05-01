using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private GameManager gameManager;
    public GameObject pauseMenu;
    public GameObject HUD;
    public GameObject deathScreen;
    public GameObject missionObjectivesPanel;
    [SerializeField] private MissionObjectiveUIHandler missionObjectiveUIHandler;
    private InputAction pauseAction;
    private InputAction[] inputActionsToDisable;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        pauseAction = InputSystem.actions.FindAction("PauseMenu");

        inputActionsToDisable = new InputAction[]
        {
            InputSystem.actions.FindAction("Attack"),
            InputSystem.actions.FindAction("Reload"),
            InputSystem.actions.FindAction("Move"),
            InputSystem.actions.FindAction("Look"),
            InputSystem.actions.FindAction("Jump"),
        };

        GetComponent<Canvas>().enabled = true;
        // DontDestroyOnLoad(gameObject); // TODO: CHECK LATER
    }

    void Start()
    {
        Debug.Log("UIManager Start");
        gameManager = GameManager.Instance;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        if (missionObjectiveUIHandler == null && missionObjectivesPanel != null)
        {
            missionObjectiveUIHandler = missionObjectivesPanel.GetComponent<MissionObjectiveUIHandler>();
        }

        Weapon CurrentWeapon = gameManager.PlayerController.GetComponentInChildren<Weapon>();
        if (CurrentWeapon != null)
        {
            OnAmmoChanged(CurrentWeapon.currentAmmo, CurrentWeapon.maxAmmo);
        }
        ProjectileWeapon CurrentProjectileWeapon = gameManager.PlayerController.GetComponentInChildren<ProjectileWeapon>();
        if (CurrentProjectileWeapon != null)
        {
            OnAmmoChanged(CurrentProjectileWeapon.currentAmmo, CurrentProjectileWeapon.maxAmmo);
        }
    }

    public void OnAmmoChanged(float currentAmmo, float maxAmmo)
    {
        HUD.transform.Find("CurrentWeapon/AmmoAmount").GetComponent<TMP_Text>().text = $"{currentAmmo}/{maxAmmo}";
    }

    private void ToggleGameState()
    {
        ToggleGameState(!gameManager.IsPaused);
    }
    private void ToggleGameState(bool isPaused)
    {
        gameManager.TogglePause(isPaused);
        Cursor.lockState = gameManager.IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = gameManager.IsPaused;

        foreach (var action in inputActionsToDisable)
        {
            if (gameManager.IsPaused)
            {
                action.Disable();
            }
            else
            {
                action.Enable();
            }
        }
    }

    private void TogglePauseMenu()
    {
        Debug.Log("Toggle pause menu");
        gameManager.TogglePause();
        Cursor.lockState = gameManager.IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = gameManager.IsPaused;
        pauseMenu.SetActive(gameManager.IsPaused);

        foreach (var action in inputActionsToDisable)
        {
            if (gameManager.IsPaused)
            {
                action.Disable();
            }
            else
            {
                action.Enable();
            }
        }

        Debug.Log(pauseMenu.activeSelf ? "Pause menu active" : "Pause menu inactive");
        Debug.Log("Game paused: " + gameManager.IsPaused);
    }

    public void OnPlayerDeath()
    {
        ToggleGameState(true);
        HUD.SetActive(false);
        deathScreen.SetActive(true);
    }

    public void OnResumeButton()
    {
        TogglePauseMenu();
    }

    public void OnRestartButton()
    {
        Debug.Log("Restart button clicked");
        gameManager.RestartGame(); // TODO: Implement RestartLevel
    }

    public void OnMainMenuButton()
    {
        Debug.Log("Main Menu button clicked");
        Instance = null;
        gameManager.LoadScene(0); // Go to main menu scene
        Destroy(gameObject);
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

    private void HandleObjectiveRequested(string objectiveText)
    {
        if (missionObjectiveUIHandler == null)
        {
            Debug.LogWarning("UIManager could not add objective because MissionObjectiveUIHandler is not assigned.");
            return;
        }

        missionObjectiveUIHandler.AddObjective(objectiveText);
        missionObjectivesPanel.SetActive(true);
    }

    public void OnEnable()
    {
        if (pauseAction == null) return;
        pauseAction.performed += OnPausePerformed;
        pauseAction.Enable();
        Teleporter.OnObjectiveRequested += HandleObjectiveRequested;
    }
    public void OnDisable()
    {
        if (pauseAction == null) return;
        pauseAction.performed -= OnPausePerformed;
        pauseAction.Disable();
        Teleporter.OnObjectiveRequested -= HandleObjectiveRequested;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePauseMenu();
    }
}