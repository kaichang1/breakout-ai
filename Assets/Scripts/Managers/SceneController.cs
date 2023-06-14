using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    internal static string _mainMenu = "MainMenu";
    internal static string _singlePlayer = "Breakout";
    internal static string _versusAI = "BreakoutVersus";

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
