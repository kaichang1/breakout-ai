using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> OnBallDeath;  // Subscribed by GameManager

    internal Player _player;

    /// <summary>
    /// Initialize the ball.
    /// </summary>
    /// <param name="owner">The player associated with the ball.</param>
    /// <param name="containerTransform">The player's balls container.</param>
    public void Init(Player owner, Transform containerTransform)
    {
        _player = owner;
        transform.SetParent(containerTransform);
    }

    /// <summary>
    /// Ball death.
    /// 
    /// Called when the ball collides with the Death Wall, and invokes the
    /// OnBallDeath event.
    /// </summary>
    public void Death() {
        OnBallDeath?.Invoke(this);
    }
}
