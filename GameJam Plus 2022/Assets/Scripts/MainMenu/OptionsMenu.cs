using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    public bool optionsOpen;

    public void OpenOptions()
    {
        if(!optionsOpen)
        {
            optionsMenu.SetActive(true);
            optionsOpen = true;
        }
    }
    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
        optionsOpen = false;
    }
}
