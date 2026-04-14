using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager = GameManager.Instance;
    public GameObject pauseMenu;
    public GameObject HUD;
    private InputAction pauseAction;

    void Start()
    {
        Debug.Log("UIManager Start");
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        pauseAction = InputSystem.actions.FindAction("PauseMenu");
        pauseAction.Enable();
        pauseAction.performed += ctx => TogglePauseMenu();

        Weapon CurrentWeapon = gameManager.PlayerController.GetComponentInChildren<Weapon>();
        if (CurrentWeapon != null) {
            CurrentWeapon.OnAmmoChanged += OnAmmoChanged;
            OnAmmoChanged(CurrentWeapon.currentAmmo, CurrentWeapon.maxAmmo);
        } else {
            Debug.LogWarning("Player's weapon not found. Ammo display will not update.");
        }
    }

    void Update()
    {
        
    }

    public void OnAmmoChanged(float currentAmmo, float maxAmmo)
    {
        HUD.transform.Find("CurrentWeapon/AmmoAmount").GetComponent<TMP_Text>().text = $"{currentAmmo}/{maxAmmo}";
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
    public void OnEnable()
    {
        pauseAction.performed += ctx => TogglePauseMenu();
    }
    public void OnDisable()
    {
        pauseAction.performed -= ctx => TogglePauseMenu();
    }
}