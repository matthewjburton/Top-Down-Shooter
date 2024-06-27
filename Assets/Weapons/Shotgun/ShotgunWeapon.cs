using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun Weapon", menuName = "Weapons/Shotgun Weapon")]
public class ShotgunWeapon : ProjectileWeapon
{
    [SerializeField, Min(0)] int numberOfProjectiles;
    [SerializeField, Range(0, 360)] float spreadAngle;

    public override void Shoot(GameObject shooter)
    {
        if (IsCooldownActive())
            return;

        if (HandleReload(shooter))
            return;

        Vector3 targetPosition = GetTargetPosition();
        Vector2 direction = CalculateDirection(shooter.transform.position, targetPosition);

        ScreenShake.Instance.Shake(0.05f, 0.05f);
        PlayShootSound(shooter);
        timeOfLastShot = Time.time;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Quaternion rotation = CalculateRotation(i);
            GameObject newProjectile = SpawnProjectile(shooter);
            ApplyVelocity(newProjectile, rotation * direction);

            if (HandleReload(shooter))
                return;
        }
    }

    Quaternion CalculateRotation(int projectileID)
    {
        // Start firing from the leftmost angle to the rightmost angle
        float startAngle = -spreadAngle / 2f;
        float endAngle = spreadAngle / 2f;

        // Calculate the current angle for this projectile
        float angle = Mathf.Lerp(startAngle, endAngle, (float)projectileID / (numberOfProjectiles - 1));

        // Calculate rotation based on the current angle
        return Quaternion.Euler(0f, 0f, angle);
    }
}