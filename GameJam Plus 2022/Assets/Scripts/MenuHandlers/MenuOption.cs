using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI text;
    public float currentSize;
    public float nativeSize;
    [SerializeField] private float bigSize;
    public float lerpSize;
    [SerializeField] private MenuMultipleOptions main;
    [SerializeField] private int menuNumber;
    public bool mouseOver;

    void Awake()
    {
        nativeSize = currentSize = text.fontSize;
    }

    void Update()
    {
        if(mouseOver && main.selectedButton == menuNumber && currentSize < bigSize)
        {
            currentSize = Mathf.Lerp(currentSize, bigSize, lerpSize * Time.deltaTime);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, nativeSize, lerpSize * Time.deltaTime);
        }
        text.fontSize = currentSize;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        main.selectedButton = menuNumber;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }
}
