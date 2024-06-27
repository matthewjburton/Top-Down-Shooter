using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D rb;

    void Awake()
    {
        gameObject.TryGetComponentWithWarning(out rb);
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 moveDirection = InputManager.Instance.MoveInput.normalized;

        // Calculate the velocity directly based on move direction and move speed
        Vector2 targetVelocity = moveDirection * moveSpeed;

        // Smoothly interpolate between current velocity and target velocity
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, 0.5f);
    }
}
