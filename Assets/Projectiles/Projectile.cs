using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Min(0)] int damage;
    [SerializeField, Min(0)] float knockbackForce;
    [SerializeField, Min(0)] float maximumLifetime;

    private GameObject shooter;

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    void Start()
    {
        Destroy(gameObject, maximumLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            ApplyDamage(other);
            AplyKockback(other);
            Destroy(gameObject);
        }
    }

    void ApplyDamage(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
    }

    void AplyKockback(Collider2D other)
    {
        if (other.gameObject.TryGetComponentWithWarning(out Rigidbody2D rb))
        {
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}