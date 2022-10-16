using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaHandler : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float imageFillAmount;

    [Header("Costs")]
    public float dashCost;
    public float jumpCost;

    [Header("Stamina")]
    [Range(0,100)] public float currentStamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRegenPerSecond;
    [SerializeField] private float degenPerSecond;
    [SerializeField] private bool degen;
    
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
    public void StopDegen()
    {
        degen = false;
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
    public void CastJump()
    {
        if(jumpCost < currentStamina)
        {
            currentStamina -= jumpCost;
        }
    }
}
