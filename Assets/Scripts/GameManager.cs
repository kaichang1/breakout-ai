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
    public float paddleHorizontalBounceMultiplier;  // Affects how much the ball bounces left or right during paddle collisions

    [SerializeField] private int _brickPoints;  // Number of points given for each brick hitpoint

    internal Player[] _players;  // { human, AI }

    private void OnEnable()
    {
        Brick.OnBrickDestruction += UpdateScore;
        Ball.OnBallDeath += OnBallDeath;
    }

    private void OnDisable()
    {
        // Necessary for re-loading game scenes
        Brick.OnBrickDestruction -= UpdateScore;
        Ball.OnBallDeath -= OnBallDeath;
    }

    void Start()
    {
        // Set up players array.
        // The first index is reserved for the human player.
        // The second index is reversed for the AI player.
        // Elements in the array may be null if only the human or AI is playing.
        _players = new Player[2];

        GameObject human = GameObject.Find("Human");
        if (human != null)
        {
            _players[0] = human.GetComponent<Player>();
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
                //player.paddle.ResetPosition();

                // Pause the game
                player._isGameStarted = false;
            }
        }
    }

    private void UpdateScore(Brick brick)
    {
        Player player = brick._player;

        player._score += brick.initialHp * GameManager.Instance._brickPoints;
        UIManager.Instance.UpdateScoreText(player);
    }

    /// <summary>
    /// Reset lives to initialLives and update the UI.
    /// </summary>
    public void ResetLives(Player player)
    {
        player._lives = initialLives;
        UIManager.Instance.UpdateLivesText(player);
    }

    /// <summary>
    /// Reset score to 0 and update the UI.
    /// </summary>
    public void ResetScore(Player player)
    {
        player._score = 0;
        UIManager.Instance.UpdateScoreText(player);
    }

    private void GameOver(Player player)
    {
        BallManager.Instance.DestroyBalls(player);
        player.gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
