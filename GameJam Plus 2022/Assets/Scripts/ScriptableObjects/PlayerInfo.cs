using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "The Truth", order = 1)]
public class PlayerInfo : ScriptableObject
{
    public int checkpointReached;

    public void NewGame()
    {
        checkpointReached = 0;
    }
}
