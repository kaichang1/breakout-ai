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

    [SerializeField] private Ball _ballRedPrefab;
    [SerializeField] private float _padding;  // Padding between ball and paddle

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
        foreach (Player player in GameManager.Instance._players)
        {
            if (player != null && !player._isGameStarted && player._ballsCount == 1)
            {
                Vector3 paddlePosition = player._paddle.transform.position;
                Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + _padding, paddlePosition.z);
                player._startingBall.transform.position = ballPosition;

                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    player._isGameStarted = true;
                    ShootBall(player);
                }
            }
        }
    }

    /// <summary>
    /// Shoot the ball.
    /// 
    /// This method should only be called after balls have been reset and the game has not started.
    /// </summary>
    /// <param name="player"></param>
    public void ShootBall(Player player)
    {
        if (player._ballsCount == 1)
        {
            player._startingBall.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ballSpeed);
        }
    }

    public void CreateBall(Player player, Ball ballPrefab)
    {
        Vector3 paddlePosition = player._paddle.transform.position;
        Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + _padding, paddlePosition.z);

        Ball ball = Instantiate(ballPrefab, ballPosition, Quaternion.identity) as Ball;
        ball.Init(player, player._ballsContainer.transform);

        player._startingBall = ball;
        player._ballsCount = 1;
    }
    
    public void DestroyBalls(Player player)
    {
        for (int i = 0; i < player._ballsContainer.transform.childCount; i++)
        {
            Destroy(player._ballsContainer.transform.GetChild(i).gameObject);
        }
        player._ballsCount = 0;
    }

    public void ResetBalls(Player player)
    {
        DestroyBalls(player);
        CreateBall(player, _ballRedPrefab);
    }
}
