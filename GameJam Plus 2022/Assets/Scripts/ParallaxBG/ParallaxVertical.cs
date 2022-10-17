using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxVertical : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 lastCameraPosition;
    public Vector2 parallaxMultiplier;
    
    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }
    
    void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y, 0);
        lastCameraPosition = cameraTransform.position;
    }
}
