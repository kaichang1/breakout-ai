using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static string _mainMenu = "MainMenu";
    private static string _singlePlayer = "Breakout";
    private static string _versusAI = "BreakoutVersus";

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene(_mainMenu);
    }

    public static void LoadSinglePlayer() {
        SceneManager.LoadScene(_singlePlayer);
    }
    public static void LoadVersusAI() {
        SceneManager.LoadScene(_versusAI);
    }
}
