using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Stream Weapon", menuName = "Weapons/Stream Weapon")]
public class StreamWeapon : ProjectileWeapon
{
    [SerializeField, Range(0, 360)] float spreadAngle;
    [SerializeField, Min(0)] float fireRate;

    public override void Shoot(GameObject shooter)
    {
        // Ensure shooter has a MonoBehaviour component to start coroutine
        if (shooter.TryGetComponentWithWarning(out MonoBehaviour shooterMonoBehaviour))
            shooterMonoBehaviour.StartCoroutine(ShootContinuous(shooter));
    }

    IEnumerator ShootContinuous(GameObject shooter)
    {
        if (ammo.NeedReload())
        {
            if (shooter.TryGetComponentWithWarning(out MonoBehaviour shooterMonoBehaviour))
                shooterMonoBehaviour.StartCoroutine(ammo.Reload(shooter));
            yield break;
        }

        while (InputManager.Instance.AttackPressed) // Continuously fire while shoot button is pressed
        {
            Vector3 targetPosition = GetTargetPosition();
            Vector2 direction = CalculateDirection(shooter.transform.position, targetPosition);

            Quaternion rotation = CalculateRotation();
            GameObject newProjectile = SpawnProjectile(shooter);
            ApplyVelocity(newProjectile, rotation * direction);

            SoundManager.Instance.PlayRandomSound(shootSounds, shooter.transform);

            if (ammo.NeedReload())
            {
                if (shooter.TryGetComponentWithWarning(out MonoBehaviour shooterMonoBehaviour))
                    shooterMonoBehaviour.StartCoroutine(ammo.Reload(shooter));
                yield break;
            }

            yield return new WaitForSeconds(fireRate);
        }
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
}
