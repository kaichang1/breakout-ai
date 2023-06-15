using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    #region Singleton
    public static GameSettings Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    internal bool isMuted;
    internal bool isIgnoreMouseClicks;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isIgnoreMouseClicks = false;  // Ensure mouse clicks are activated at the start of every scene load
    }

    void Start()
    {
        isMuted = false;  // Game starts off unmuted
    }
}
