using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> OnBallDeath;  // Subscribed by GameManager

    internal Player _player;

    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Initialize the ball.
    /// </summary>
    /// <param name="owner">The player associated with the ball.</param>
    /// <param name="containerTransform">The player's balls container.</param>
    public void Init(Player owner, Transform containerTransform, Sprite sprite)
    {
        _player = owner;
        transform.SetParent(containerTransform);
        _sr.sprite = sprite;
    }

    /// <summary>
    /// Ball death.
    /// 
    /// Called when the ball collides with the Death Wall, and invokes the
    /// OnBallDeath event.
    /// </summary>
    public void Death() {
        DeathSFX();

        OnBallDeath?.Invoke(this);
    }

    /// <summary>
    /// Play a sound effect for human player ball deaths.
    /// </summary>
    private void DeathSFX()
    {
        if (_player == GameManager.Instance._players[0])
        {
            // Do not play SFX for the last ball of the last life since game over SFX will be played
            if (_player._ballsCount == 1 && _player._lives == 1)
            {
                return;
            }

            AudioManager.Instance.Play(AudioManager.ballDeath);
        }
    }
}
