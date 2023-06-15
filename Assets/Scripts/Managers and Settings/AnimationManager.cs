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

    public GameObject transitionStartHuman;
    public GameObject transitionEndHuman;

    public GameObject transitionStartAI;
    public GameObject transitionEndAI;

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

        if (player == GameManager.Instance._players[0])
        {
            transitionStartHuman.SetActive(true);
            yield return new WaitForSeconds(transitionTime);
            transitionStartHuman.SetActive(false);
        }
        else
        {
            transitionStartAI.SetActive(true);
            yield return new WaitForSeconds(transitionTime);
            transitionStartAI.SetActive(false);
        }
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
        if (player == GameManager.Instance._players[0])
        {
            transitionEndHuman.SetActive(true);
            yield return new WaitForSeconds(transitionTime);
            transitionEndHuman.SetActive(false);
        }
        else
        {
            transitionEndAI.SetActive(true);
            yield return new WaitForSeconds(transitionTime);
            transitionEndAI.SetActive(false);
        }

        player._isPlayerPaused = false;
    }
}
