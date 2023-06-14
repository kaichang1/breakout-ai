using UnityEngine;

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

    internal bool muted;

    private void Start()
    {
        muted = false;
    }
}
