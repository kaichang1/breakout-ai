using UnityEngine;

public class WallDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Ball")) {
            Ball ball = collision.GetComponent<Ball>();
            ball.Death();
        }
    }
}
