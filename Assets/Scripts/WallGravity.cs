using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGravity : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.GetComponent<Rigidbody2D>();





            Debug.Log("WALLGRAVITY COLLISION");
            Debug.Log(ballRb.velocity);






            Vector2 velocity = new Vector2(ballRb.velocity.x, Mathf.Min(0f, ballRb.velocity.y));
            ballRb.velocity = velocity;





            Debug.Log(ballRb.velocity);
            Debug.Log("END WALLGRAVITY COLLISION");
        }
    }
}
