using System;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Singleton
    public static BallManager Instance { get; private set; }

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

    public float ballSpeed;
    public Sprite[] ballSprites;  // { red ball, blue ball }

    [SerializeField] private Ball _ballRedPrefab;
    [SerializeField] private float _padding;  // Padding between ball and paddle during ball-shoot phase

    void Start()
    {
        foreach (Player player in GameManager.Instance._players)
        {
            if (player != null)
            {
                CreateBall(player, _ballRedPrefab);
            }
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.isGamePaused)
        {
            foreach (Player player in GameManager.Instance._players)
            {
                // Ball-shoot phase
                if (player != null && !player._isGameStarted && player._startingBall != null)
                {
                    // Ball follows paddle
                    Vector3 paddlePosition = player._paddle.transform.position;
                    Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + _padding, paddlePosition.z);
                    player._startingBall.transform.position = ballPosition;

                    // Only Shoot the human player's ball.
                    // The AI player shoots the ball via Agent files.
                    bool isHuman = player == GameManager.Instance._players[0];
                    bool isShoot = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                    if (isHuman && isShoot)
                    {
                        ShootBall(player);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Create and initialize a ball.
    /// 
    /// If this newly created ball is the player's only ball, set it to the starting ball.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <param name="ballPrefab">Ball prefab.</param>
    public void CreateBall(Player player, Ball ballPrefab)
    {
        // Instantiate the ball
        Vector3 paddlePosition = player._paddle.transform.position;
        Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + _padding, paddlePosition.z);
        Quaternion ballRotation = Quaternion.identity;
        Ball ball = Instantiate(ballPrefab, ballPosition, ballRotation) as Ball;
        // Initialize the ball
        int i = Array.IndexOf(GameManager.Instance._players, player);
        Sprite sprite = ballSprites[i];
        ball.Init(player, player._ballsContainer.transform, sprite);

        player._ballsCount++;
        if (player._ballsCount == 1)
        {
            player._startingBall = ball;
        }
    }

    /// <summary>
    /// Shoot the ball and start the game.
    /// 
    /// This method should only be called after balls have been reset and the game has not started.
    /// </summary>
    /// <param name="player">The player.</param>
    public void ShootBall(Player player)
    {
        if (!player._isGameStarted && player._startingBall != null)
        {
            player._startingBall.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ballSpeed);
        }

        player._isGameStarted = true;
    }

    /// <summary>
    /// Destroy all balls and reset the ball count.
    /// </summary>
    /// <param name="player">The player.</param>
    public void DestroyBalls(Player player)
    {
        for (int i = 0; i < player._ballsContainer.transform.childCount; i++)
        {
            Destroy(player._ballsContainer.transform.GetChild(i).gameObject);
        }
        player._ballsCount = 0;
    }

    /// <summary>
    /// Destroy all existing balls and create a single starting ball.
    /// </summary>
    /// <param name="player">The player.</param>
    public void ResetBalls(Player player)
    {
        DestroyBalls(player);
        CreateBall(player, _ballRedPrefab);
    }
}
