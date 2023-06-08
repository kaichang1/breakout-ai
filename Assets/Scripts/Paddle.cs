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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 contactPoint = collision.GetContact(0).point;
            Vector2 paddleCenter = transform.position;

            // Contact point with ball determines how far left/right the ball bounces
            float diff = contactPoint.x - paddleCenter.x;
            float horizontalVelocity = diff * GameManager.Instance.paddleHorizontalBounceMultiplier;
            // Ensure that the velocity vector's magnitude equals the ballSpeed
            Vector2 velocityVector = new Vector2(horizontalVelocity, BallManager.Instance.ballSpeed);
            velocityVector = velocityVector.normalized * BallManager.Instance.ballSpeed;

            ballRb.velocity = velocityVector;
        }
    }

    public void ResetPosition()
    {
        transform.position = _paddlePositionInitial;
    }
}
