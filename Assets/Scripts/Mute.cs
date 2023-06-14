//using UnityEngine;
//using UnityEngine.UI;

//public class Mute : MonoBehaviour
//{
//    public Sprite musicOnSprite;
//    public Sprite musicOnSelectedSprite;

//    public Sprite musicOffSprite;
//    public Sprite musicOffSelectedSprite;

//    private Image image;
//    private Button button;

//    private void Awake()
//    {
//        image = GetComponent<Image>();
//        button = GetComponent<Button>();
//    }

//    public void Toggle()
//    {
//        if (GameSettings.Instance.muted)
//        {
//            UnmuteAudio();

//            GameSettings.Instance.muted = false;
//            image.sprite = musicOnSprite;
//            // Change Highlited Sprite
//            SpriteState ss = button.spriteState;
//            ss.highlightedSprite = musicOnSelectedSprite;
//            button.spriteState = ss;
//        }
//        else
//        {
//            MuteAudio();

//            GameSettings.Instance.muted = true;
//            image.sprite = musicOffSprite;
//            // Change Highlited Sprite
//            SpriteState ss = button.spriteState;
//            ss.highlightedSprite = musicOffSelectedSprite;
//            button.spriteState = ss;
//        }
//    }

//    public void MuteAudio()
//    {
//        AudioListener.pause = true;
//    }

//    public void UnmuteAudio()
//    {
//        AudioListener.pause = false;
//    }
//}

using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour
{
    public Image icon;

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

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
