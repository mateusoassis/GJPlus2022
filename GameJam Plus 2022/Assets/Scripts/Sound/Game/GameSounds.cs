using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour
{
    [SerializeField] private AudioClipManager audio;
    // Start is called before the first frame update
    void Start()
    {
        audio.Play("StageSound");
    }
}
