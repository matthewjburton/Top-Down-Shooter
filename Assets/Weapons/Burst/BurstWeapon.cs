using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Burst  Weapon", menuName = "Weapons/Burst  Weapon")]
public class BurstWeapon : ProjectileWeapon
{
    [SerializeField, Min(0)] int numberOfProjectiles;
    [SerializeField, Min(0)] float fireRate;

    public override void Shoot(GameObject shooter)
    {
        if (IsCooldownActive())
            return;

        if (HandleReload(shooter))
            return;

        Vector3 targetPosition = GetTargetPosition();
        Vector2 direction = CalculateDirection(shooter.transform.position, targetPosition);

        // Ensure shooter has a MonoBehaviour component to start coroutine
        if (shooter.TryGetComponentWithWarning(out MonoBehaviour shooterMonoBehaviour))
            shooterMonoBehaviour.StartCoroutine(ShootBurst(shooter, direction));
    }

    IEnumerator ShootBurst(GameObject shooter, Vector2 direction)
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject newProjectile = SpawnProjectile(shooter);
            ApplyVelocity(newProjectile, direction);

            ScreenShake.Instance.Shake(0.05f, 0.05f);
            PlayShootSound(shooter);
            timeOfLastShot = Time.time;

            if (HandleReload(shooter))
                yield break;

            yield return new WaitForSeconds(fireRate);
        }
    }
}