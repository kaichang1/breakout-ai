using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> OnBallDeath;

    public Player player { get; set; }

    public void Init(Player owner)
    {
        player = owner;
        transform.SetParent(player.transform);
    }

    public void Death() {
        OnBallDeath?.Invoke(this);
    }
}
