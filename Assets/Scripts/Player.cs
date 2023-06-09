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

    internal bool _isGameStarted;  // False if the game is in the ball-shoot phase, else true

    internal int _currentLevel;
    internal int _score;
    internal int _lives;

    internal Paddle _paddle;
    internal Ball _startingBall;  // Starting ball to shoot out
    internal GameObject _ballsContainer;  // Container to hold balls in the editor
    internal int _ballsCount;  // Number of balls remaining
    internal GameObject _bricksContainer;  // Container to hold bricks in the editor
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
            if (paddleTransform == null)
            {
                paddleTransform = transform.Find("PaddleAITrainer");
            }
        }
        _paddle = paddleTransform.GetComponent<Paddle>();

        _ballsContainer = new GameObject("Balls Container");
        _ballsContainer.transform.SetParent(transform);

        _bricksContainer = new GameObject("Bricks Container");
        _bricksContainer.transform.SetParent(transform);
    }
}
