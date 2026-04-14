using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject pauseMenu;
    public GameObject HUD;
    private InputAction pauseAction;
    private Weapon CurrentWeapon;

    void Awake()
    {
        pauseAction = InputSystem.actions.FindAction("PauseMenu");
    }

    void Start()
    {
        Debug.Log("UIManager Start");
        gameManager = GameManager.Instance;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

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

    public void OnEnable()
    {
        if (pauseAction == null) return;
        pauseAction.performed += OnPausePerformed;
        pauseAction.Enable();
    }
    public void OnDisable()
    {
        if (pauseAction == null) return;
        pauseAction.performed -= OnPausePerformed;
        pauseAction.Disable();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePauseMenu();
    }
}