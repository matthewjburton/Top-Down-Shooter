using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    [Header("Projectile Settings")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField, Min(0)] protected float projectileSpeed;

    [Header("Ammo Settings")]
    [SerializeField] public Ammo ammo;

    [Header("Audio")]
    [SerializeField] protected AudioClip[] shootSounds;

    void OnEnable()
    {
        ammo.SetAmmo(ammo.MaxAmmo);
    }

    public override void Attack(GameObject attacker)
    {
        Shoot(attacker);
    }

    public virtual void Shoot(GameObject shooter)
    {
        if (ammo.NeedReload())
        {
            if (shooter.TryGetComponentWithWarning(out MonoBehaviour shooterMonoBehaviour))
                shooterMonoBehaviour.StartCoroutine(ammo.Reload(shooter));
            return;
        }

        Vector3 targetPosition = GetTargetPosition();
        Vector2 direction = CalculateDirection(shooter.transform.position, targetPosition);

        GameObject newProjectile = SpawnProjectile(shooter);
        ApplyVelocity(newProjectile, direction);
        ammo.UseAmmo();

        SoundManager.Instance.PlayRandomSound(shootSounds, shooter.transform);

        if (ammo.NeedReload())
        {
            if (shooter.TryGetComponentWithWarning(out MonoBehaviour shooterMonoBehaviour))
                shooterMonoBehaviour.StartCoroutine(ammo.Reload(shooter));
            return;
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
}