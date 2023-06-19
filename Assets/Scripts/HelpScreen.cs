using System.Collections;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{
    public int waitTime;  // Minimum amount of time to show the initial help screen

    public GameObject movementScreen;
    public GameObject shootScreen;

    private GameObject[] screens;

    void Start()
    {
        screens = new GameObject[] { movementScreen, shootScreen };

        Player humanPlayer = GameManager.Instance._players[0];
        if (humanPlayer != null )
        {
            StartCoroutine(MovementHelp(humanPlayer));
            StartCoroutine(ShootHelp(humanPlayer));
        }
    }

    /// <summary>
    /// Show the movement help screen.
    /// 
    /// The help screen is disabled after the player moves.
    /// </summary>
    /// /// <param name="player">The player.</param>
    /// <returns></returns>
    private IEnumerator MovementHelp(Player player)
    {
        movementScreen.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        // While the player hasn't moved
        while (player._paddle.transform.position == player._paddle._paddlePositionInitial)
        {
            yield return null;
        }

        movementScreen.SetActive(false);
    }

    /// <summary>
    /// Show the shoot help screen.
    /// 
    /// The help screen is disabled after the player releases the ball.
    /// </summary>
    /// /// <param name="player">The player.</param>
    /// <returns></returns>
    private IEnumerator ShootHelp(Player player)
    {
        // If the player hasn't released the ball
        if (!player._isGameStarted)
        {
            yield return StartCoroutine(WaitForScreens());

            shootScreen.SetActive(true);

            while (!player._isGameStarted)
            {
                yield return null;
            }

            shootScreen.SetActive(false);
        }
    }

    /// <summary>
    /// Wait for all help screens to become disabled.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns></returns>
    private IEnumerator WaitForScreens()
    {
        foreach (GameObject screen in screens)
        {
            while (screen.activeSelf)
            {
                yield return null;
            }
        }
    }
}
