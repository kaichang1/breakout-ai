using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private string _singlePlayerMode;
    [SerializeField] private string _versusAIMode;

    public void SinglePlayerButton() {
        SceneManager.LoadScene(_singlePlayerMode);
    }
    public void VersusModeButton() {
        SceneManager.LoadScene(_versusAIMode);
    }
}
