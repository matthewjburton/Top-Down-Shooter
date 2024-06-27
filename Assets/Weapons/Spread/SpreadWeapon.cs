using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Spread Weapon", menuName = "Weapons/Spread Weapon")]
public class SpreadWeapon : ProjectileWeapon
{
    [SerializeField, Range(0, 360)] float spreadAngle;
    [SerializeField, Min(0)] float fireRate;
    [SerializeField] bool isShooting;

    public override void Shoot(GameObject shooter)
    {
        if (isShooting)
            return;

        if (HandleReload(shooter))
            return;

        // Ensure shooter has a MonoBehaviour component to start coroutine
        if (shooter.TryGetComponentWithWarning(out MonoBehaviour shooterMonoBehaviour))
            shooterMonoBehaviour.StartCoroutine(ShootContinuous(shooter));
    }

    IEnumerator ShootContinuous(GameObject shooter)
    {
        isShooting = true;

        while (InputManager.Instance.AttackPressed && isShooting)
        {
            Vector3 targetPosition = GetTargetPosition();
            Vector2 direction = CalculateDirection(shooter.transform.position, targetPosition);
            Quaternion rotation = CalculateRotation();
            GameObject newProjectile = SpawnProjectile(shooter);
            ApplyVelocity(newProjectile, rotation * direction);

            ScreenShake.Instance.Shake(0.05f, 0.05f);
            PlayShootSound(shooter);
            timeOfLastShot = Time.time;

            if (HandleReload(shooter))
                yield return new WaitUntil(() => ammo.CurrentAmmo > 0);

            yield return new WaitForSeconds(fireRate);
        }

        isShooting = false;
    }

    Quaternion CalculateRotation()
    {
        // Start firing from the leftmost angle to the rightmost angle
        float startAngle = -spreadAngle / 2f;
        float endAngle = spreadAngle / 2f;

        // Calculate the current angle for this projectile
        float angle = Random.Range(startAngle, endAngle);

        // Calculate rotation based on the current angle
        return Quaternion.Euler(0f, 0f, angle);
    }

    void OnEnable()
    {
        PlayerCombat.OnWeaponSwitched += () => isShooting = false;
    }
}
