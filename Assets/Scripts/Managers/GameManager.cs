using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

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

    public int initialLives;
    public float paddleSpeed;
    public float paddleHorizontalBounceMultiplier;  // How much the ball bounces left or right during paddle collisions

    [SerializeField] private int _brickPoints;  // Number of score points given for each brick hitpoint

    internal Player[] _players;  // { human, AI }
    internal bool isGamePaused;  // Escape menu pauses the game

    private void OnEnable()
    {
        Brick.OnBrickDestruction += UpdateScore;
        Ball.OnBallDeath += OnBallDeath;
    }

    private void OnDisable()
    {
        Brick.OnBrickDestruction -= UpdateScore;
        Ball.OnBallDeath -= OnBallDeath;
    }

    void Start()
    {
        isGamePaused = false;

        // Set up players array.
        // The first index is reserved for the human player.
        // The second index is reversed for the AI player.
        // Elements in the array may be null if only the human or AI is playing.
        _players = new Player[2];

        GameObject human = GameObject.Find("Human");
        if (human != null)
        {
            _players[0] = human.GetComponent<Player>();

            Cursor.visible = false;
            StartCoroutine(ResetMousePosition());
        }

        GameObject AI = GameObject.Find("AI");
        if (AI != null)
        {
            _players[1] = AI.GetComponent<Player>();
        }

        // Update UI
        foreach (Player player in _players)
        {
            if ( player != null )
            {
                UIManager.Instance.UpdateLevelText(player);
                UIManager.Instance.UpdateScoreText(player);
                UIManager.Instance.UpdateLivesText(player);
            }
        }
    }

    /// <summary>
    /// Handle ball death logic whenever a ball is lost.
    /// </summary>
    /// <param name="ball">Ball that was lost.</param>
    public void OnBallDeath(Ball ball)
    {
        Player player = ball._player;

        player._ballsCount--;
        if (player._ballsCount <= 0)
        {
            player._lives--;
            UIManager.Instance.UpdateLivesText(player);

            if (player._lives <= 0)
            {
                GameOver(player);
            }
            else
            {
                BallManager.Instance.ResetBalls(player);

                // Enter ball-shoot phase
                player._isGameStarted = false;
            }
        }
    }

    /// <summary>
    /// Update the score whenever a brick is destroyed.
    /// </summary>
    /// <param name="brick">Brick that was destroyed.</param>
    private void UpdateScore(Brick brick)
    {
        Player player = brick._player;

        player._score += brick.initialHp * _brickPoints;
        UIManager.Instance.UpdateScoreText(player);
    }

    /// <summary>
    /// Reset lives to the initial lives count.
    /// </summary>
    /// /// <param name="player">The player.</param>
    public void ResetLives(Player player)
    {
        player._lives = initialLives;
        UIManager.Instance.UpdateLivesText(player);
    }

    /// <summary>
    /// Reset score to 0.
    /// </summary>
    /// /// <param name="player">The player.</param>
    public void ResetScore(Player player)
    {
        player._score = 0;
        UIManager.Instance.UpdateScoreText(player);
    }

    /// <summary>
    /// Set the victory screen.
    /// </summary>
    /// <param name="player">The player.</param>
    internal void Victory(Player player)
    {
        if (player == GameManager.Instance._players[0])
        {
            Cursor.visible = true;
        }

        BallManager.Instance.DestroyBalls(player);
        UIManager.Instance.UpdateFinalScoreText(player);
        player.victoryScreen.SetActive(true);
    }

    /// <summary>
    /// Set the game over screen.
    /// </summary>
    /// <param name="player">The player.</param>
    private void GameOver(Player player)
    {
        if (player == GameManager.Instance._players[0])
        {
            Cursor.visible = true;
        }

        BallManager.Instance.DestroyBalls(player);
        player.gameOverScreen.SetActive(true);
    }

    /// <summary>
    /// Restart the game.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Reset the mouse position to the center of the screen.
    /// 
    /// This method should only be called for the human player.
    /// </summary>
    internal IEnumerator ResetMousePosition()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yield return null;  // Must wait one frame for the mouse position to reset
        Cursor.lockState = CursorLockMode.None;
    }
}
