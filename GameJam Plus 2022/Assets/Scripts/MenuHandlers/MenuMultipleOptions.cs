using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuMultipleOptions : MonoBehaviour
{
    public int selectedButton;
    [SerializeField] private MenuOption[] menuOption;
    [SerializeField] private bool disabled;
    [SerializeField] private AudioClipManager sound;

    void Update()
    {
        if(!disabled)
        {
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedButton++;
                sound.PlayOneShot("MoveOption");
                if(selectedButton == menuOption.Length)
                {
                    menuOption[menuOption.Length - 1].mouseOver = false;
                    selectedButton = 0;
                }
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedButton--;
                sound.PlayOneShot("MoveOption");
                if(selectedButton < 0)
                {
                    menuOption[0].mouseOver = false;
                    selectedButton = menuOption.Length - 1;
                }
            }
            UpdateButtons();
        }
        else
        {
            for(int i = 0; i < menuOption.Length; i++)
            {
                if(menuOption[i].text.fontSize > menuOption[i].nativeSize)
                {
                    menuOption[i].currentSize = Mathf.Lerp(menuOption[i].currentSize, menuOption[i].nativeSize, menuOption[i].lerpSize * Time.deltaTime);
                }
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Return))
        {
            menuOption[selectedButton].gameObject.GetComponent<Button>().onClick.Invoke();
            sound.PlayOneShot("SelectOption");
        }
    }
    public void UpdateButtons()
    {
        if(selectedButton >= 0)
        {
            menuOption[selectedButton].mouseOver = true;
        }
        else
        {
            for(int i = 0; i < menuOption.Length; i++)
            {
                menuOption[i].mouseOver = false;
            }
        }
    }

    public void EnableThis()
    {
        disabled = false;
        selectedButton = 0;
        UpdateButtons();
    }
    public void DisableThis()
    {
        disabled = true;
        selectedButton = -1;
        UpdateButtons();
    }
}
