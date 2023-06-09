using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> OnBallDeath;

    internal Player _player;

    public void Init(Player owner, Transform containerTransform)
    {
        _player = owner;
        transform.SetParent(containerTransform);
    }

    public void Death() {
        OnBallDeath?.Invoke(this);
    }
}
