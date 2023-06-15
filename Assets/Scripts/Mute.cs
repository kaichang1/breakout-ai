using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour
{
    public Image icon;

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    private void Start()
    {
        if (GameSettings.Instance.isMuted)
        {
            icon.sprite = musicOffSprite;
        }
        else
        {
            icon.sprite = musicOnSprite;
        }
    }

    /// <summary>
    /// Toggle audio muting and update the mute icon.
    /// </summary>
    public void Toggle()
    {
        StartCoroutine(GameManager.Instance.TemporarilyIgnoreMouseClicks());

        if (GameSettings.Instance.isMuted)
        {
            UnmuteAudio();

            GameSettings.Instance.isMuted = false;
            icon.sprite = musicOnSprite;
        }
        else
        {
            MuteAudio();

            GameSettings.Instance.isMuted = true;
            icon.sprite = musicOffSprite;
        }
    }

    /// <summary>
    /// Mute all game audio.
    /// </summary>
    public void MuteAudio()
    {
        AudioListener.volume = 0f;
    }

    /// <summary>
    /// Unmute all game audio.
    /// </summary>
    public void UnmuteAudio()
    {
        AudioListener.volume = 1f;
    }
}
