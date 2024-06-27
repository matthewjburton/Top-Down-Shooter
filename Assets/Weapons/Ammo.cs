using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo", menuName = "Weapons/Ammo")]
public class Ammo : ScriptableObject
{
    [Header("Ammo")]
    [SerializeField, Min(0)] int maxAmmo;
    int currentAmmo;
    public int MaxAmmo => maxAmmo;
    public int CurrentAmmo => currentAmmo;
    public event Action OnAmmoChanged;

    [Header("Reload")]
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject reloadPrefab;
    private GameObject reloadInstance;

    public void UseAmmo()
    {
        currentAmmo--;
        OnAmmoChanged?.Invoke();
    }

    public void SetAmmo(int ammo)
    {
        currentAmmo = ammo;
        OnAmmoChanged?.Invoke();
    }

    public bool NeedReload()
    {
        return currentAmmo <= 0;
    }

    public IEnumerator Reload(GameObject shooter)
    {
        if (reloadInstance != null)
        {
            yield break;
        }

        if (reloadPrefab != null)
        {
            reloadInstance = Instantiate(reloadPrefab, shooter.transform);
            reloadInstance.transform.localPosition = new Vector3(0, 1.5f, 0);
        }

        float elapsedTime = 0f;
        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;

            if (reloadInstance != null)
            {
                float progress = elapsedTime / reloadTime;
                reloadInstance.TryGetComponentWithWarning(out ReloadSlider slider);
                slider.SetSlider(progress);
            }

            yield return null;
        }

        SetAmmo(maxAmmo);

        if (reloadInstance != null)
        {
            Destroy(reloadInstance);
        }
    }
}