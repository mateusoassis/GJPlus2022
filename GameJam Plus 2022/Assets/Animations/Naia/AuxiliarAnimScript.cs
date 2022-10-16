using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxiliarAnimScript : MonoBehaviour
{
    [SerializeField] private AudioClipManager audio;
    
    public void StepSound()
    {
        int u = Random.Range(1, 4);
        audio.PlayOneShot("Step"+u);
    }
}
