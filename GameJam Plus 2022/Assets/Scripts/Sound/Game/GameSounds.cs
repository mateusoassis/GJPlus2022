using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSounds : MonoBehaviour
{
    [SerializeField] private AudioClipManager audio;
    [SerializeField] private Scene currentScene;
    [SerializeField] private string[] scenesToDeleteSource;
    [SerializeField] private PlayerInfo playerInfo;

    void Start()
    {
        
        audio.Play("StageSound");
        playerInfo.loadSound = false;
        
    }
}
