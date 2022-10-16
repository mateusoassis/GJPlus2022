using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointRegister : MonoBehaviour
{
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private int checkpointNumber;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("choco player");
            if(playerInfo.checkpointReached < checkpointNumber)
            {
                playerInfo.checkpointReached = checkpointNumber;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position, 0.5f); 
    }
}
