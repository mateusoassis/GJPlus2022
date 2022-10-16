using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    public bool gamePaused;
    [SerializeField] private MenuMultipleOptions pauseMenuScript;

    void Awake()
    {
        //pauseMenuScript.DisableThis();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!gamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Pause();
        gamePaused = true;
        pauseMenuScript.EnableThis();
    }
    public void ResumeGame()
    {
        Resume();
        gamePaused = false;
        pauseMenuScript.DisableThis();
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
    }
    private void Resume()
    {
        pauseMenu.SetActive(false);
    }
}
