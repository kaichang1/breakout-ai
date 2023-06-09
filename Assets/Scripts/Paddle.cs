using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Player player { get; set; }

    private Vector3 _paddlePositionInitial;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<Player>();

        _paddlePositionInitial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Only move the human player paddle.
        // The AI player moves the paddle via Agent files.
        if (player == GameManager.Instance.players[0])
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
                // Update paddle X position based on keyboard horizontal axis input and speed
                Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                transform.Translate(GameManager.Instance.paddleSpeed * Time.deltaTime * direction);
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

    private void BallCollision(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.GetComponent<Rigidbody2D>();
            Vector2 ballCenter = ballRb.position;
            Vector2 paddleCenter = transform.position;

            Vector2 velocity = ballRb.velocity;
            if (ballCenter.y < paddleCenter.y)
            {
                // Side collisions below the paddle midpoint should bounce the ball downwards
                float velocityAbsX = Mathf.Abs(velocity.x);
                velocity.x = ballCenter.x < paddleCenter.x ? -velocityAbsX : velocityAbsX;
                velocity.y = Mathf.Min(velocity.y, -velocity.y);
            }
            else
            {
                // Contact point with ball determines how far left/right the ball bounces for top collisions
                float diff = ballCenter.x - paddleCenter.x;
                float horizontalSpeed = diff * GameManager.Instance.paddleHorizontalBounceMultiplier;
                // Top collisions should bounce the ball upwards
                velocity.x = horizontalSpeed;
                velocity.y = Mathf.Max(velocity.y, -velocity.y);
            }
            // Ensure that the velocity vector's magnitude (speed) equals the ballSpeed
            velocity = velocity.normalized * BallManager.Instance.ballSpeed;
            ballRb.velocity = velocity;
        }
    }

    public void ResetPosition()
    {
        transform.position = _paddlePositionInitial;
    }
}
