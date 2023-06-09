using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text finalScoreText;

    public GameObject victoryScreen;
    public GameObject gameOverScreen;

    internal bool _isGameStarted;

    internal int _currentLevel;
    internal int _score;
    internal int _lives;

    internal Paddle _paddle;
    internal Ball _startingBall;
    internal GameObject _ballsContainer;  // Container to hold instantiated balls
    internal int _ballsCount;  // Number of balls
    internal GameObject _bricksContainer;  // Container to hold instantiated bricks
    internal int _bricksCount;  // Number of bricks remaining in the current level

    void Start()
    {
        _isGameStarted = false;

        _currentLevel = LevelManager.Instance.initialLevel;
        _score = 0;
        _lives = GameManager.Instance.initialLives;

        Transform paddleTransform = transform.Find("PaddleHuman");
        if (paddleTransform == null)
        {
            paddleTransform = transform.Find("PaddleAI");
        }
        _paddle = paddleTransform.GetComponent<Paddle>();

        _ballsContainer = new GameObject("Balls Container");
        _ballsContainer.transform.SetParent(transform);

        _bricksContainer = new GameObject("Bricks Container");
        _bricksContainer.transform.SetParent(transform);
    }
}
