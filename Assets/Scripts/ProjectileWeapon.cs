
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.VisualScripting;
using Unity.Entities.UniversalDelegates;

// Refactored heavily from: https://www.youtube.com/watch?v=wZ2UUOC17AY
public class ProjectileWeapon : MonoBehaviour
{
    public GameObject bullet;
    public Rigidbody playerRb;
    public Transform attackPoint;
    public GameObject muzzleFlash;
    public float shootForce, upwardForce;
    public float spread, reloadTime, timeBetweenShots;
    public float rpm;
    public int maxAmmo, bulletsPerTap;
    public float recoilForce;
    public bool isAutomatic;
    public int currentAmmo { get; private set; }
    private int currentAmmoDisplay => currentAmmo / bulletsPerTap;
    private int maxAmmoDisplay => maxAmmo / bulletsPerTap;
    private float nextTimeToShoot;
    private int bulletsShot;
    private bool shooting, canShoot, reloading;
    private float fireRate;

    private Camera mainCamera;
    public static UnityAction<float, float> OnAmmoChangedEvent;
    private InputAction shootAction;
    private InputAction reloadAction;
    
    private void Awake()
    {
        currentAmmo = maxAmmo;
        canShoot = true;

        fireRate = 60 / rpm;

        // Warn if timeBetweenShots is too high for the given fire rate and bullets per tap, which can cause burst shots to fire slower than intended
        if (bulletsPerTap > 1 && timeBetweenShots >= fireRate)
        {
            Debug.LogWarning($"ProjectileWeapon: timeBetweenShots ({timeBetweenShots:F3}s) >= fireRate ({fireRate:F3}s). " +
                $"Burst shots may fire slower than intended. Set timeBetweenShots < {fireRate:F3}.", gameObject);
        }

        shootAction = InputSystem.actions.FindAction("Attack");
        reloadAction = InputSystem.actions.FindAction("Reload");
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        if (shootAction == null || reloadAction == null) return;
        shootAction.Enable();
        reloadAction.Enable();
        shootAction.started += onShootStarted;
        shootAction.canceled += onShootCanceled;
        reloadAction.performed += onReloadPerformed;
    }

    private void OnDisable()
    {
        if (shootAction == null || reloadAction == null) return;
        shootAction.Disable();
        reloadAction.Disable();
        shootAction.started -= onShootStarted;
        shootAction.canceled -= onShootCanceled;
        reloadAction.performed -= onReloadPerformed;
    }
    private void Update()
    {
        if (shooting && canShoot && !reloading && currentAmmo > 0 && Time.time >= nextTimeToShoot)
        {
            // nextTimeToShoot = Time.time + fireRate; // todo: check later
            while (Time.time >= nextTimeToShoot)
            {
                nextTimeToShoot += fireRate;
                bulletsShot = 0;
                Shoot();
            }
        }
        else
        {
            nextTimeToShoot = Time.time;
        }
    }

    private void onReloadPerformed(InputAction.CallbackContext ctx)
    {
        if (currentAmmo < maxAmmo && !reloading) Reload();
    }

    private void onShootStarted(InputAction.CallbackContext ctx)
    {
        //Reload automatically when trying to shoot without ammo
        if (canShoot && !reloading && currentAmmo <= 0) {
            Reload();
            return;
        }

        shooting = true;
    }
    private void onShootCanceled(InputAction.CallbackContext ctx)
    {
        shooting = false;
    }
    
    private void Shoot()
    {
        canShoot = false;
        
        //Find the exact hit position using a raycast
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        RaycastHit hit;
        
        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player
            
            //Calculate direction from attackPoint to targetPoint
            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        
        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        
        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction
        
        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;
        
        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(mainCamera.transform.up * upwardForce, ForceMode.Impulse);
        
        //Instantiate muzzle flash if exists
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        
        currentAmmo--;
        bulletsShot++;
        
        OnAmmoChangedEvent?.Invoke(currentAmmoDisplay, maxAmmoDisplay);
        
        if (playerRb != null && bulletsShot == 1)
        {
            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && currentAmmo > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
        else
            canShoot = true;
    }
    
    private void Reload()
    {
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }
    private void ReloadFinished()
    {
        currentAmmo = maxAmmo;
        reloading = false;
        OnAmmoChangedEvent?.Invoke(currentAmmoDisplay, maxAmmoDisplay);
    }
}
