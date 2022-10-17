using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxHandler : MonoBehaviour
{
    [SerializeField] private Transform[] parallaxObjects;
    [SerializeField] private float parallaxMultiplierLoaded;
    [SerializeField] private float parallaxDistanceLoaded;
    [SerializeField] private float parallaxCalcDistance;
    [SerializeField] private Transform camera;
    [SerializeField] private Vector3 cameraLastParallaxPos;

    void Start()
    {
        parallaxMultiplierLoaded = parallaxObjects[0].GetComponent<ParallaxVertical>().parallaxMultiplier.y;
        parallaxDistanceLoaded = Mathf.Abs(parallaxObjects[0].transform.position.y - parallaxObjects[1].transform.position.y);
        parallaxCalcDistance = parallaxDistanceLoaded * parallaxMultiplierLoaded;
    }
}
