using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneChanges : MonoBehaviour
{
    [SerializeField] private string[] sceneNames;
    [SerializeField] private OptionsMenu optionsMenuScript;

    private void ChangeToScene(int i)
    {
        SceneManager.LoadScene(sceneNames[i], LoadSceneMode.Single);
    }

    public void SwitchToMenu()
    {
        if(!optionsMenuScript.optionsOpen)
        {
            ChangeToScene(0);
        }
        
    }
    public void SwitchToGame()
    {
        if(!optionsMenuScript.optionsOpen)
        {
            ChangeToScene(1);
        }
    }

    public void ExitGame()
    {
        if(!optionsMenuScript.optionsOpen)
        {
            Application.Quit();
        }
    }
}
