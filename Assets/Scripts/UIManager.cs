using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private GameManager gameManager;
    public GameObject pauseMenu;
    public GameObject HUD;
    public GameObject missionObjectivesPanel;
    [SerializeField] private MissionObjectiveUIHandler missionObjectiveUIHandler;
    private InputAction pauseAction;
    private Weapon CurrentWeapon;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        pauseAction = InputSystem.actions.FindAction("PauseMenu");
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

        CurrentWeapon = gameManager.PlayerController.GetComponentInChildren<Weapon>();
        if (CurrentWeapon != null) {
            CurrentWeapon.OnAmmoChanged += OnAmmoChanged;
            OnAmmoChanged(CurrentWeapon.currentAmmo, CurrentWeapon.maxAmmo);
        } else {
            Debug.LogWarning("Player's weapon not found. Ammo display will not update.");
        }
    }

    public void OnAmmoChanged(float currentAmmo, float maxAmmo)
    {
        HUD.transform.Find("CurrentWeapon/AmmoAmount").GetComponent<TMP_Text>().text = $"{currentAmmo}/{maxAmmo}";
    }

    private void TogglePauseMenu()
    {
        Debug.Log("Toggle pause menu");
        gameManager.TogglePause();
        Cursor.lockState = gameManager.IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = gameManager.IsPaused;
        Debug.Log(pauseMenu != null ? "Pause menu found" : "Pause menu not found");
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(gameManager.IsPaused);
        }
        Debug.Log(pauseMenu.activeSelf ? "Pause menu active" : "Pause menu inactive");
        Debug.Log("Game paused: " + gameManager.IsPaused);
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