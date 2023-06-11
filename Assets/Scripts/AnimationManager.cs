using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    #region Singleton
    public static AnimationManager Instance { get; private set; }

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

    public GameObject transitionStart;
    public GameObject transitionEnd;

    public float transitionTime;

    /// <summary>
    /// Play the start transition and pause the player's game.
    /// 
    /// PlayTransitionEnd should be called after this method to finish the
    /// transition and unpause the game.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns></returns>
    internal IEnumerator PlayTransitionStart(Player player)
    {
        player._isPlayerPaused = true;

        transitionStart.SetActive(true);
        yield return new WaitForSeconds(transitionTime);
        transitionStart.SetActive(false);
    }

    /// <summary>
    /// Play the end transition and unpause the player's game.
    /// 
    /// PlayTransitionStart should be called before this method to start the
    /// transition and pause the game.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns></returns>
    internal IEnumerator PlayTransitionEnd(Player player)
    {
        transitionEnd.SetActive(true);
        yield return new WaitForSeconds(transitionTime);
        transitionEnd.SetActive(false);

        player._isPlayerPaused = false;
    }
}
