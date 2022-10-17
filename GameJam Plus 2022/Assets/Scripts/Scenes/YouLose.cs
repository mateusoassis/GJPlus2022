using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouLose : MonoBehaviour
{
    public bool playerCollision;
    [SerializeField] private GameObject youLoseTransition;
    [SerializeField] private AudioClipManager sfx;

    void Awake()
    {
        playerCollision = false;
    }

    void Start()
    {
        youLoseTransition = GameObject.Find("MorteTransicao");
        sfx = GameObject.FindWithTag("Sound").GetComponent<AudioClipManager>();
    }

    void Update()
    {
        if(playerCollision == true)
        {
            youLoseTransition.SetActive(true);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerCollision = true;
            sfx.PlayOneShot("DeathSound");
            sfx.PlayOneShot("LoseSound");
        }
    }
}
