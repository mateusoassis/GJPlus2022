using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Reference")]
    public TextMeshProUGUI text;
    [SerializeField] private MenuMultipleOptions main;
    [Header("Ignore")]
    public float currentSize;
    public float nativeSize;
    [Header("Fill the gaps")]
    [SerializeField] private float bigSize;
    public float lerpSize;
    [SerializeField] private int menuNumber;
    [Header("True the first option")]
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
