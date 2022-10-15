using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaHandler : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float imageFillAmount;

    [SerializeField] private float maxStamina;
    [SerializeField][Range(0,100)] private float currentStamina;
    [SerializeField] private float staminaRegenPerSecond;
    [SerializeField] private float dashCost;
    [SerializeField] private float degenPerSecond;

    [SerializeField] private bool degen;

    [SerializeField]
    
    void Start()
    {
        image.fillAmount = imageFillAmount;
    }

    void Update()
    {
        if(degen == true)
        {
            if(currentStamina > 0)
            {
                currentStamina -= Time.deltaTime * degenPerSecond;
            }
            else
            {
                currentStamina = 0;
            }
        }
        else
        {
            if(currentStamina < 100)
            {
                currentStamina += Time.deltaTime * staminaRegenPerSecond;
            }
            else
            {
                currentStamina = 100;
            }
        }
        UpdateImageFillAmount();
    }

    public void StartDegen()
    {
        degen = true;
    }

    public void UpdateImageFillAmount()
    {
        imageFillAmount = currentStamina / maxStamina;
        image.fillAmount = imageFillAmount;
    }

    public void CastDash()
    {
        if(dashCost < currentStamina)
        {
            currentStamina -= dashCost;
        }
    }
}
