using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string[] sceneNames;

    private void ChangeToScene(int i)
    {
        SceneManager.LoadScene(sceneNames[i], LoadSceneMode.Single);
    }

    public void SwitchToMenu()
    {
        ChangeToScene(0);
    }
    public void SwitchToGame()
    {
        ChangeToScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
