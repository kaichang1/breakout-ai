using UnityEngine;

public class Paddle : MonoBehaviour
{
    internal Player _player;

    private Vector3 _paddlePositionInitial;

    void Start()
    {
        _player = transform.parent.GetComponent<Player>();

        _paddlePositionInitial = transform.position;
    }

    void Update()
    {
        if (!GameManager.Instance.isGamePaused)
        {
            // Only move the human player's paddle.
            // The AI player moves the paddle via Agent files.
            if (_player == GameManager.Instance._players[0])
            {
                if (Input.GetAxisRaw("Mouse X") != 0)  // If the mouse has moved along the X axis
                {
                    // Update paddle X position based on mouse X position
                    Vector2 position;
                    position.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                    position.y = _paddlePositionInitial.y;
                    transform.position = position;
                }
                else
                {
                    // Update paddle X position based on keyboard horizontal axis input (arrow keys) and speed
                    Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                    transform.Translate(GameManager.Instance.paddleSpeed * Time.deltaTime * direction);
                }
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BallCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        BallCollision(collision);
    }

    /// <summary>
    /// Handle ball collisions.
    /// 
    /// Collisions to the bottom half of the paddle (mainly on the sides)
    /// bounce the ball downwards in the opposite X direction.
    /// 
    /// Collisions to the top of the paddle bounce the ball upwards and to the
    /// left or right with varying degrees of horizontal magnitude depending on
    /// the contact point's location.
    /// </summary>
    /// <param name="collision">Collision object's collider.</param>
    private void BallCollision(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.GetComponent<Rigidbody2D>();
            Vector2 ballCenter = ballRb.position;
            Vector2 paddleCenter = transform.position;

            Vector2 velocity = ballRb.velocity;
            // Collisions below the paddle midpoint (on the sides)
            if (ballCenter.y < paddleCenter.y)
            {
                // Bounce the ball downwards in the opposite X direction
                float velocityAbsX = Mathf.Abs(velocity.x);
                velocity.x = ballCenter.x < paddleCenter.x ? -velocityAbsX : velocityAbsX;
                velocity.y = Mathf.Min(velocity.y, -velocity.y);
            }
            // Top collisions
            else
            {
                // Contact point with ball determines how far left/right the ball bounces
                float diff = ballCenter.x - paddleCenter.x;
                float horizontalSpeed = diff * GameManager.Instance.paddleHorizontalBounceMultiplier;
                // Bounce the ball upwards and to the left/right depending on the location of contact
                velocity.x = horizontalSpeed;
                velocity.y = Mathf.Max(velocity.y, -velocity.y);
            }
            // Ensure that the velocity vector's magnitude (speed) equals the ballSpeed
            velocity = velocity.normalized * BallManager.Instance.ballSpeed;
            ballRb.velocity = velocity;
        }
    }

    /// <summary>
    /// Reset the paddle position.
    /// </summary>
    public void ResetPosition()
    {
        transform.position = _paddlePositionInitial;
    }
}
