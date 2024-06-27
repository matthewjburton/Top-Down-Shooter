using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField, Min(0)] int maxHealth;
    int health;
    bool flashing;

    [Header("Audio")]
    [SerializeField] AudioClip[] hitSounds;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        ScreenShake.Instance.Shake(0.1f, 0.1f);

        SoundManager.Instance.PlayRandomSound(hitSounds, transform);

        if (!flashing)
            StartCoroutine(Flash());

        health -= damage;

        if (health <= 0)
            Destroy(gameObject);
    }

    IEnumerator Flash()
    {
        flashing = true;

        Color originalColor;

        if (gameObject.TryGetComponentWithWarning(out SpriteRenderer renderer))
            originalColor = renderer.color;
        else
            yield break;

        renderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        renderer.color = originalColor;

        flashing = false;
    }
}