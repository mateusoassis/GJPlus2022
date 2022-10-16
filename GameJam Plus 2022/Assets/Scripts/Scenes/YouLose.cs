using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouLose : MonoBehaviour
{
    public bool playerCollision;

    void Awake()
    {
        playerCollision = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerCollision = true;
        }
    }
}
