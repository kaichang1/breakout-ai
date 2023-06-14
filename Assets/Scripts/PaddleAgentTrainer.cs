using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PaddleAgentTrainer : Agent
{
    private Player _player;
    private float _minX;
    private float _maxX;

    void Start()
    {
        _player = GameManager.Instance._players[1];  // AI player

        Brick.OnBrickDestruction += OnBrickDestructionReward;
        Ball.OnBallDeath += OnBallDeathReward;

        ClampToBoundaries clampToBoundaries = GetComponent<ClampToBoundaries>();
        _minX = clampToBoundaries._minX - transform.parent.position.x;  // Local position
        _maxX = clampToBoundaries._maxX - transform.parent.position.x;
    }

    public override void OnEpisodeBegin()
    {
        LevelManager.Instance.ResetLevels(_player);
        GameManager.Instance.ResetScore(_player);
        GameManager.Instance.ResetLives(_player);

        BallManager.Instance.ResetBalls(_player);
        _player._paddle.ResetPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observations are based on local positions. This is useful since
        // Training positions are likely centered on (0, 0, 0).

        // Paddle position
        sensor.AddObservation(transform.localPosition);

        // Wall positions
        sensor.AddObservation(_minX);
        sensor.AddObservation(_maxX);

        // Ball positions: Ray Perception Sensor

        // Brick positions:  Ray Perception Sensor
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Update paddle X position
        float moveX = actions.ContinuousActions[0];
        transform.Translate(GameManager.Instance.paddleSpeed * Time.deltaTime * new Vector3(moveX, 0, 0));

        // Shoot the ball
        int shoot = actions.DiscreteActions[0];
        if (!_player._isGameStarted && shoot == 1)
        {
            BallManager.Instance.ShootBall(_player);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Human horizontal input (arrow keys)
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        // Human space key input
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;
    }

    private void OnBrickDestructionReward(Brick brick)
    {
        SetReward(0.1f);

        bool isLevelCompleted = LevelManager.Instance.CheckLevelCompletion(_player);
        bool isFinalLevel = LevelManager.Instance.CheckFinalLevel(_player);
        if (isLevelCompleted && isFinalLevel)
        {
            // Reset the game and continue training
            _player.victoryScreen.SetActive(false);
            _player._isGameStarted = false;
            EndEpisode();
        }
    }

    private void OnBallDeathReward(Ball obj)
    {
        SetReward(-1f);
        EndEpisode();
    }
}
