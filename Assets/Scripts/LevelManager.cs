using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager Instance { get; private set; }

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

    public int initialLevel;
    public Sprite[] brickSprites;  // { 1 hp brick, 2 hp brick, 3 hp brick }

    [SerializeField] private Color[] _brickColors;
    [SerializeField] private Brick _brickPrefab;
    [SerializeField] private string _levelsHpFile;
    [SerializeField] private string _levelsColorsFile;

    private List<int[,]> _levelsHpData;  // Brick hp data for all levels
    private List<int[,]> _levelsColorsData;  // Brick color data for all levels

    private int _levelRows = 15;
    private int _levelCols = 12;
    private Vector3 _initialBrickPosition = new Vector3(-1.99f, 3.377f, 0f);  // Top-left brick local position
    private float _shift = 0.365f;  // Brick width + padding

    private string[] newlineChars = new string[] { "\r\n", "\r", "\n" };

    void Start()
    {
        _levelsHpData = LoadLevelsHpData();
        _levelsColorsData = LoadLevelsColorsData();

        foreach (Player player in GameManager.Instance._players)
        {
            if (player != null)
            {
                GenerateLevel(player, player._currentLevel);
            }
        }
    }

    private void OnEnable()
    {
        Brick.OnBrickDestruction += LevelCompletion;
    }

    private void OnDisable()
    {
        Brick.OnBrickDestruction -= LevelCompletion;
    }

    /// <summary>
    /// Handle level completion logic whenever a brick is destroyed.
    /// 
    /// If the final level has been completed, show the victory screen.
    /// If a non-final level has been completed, generate the next level.
    /// </summary>
    /// <param name="brick">Brick that was destroyed.</param>
    private void LevelCompletion(Brick brick)
    {
        Player player = brick._player;

        if (CheckLevelCompletion(player))
        {
            if (CheckFinalLevel(player))
            {
                BallManager.Instance.DestroyBalls(player);

                UIManager.Instance.UpdateFinalScoreText(player);
                player.victoryScreen.SetActive(true);
            }
            else
            {
                player._currentLevel++;

                GenerateLevel(player, player._currentLevel);
                UIManager.Instance.UpdateLevelText(player);
                BallManager.Instance.ResetBalls(player);
                //player.paddle.ResetPosition();

                player._isGameStarted = false;
            }
        }
    }

    /// <summary>
    /// Check for level completion.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns>True if the current level has been completed, else false.</returns>
    private bool CheckLevelCompletion(Player player)
    {
        return player._bricksCount == 0;
    }

    /// <summary>
    /// Check if the current level is the final level.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns>True if the current level is the final level, else false.</returns>
    private bool CheckFinalLevel(Player player)
    {
        return player._currentLevel == _levelsHpData.Count;
    }

    /// <summary>
    /// Destroy all bricks in the current level.
    /// </summary>
    /// <param name="player">The player.</param>
    private void ClearLevel(Player player)
    {
        // Destroy all current bricks
        for (int i = 0; i < player._bricksContainer.transform.childCount; i++)
        {
            Destroy(player._bricksContainer.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Reset the current level to level 1.
    /// </summary>
    /// <param name="player">The player.</param>
    public void ResetLevels(Player player)
    {
        ClearLevel(player);

        player._currentLevel = 1;
        GenerateLevel(player, player._currentLevel);

        UIManager.Instance.UpdateLevelText(player);
    }

    /// <summary>
    /// Generate bricks for the current level.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <param name="currentLevel">Current level.</param>
    private void GenerateLevel(Player player, int currentLevel)
    {
        int[,] levelHpData = _levelsHpData[currentLevel - 1];
        int[,] levelColorData = _levelsColorsData[currentLevel - 1];

        player._bricksCount = 0;
        
        // Local position is affected by parent position
        float currentX =  _initialBrickPosition.x + player.transform.position.x;
        float currentY = _initialBrickPosition.y;

        for (int i = 0; i < levelHpData.GetLength(0); i++)
        {
            for (int j = 0; j < levelHpData.GetLength(1); j++)
            {
                int brickHp = levelHpData[i, j];
                int brickColor = levelColorData[i, j];

                if (brickHp > 0)
                {
                    // Instantiate the brick
                    Vector3 brickPosition = new Vector3(currentX, currentY, _initialBrickPosition.z);
                    Quaternion brickRotation = Quaternion.identity;
                    Brick brick = Instantiate(_brickPrefab, brickPosition, brickRotation) as Brick;
                    // Initialize the brick
                    Sprite sprite = brickSprites[brickHp - 1];
                    Color color = _brickColors[brickColor - 1];
                    brick.Init(player, player._bricksContainer.transform, sprite, color, brickHp);

                    player._bricksCount++;
                }

                currentX += _shift;
            }

            currentX = _initialBrickPosition.x + player.transform.position.x;
            currentY -= _shift;
        }
    }

    /// <summary>
    /// Load levels data (hitpoints) from text file.
    /// 
    /// Note that the hitpoints data must match the colors data, i.e. all
    /// bricks present in one file must be present in the other file.
    /// 
    /// Integers in the text file represent bricks and their associated hitpoints.
    ///   0: No brick
    ///   1: Brick with 1 hp
    ///   2: Brick with 2 hp
    ///   3: Brick with 3 hp
    ///   
    /// The returned list of levels data mirrors this representation. Each level
    /// is represented as an integer matrix with hp values as described above.
    /// </summary>
    /// <returns>List of levels data (hitpoints).</returns>
    private List<int[,]> LoadLevelsHpData()
    {
        return LoadLevelsData(_levelsHpFile);
    }

    /// <summary>
    /// Load levels data (colors) from text file.
    /// 
    /// Note that the colors data must match the hitpoints data, i.e. all
    /// bricks present in one file must be present in the other file.
    /// 
    /// Integers in the text file represent bricks and their associated color.
    ///   0: No brick
    ///   1: Brick with color brickColors[0]
    ///   ...
    ///   5: Brick with color brickColors[4]
    ///   
    /// The returned list of levels data mirrors this representation. Each level
    /// is represented as an integer matrix with color values as described above.
    /// </summary>
    /// <returns>List of levels data (colors).</returns>
    private List<int[,]> LoadLevelsColorsData()
    {
        return LoadLevelsData(_levelsColorsFile);
    }

    /// <summary>
    /// Load levels data from text file.
    /// 
    /// Within the text file:
    /// The end of a level is signified by "---".
    /// Each level must consist of _levelRows rows and _levelCols columns, which identify brick positions.
    /// Brick positions are separated by commas.
    /// The integer value at these brick positions represent either hp or color data for each brick,
    ///     depending on the file being loaded.
    /// </summary>
    /// <param name="fileName">The file to be loaded.</param>
    /// <returns>List of levels data</returns>
    private List<int[,]> LoadLevelsData(string fileName)
    {
        List<int[,]> levelsData = new List<int[,]>();

        TextAsset levelsAsset = Resources.Load(fileName) as TextAsset;
        string[] lines = levelsAsset.text.TrimEnd().Split(newlineChars, StringSplitOptions.RemoveEmptyEntries);

        int[,] curLevel = new int[_levelRows, _levelCols];
        int matrixRow = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.Equals(line, "---"))  // If the current level has been fully parsed
            {
                levelsData.Add(curLevel);

                curLevel = new int[_levelRows, _levelCols];
                matrixRow = 0;
            }
            else
            {
                string[] lineBricks = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < lineBricks.Length; j++)
                {
                    curLevel[matrixRow, j] = int.Parse(lineBricks[j]);
                }

                matrixRow++;
            }
        }

        return levelsData;
    }
}
