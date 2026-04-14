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

    public event UnityAction<float, float> OnAmmoChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainCamera = Camera.main;
        shootAction = InputSystem.actions.FindAction("Attack");
        shootAction.Enable();
        shootAction.started += ctx => Shoot();

        reloadAction = InputSystem.actions.FindAction("Reload");
        reloadAction.Enable();
        reloadAction.started += ctx => Reload();

        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Shoot()
    {
        if (!hasAmmo) return;

        RaycastHit hit;
        if (MainCamera != null && Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            currentAmmo--;
            OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);

            Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
            if (enemy != null) {
                enemy.TakeDamage(damage);

                Debug.Log("Enemy took damage: " + enemy.currentHealth + "/" + enemy.maxHealth);
            }
        }
    }

    private void Reload()
    {
        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }

    private void OnDrawGizmosSelected()
    {
        if (MainCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(MainCamera.transform.position, MainCamera.transform.forward * range);
        }
    }

    public void OnEnable()
    {
        shootAction.started += ctx => Shoot();
        reloadAction.started += ctx => Reload();
    }
    private void OnDisable()
    {
        shootAction.started -= ctx => Shoot();
        reloadAction.started -= ctx => Reload();
    }
}
