using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private Transform[] checkpointsTransforms;
    [SerializeField] private Transform player;

    void Awake()
    {
        StartGame();
    }

    public void StartGame()
    {
        player.transform.position = checkpointsTransforms[playerInfo.checkpointReached].position;
    }
}
