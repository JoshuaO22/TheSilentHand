using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public float damage = 25f;
    public float range = 100f;
    public float currentAmmo;
    public float maxAmmo = 30f;
    public bool hasAmmo => currentAmmo > 0;

    private GameManager gameManager = GameManager.Instance;
    private Camera MainCamera;
    private InputAction shootAction;
    private InputAction reloadAction;

    public static UnityAction<float, float> OnAmmoChangedEvent;

    private void Awake()
    {
        shootAction = InputSystem.actions.FindAction("Attack");
        reloadAction = InputSystem.actions.FindAction("Reload");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Debug.Log("Weapon Start");
        MainCamera = Camera.main;
        currentAmmo = maxAmmo;
    }

    private void Shoot()
    {
        if (!hasAmmo) return;

        RaycastHit hit;
        if (MainCamera != null && Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            currentAmmo--;
            OnAmmoChangedEvent?.Invoke(currentAmmo, maxAmmo);

            Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                Debug.Log("Enemy took damage: " + enemy.currentHealth + "/" + enemy.maxHealth);
            }
        }
    }

    private void Reload()
    {
        currentAmmo = maxAmmo;
        OnAmmoChangedEvent?.Invoke(currentAmmo, maxAmmo);
    }

    private void OnDrawGizmosSelected()
    {
        if (MainCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(MainCamera.transform.position, MainCamera.transform.forward * range);
        }
    }

    // TODO: on scene change, actions aren't set before script enables
    private void OnEnable()
    {
        if (shootAction == null || reloadAction == null) return;
        shootAction.Enable();
        reloadAction.Enable();
        shootAction.started += OnShootPerformed;
        reloadAction.started += OnReloadPerformed;
    }
    private void OnDisable()
    {
        if (shootAction == null || reloadAction == null) return;
        shootAction.Disable();
        reloadAction.Disable();
        shootAction.started -= OnShootPerformed;
        reloadAction.started -= OnReloadPerformed;
    }

    private void OnShootPerformed(InputAction.CallbackContext ctx)
    {
        Shoot();
    }

    private void OnReloadPerformed(InputAction.CallbackContext ctx)
    {
        Reload();
    }

}
