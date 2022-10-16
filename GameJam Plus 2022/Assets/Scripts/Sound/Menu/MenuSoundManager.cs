using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClipManager soundScript;
    void Start()
    {
        soundScript.Play("MenuIntro");
    }
}
