using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private string singlePlayerMode;
    [SerializeField] private string versusAIMode;

    public void singlePlayerButton() {
        SceneManager.LoadScene(singlePlayerMode);
    }
    public void versusModeButton() {
        SceneManager.LoadScene(versusAIMode);
    }
}
