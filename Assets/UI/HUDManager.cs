using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }
    public GameObject ammo;
    public Image weaponIcon;

    private ProjectileWeapon currentWeapon;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateCurrentWeapon();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponentWithWarning(out PlayerCombat playerCombat))
        {
            if (playerCombat.currentWeapon != currentWeapon)
            {
                UpdateCurrentWeapon();
            }
        }
#endif
    }

    void UpdateCurrentWeapon()
    {
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponentWithWarning(out PlayerCombat playerCombat))
        {
            if (playerCombat.currentWeapon == null)
                return;

            weaponIcon.sprite = playerCombat.currentWeapon.icon;

            currentWeapon = (ProjectileWeapon)playerCombat.currentWeapon;
            UpdateAmmoDisplay(currentWeapon.ammo.CurrentAmmo, currentWeapon.ammo.MaxAmmo);

            if (currentWeapon != null)
            {
                currentWeapon.ammo.OnAmmoChanged += () => UpdateAmmoDisplay(currentWeapon.ammo.CurrentAmmo, currentWeapon.ammo.MaxAmmo);
            }
        }
    }

    void UpdateAmmoDisplay(int currentAmmo, int maxAmmo)
    {
        if (ammo == null)
            return;

        if (currentWeapon != null)
        {
            if (ammo.TryGetComponentWithWarning(out TextMeshProUGUI ammoText))
            {
                ammoText.text = $"{currentAmmo}/{maxAmmo}";
            }
        }
    }

    void OnEnable()
    {
        PlayerCombat.OnWeaponSwitched += UpdateCurrentWeapon;
    }

    void OnDisable()
    {
        if (currentWeapon != null)
        {
            currentWeapon.ammo.OnAmmoChanged -= () => UpdateAmmoDisplay(currentWeapon.ammo.CurrentAmmo, currentWeapon.ammo.MaxAmmo);
        }

        PlayerCombat.OnWeaponSwitched -= UpdateCurrentWeapon;
    }
}
