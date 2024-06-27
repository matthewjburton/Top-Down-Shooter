using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    [Header("Projectile Settings")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField, Min(0)] protected float projectileSpeed;

    [Header("Ammo Settings")]
    [SerializeField] public Ammo ammo;

    [Header("Cooldown")]
    [SerializeField, Min(0)] protected float cooldownTime;
    protected float timeOfLastShot;

    [Header("Audio")]
    [SerializeField] protected AudioClip[] shootSounds;

    void OnEnable()
    {
        ammo.SetAmmo(ammo.MaxAmmo);
        timeOfLastShot = -cooldownTime;
    }

    public override void Attack(GameObject attacker)
    {
        Shoot(attacker);
    }

    public virtual void Shoot(GameObject shooter)
    {
        if (IsCooldownActive())
            return;

        if (HandleReload(shooter))
            return;

        Vector3 targetPosition = GetTargetPosition();
        Vector2 direction = CalculateDirection(shooter.transform.position, targetPosition);
        GameObject newProjectile = SpawnProjectile(shooter);
        ApplyVelocity(newProjectile, direction);

        ScreenShake.Instance.Shake(0.05f, 0.05f);
        PlayShootSound(shooter);
        timeOfLastShot = Time.time;

        if (HandleReload(shooter))
            return;
    }

    protected virtual bool HandleReload(GameObject shooter)
    {
        if (ammo.NeedReload())
        {
            if (shooter.TryGetComponentWithWarning(out MonoBehaviour shooterMonoBehaviour))
                shooterMonoBehaviour.StartCoroutine(ammo.Reload(shooter));
            return true;
        }
        return false;
    }

    protected virtual void PlayShootSound(GameObject shooter)
    {
        SoundManager.Instance.PlayRandomSound(shootSounds, shooter.transform);
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

        ammo.UseAmmo();

        return newProjectile;
    }

    protected virtual void ApplyVelocity(GameObject projectile, Vector2 direction)
    {
        if (projectile.TryGetComponentWithWarning(out Rigidbody2D rb))
        {
            rb.velocity = direction * projectileSpeed;
        }
    }

    protected bool IsCooldownActive()
    {
        return Time.time - timeOfLastShot < cooldownTime;
    }
}