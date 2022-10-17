using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimageHandler : MonoBehaviour
{
    [Header("Player Reference")]
    private Transform player;
    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer afterimageSpriteRenderer;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [Header("Color")]
    [SerializeField] private Color color;
    [Header("Time")]
    [SerializeField] private float activeTime;
    [SerializeField] private float timeActivated;
    [Header("Alphas")]
    [SerializeField] private float alpha;
    [SerializeField] private float alphaSet;
    [SerializeField] private float alphaMultiplier;

    private void OnEnable()
    {
        afterimageSpriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("PlayerSprite").transform;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        afterimageSpriteRenderer.sprite = playerSpriteRenderer.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        afterimageSpriteRenderer.color = color;

        if(Time.time >= (timeActivated + activeTime))
        {
            AfterimagePool.Instance.AddToPool(gameObject);
        }
    }
}
