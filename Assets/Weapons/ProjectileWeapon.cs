using System.Collections;
using TMPro;
using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    [Header("Projectile Settings")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField, Min(0)] protected float projectileSpeed;

    [Header("Ammo Settings")]
    [SerializeField, Min(0)] protected int maxAmmo;
    [SerializeField, Min(0)] protected float reloadTime;
    [SerializeField] GameObject reloadPrefab;
    protected int ammo;
    protected GameObject reloadInstance;

    [Header("Audio")]
    [SerializeField] protected AudioClip[] shootSounds;

    void OnEnable()
    {
        ammo = maxAmmo;
        GameObject.Find("Ammo").TryGetComponentWithWarning(out TextMeshProUGUI text);
        text.text = $"{ammo}/{maxAmmo}";
    }

    public override void Attack(GameObject attacker)
    {
        Shoot(attacker);
    }

    public virtual void Shoot(GameObject shooter)
    {
        if (NeedReload(shooter))
        {
            return;
        }

        Vector3 targetPosition = GetTargetPosition();
        Vector2 direction = CalculateDirection(shooter.transform.position, targetPosition);

        GameObject newProjectile = SpawnProjectile(shooter);

        ApplyVelocity(newProjectile, direction);

        SoundManager.Instance.PlayRandomSound(shootSounds, shooter.transform);

        if (NeedReload(shooter))
        {
            return;
        }
    }

    protected virtual IEnumerator Reload(GameObject shooter)
    {
        if (reloadInstance != null)
        {
            yield break;
        }

        // Instantiate the reload slider
        if (reloadPrefab != null)
        {
            reloadInstance = Instantiate(reloadPrefab, shooter.transform);
            reloadInstance.transform.localPosition = new Vector3(0, 1.5f, 0); // Position above the shooter
        }

        float elapsedTime = 0f;
        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;

            // Update reload progress
            if (reloadInstance != null)
            {
                float progress = elapsedTime / reloadTime;
                reloadInstance.TryGetComponentWithWarning(out ReloadSlider slider);
                slider.SetSlider(progress);
            }

            yield return null;
        }

        ammo = maxAmmo;
        GameObject.Find("Ammo").TryGetComponentWithWarning(out TextMeshProUGUI text);
        text.text = $"{ammo}/{maxAmmo}";

        // Destroy the reload slider instance after reload is complete
        if (reloadInstance != null)
        {
            Destroy(reloadInstance);
        }
    }

    protected virtual Vector3 GetTargetPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(InputManager.Instance.MouseInput);
        mousePosition.z = 0;
        return mousePosition;
    }

    protected virtual Vector2 CalculateDirection(Vector3 shooterPosition, Vector3 targetPosition)
    {
        return (targetPosition - shooterPosition).normalized;
    }

    protected virtual GameObject SpawnProjectile(GameObject shooter)
    {
        GameObject newProjectile = Instantiate(projectilePrefab, shooter.transform.position, Quaternion.identity);

        if (newProjectile.TryGetComponentWithWarning(out Projectile projectile))
            projectile.SetShooter(shooter);

        ammo--;
        GameObject.Find("Ammo").TryGetComponentWithWarning(out TextMeshProUGUI text);
        text.text = $"{ammo}/{maxAmmo}";

        return newProjectile;
    }

    protected virtual void ApplyVelocity(GameObject projectile, Vector2 direction)
    {
        if (projectile.TryGetComponentWithWarning(out Rigidbody2D rb))
        {
            rb.velocity = direction * projectileSpeed;
        }
    }

    protected bool NeedReload(GameObject shooter)
    {
        if (ammo <= 0)
        {
            if (shooter.TryGetComponentWithWarning(out MonoBehaviour shooterMonoBehaviour))
                shooterMonoBehaviour.StartCoroutine(Reload(shooter));
            return true;
        }

        return false;
    }
}