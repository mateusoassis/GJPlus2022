using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeoutManager : MonoBehaviour
{
    /*
    [SerializeField] private GameObject loseGameFade;
    [SerializeField] private float loseGameFadeDuration;
    [SerializeField] private GameObject newGameFade;
    [SerializeField] private float newGameFadeDuration;
    [SerializeField] private float startTimer;

    [SerializeField] private YouLose youLoseScript;
    [SerializeField] private PauseManager pauseManager;
    */
    [SerializeField] private GameObject newGameTransition;

    void Start()
    {
        newGameTransition.SetActive(true);
    }

    /*
    void Start()
    {
        startTimer = newGameFadeDuration;
        newGameFade.SetActive(true);
        Time.timeScale = 1;
    }

    void Update()
    {
        if(startTimer > 0)
        {
            startTimer -= Time.deltaTime;
        }
        else
        {
            newGameFade.SetActive(false);
        }
        
        if(youLoseScript.playerCollision)
        {
            loseGameFade.SetActive(true);
            Time.timeScale = 0;
            pauseManager.gamePaused = true;
        }
    }
    */
}
