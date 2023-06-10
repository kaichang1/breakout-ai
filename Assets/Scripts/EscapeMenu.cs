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
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Cursor.visible = true;

        Time.timeScale = 0f;
        escapeMenu.SetActive(true);
        GameManager.Instance.isGamePaused = true;
    }

    public void Resume()
    {
        Cursor.visible = false;

        Time.timeScale = 1f;
        escapeMenu.SetActive(false);
        GameManager.Instance.isGamePaused = false;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartGame();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneController.LoadMainMenu();
    }
}
