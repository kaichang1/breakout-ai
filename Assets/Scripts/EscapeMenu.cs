using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    public GameObject escapeMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.isGamePaused)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    /// <summary>
    /// Open the escape menu and pause the game.
    /// </summary>
    public void OpenMenu()
    {
        Cursor.visible = true;

        Time.timeScale = 0f;
        escapeMenu.SetActive(true);
        GameManager.Instance.isGamePaused = true;
    }

    /// <summary>
    /// Close the escape menu and pause the game.
    /// </summary>
    public void CloseMenu()
    {
        Cursor.visible = false;

        Time.timeScale = 1f;
        escapeMenu.SetActive(false);
        GameManager.Instance.isGamePaused = false;
    }

    /// <summary>
    /// Restart the game.
    /// </summary>
    public void Retry()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartGame();
    }

    /// <summary>
    /// Load the main menu scene.
    /// </summary>
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneController.LoadMainMenu();
    }
}
