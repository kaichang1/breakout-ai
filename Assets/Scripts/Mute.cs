using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour
{
    public Image icon;

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    private void Start()
    {
        if (GameSettings.Instance.muted)
        {
            icon.sprite = musicOffSprite;
        }
        else
        {
            icon.sprite = musicOnSprite;
        }
    }

    public void Toggle()
    {
        if (GameSettings.Instance.muted)
        {
            UnmuteAudio();

            GameSettings.Instance.muted = false;
            icon.sprite = musicOnSprite;
        }
        else
        {
            MuteAudio();

            GameSettings.Instance.muted = true;
            icon.sprite = musicOffSprite;
        }
    }

    public void MuteAudio()
    {
        AudioListener.pause = true;
    }

    public void UnmuteAudio()
    {
        AudioListener.pause = false;
    }
}
