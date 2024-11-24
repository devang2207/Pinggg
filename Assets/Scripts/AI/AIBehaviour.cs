using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField, Tooltip("Ball reference")] private Transform ball;
    [SerializeField, Tooltip("Speed of AI")] private float speed = 5f;
    [SerializeField, Tooltip("Clamp AI")] private float maxPaddleY = 4f;
    [SerializeField, Tooltip("Inconsistency range")] private Vector2 inconsistencyRange = new Vector2(0.1f, 0.2f);
    [SerializeField, Tooltip("Change interval for randomness")] private float changeInterval = 0.5f;

    private Rigidbody2D rb2D;
    private float randomOffset; // Random offset to simulate inconsistency
    private float nextChangeTime; // Time to change the random offset

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        ChangeRandomOffset();
    }

    private void FixedUpdate()
    {
        if (ball == null) return;

        // Calculate the target position based on the ball's y position
        float targetY = Mathf.Clamp(ball.position.y + randomOffset, -maxPaddleY, maxPaddleY);

        // Get current position of the paddle
        Vector2 currentPosition = rb2D.position;

        // Smoothly move the paddle towards the target position
        Vector2 targetPosition = new Vector2(currentPosition.x, targetY);
        rb2D.MovePosition(Vector2.Lerp(currentPosition, targetPosition, speed * Time.fixedDeltaTime));

        // Change the random offset periodically
        if (Time.time >= nextChangeTime)
        {
            ChangeRandomOffset();
        }
    }

    private void ChangeRandomOffset()
    {
        // Change the random offset and set the next change time
        randomOffset = Random.Range(inconsistencyRange.x, inconsistencyRange.y) * (Random.value > 0.5f ? 1 : -1);
        nextChangeTime = Time.time + changeInterval;
    }
}
