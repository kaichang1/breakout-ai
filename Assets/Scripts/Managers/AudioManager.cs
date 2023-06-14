using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance { get; private set; }

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

    public Sound[] sounds;

    // Sound names should match inspector values for the sounds array
    internal static string backgroundMusicMainMenu = "BackgroundMusicMainMenu";
    internal static string backgroundMusicBreakout = "BackgroundMusicBreakout";
    internal static string paddleBounce = "PaddleBounce";
    internal static string brickHit = "BrickHit";
    internal static string levelChange = "LevelChange";
    internal static string ballDeath = "BallDeath";
    internal static string victory = "Victory";
    internal static string gameOver = "GameOver";

    private void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == SceneController._mainMenu)
        {
            Play(backgroundMusicMainMenu);
        }
        else
        {
            Play(backgroundMusicBreakout);
        }
    }

    internal void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        sound.source.Play();
    }
}
