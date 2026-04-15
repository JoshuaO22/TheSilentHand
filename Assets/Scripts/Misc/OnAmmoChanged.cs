using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnAmmoChanged : MonoBehaviour
{
    TMP_Text textComponent;

    // UIManager handles the initial ammo amounts at start
    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();

        Weapon.OnAmmoChanged += HandleAmmoChanged;
        ProjectileWeapon.OnAmmoChangedEvent += HandleAmmoChanged;
    }

    private void OnDestroy()
    {
        Weapon.OnAmmoChanged -= HandleAmmoChanged;
        ProjectileWeapon.OnAmmoChangedEvent -= HandleAmmoChanged;
    }

    private void HandleAmmoChanged(float newAmmo, float maxAmmo)
    {
        if (textComponent != null) {
            textComponent.text = $"{newAmmo}/{maxAmmo}";
        } else {
            Debug.LogWarning("OnAmmoChanged: No TMP_Text component found on this GameObject.");
        }
    }
}