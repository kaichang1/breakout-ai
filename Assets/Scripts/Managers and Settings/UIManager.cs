using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public void UpdateLevelText(Player player)
    {
        player.levelText.text = $"Level{Environment.NewLine}{player._currentLevel}";
    }

    public void UpdateScoreText(Player player)
    {
        player.scoreText.text = $"Score{Environment.NewLine}{player._score}";
    }

    public void UpdateLivesText(Player player)
    {
        player.livesText.text = $"Lives{Environment.NewLine}{player._lives}";
    }

    public void UpdateFinalScoreText(Player player)
    {
        player.finalScoreText.text = $"Final Score{Environment.NewLine}{player._score}";
    }
}
