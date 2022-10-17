using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouLose : MonoBehaviour
{
    //public bool playerCollision;
    private CollisionChecking colCheck;
    
    //[SerializeField] private AudioClipManager sfx;

    void Awake()
    {
        //playerCollision = false;
        colCheck = GameObject.FindWithTag("Player").GetComponent<CollisionChecking>();
        //youLoseTransition = GameObject.Find("MorteTransicao");
        //sfx = GameObject.FindWithTag("Sound").GetComponent<AudioClipManager>();
    }

    void Start()
    {
        //youLoseTransition.SetActive(false);        
    }

    void Update()
    {
        /*
        if(playerCollision == true)
        {
            youLoseTransition.SetActive(true);
        }
        */
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            colCheck.playerLose = true;
           // playerCollision = true;
            /*
            sfx.PlayOneShot("DeathSound");
            sfx.PlayOneShot("LoseSound");
            */
        }
    }
}
