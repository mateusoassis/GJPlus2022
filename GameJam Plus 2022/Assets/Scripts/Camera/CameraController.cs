using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float maxYDistance;
    [SerializeField] private float currentYDistance;
    [SerializeField] private bool cameraMove;
    [SerializeField] private float targetY;
    [SerializeField] private float lerpCamera;
    private float savedStartY;

    void Start()
    {
        savedStartY = transform.position.x;
    }

    void Update()
    {
        currentYDistance = Mathf.Abs(transform.position.y - player.transform.position.y);

        if(cameraMove)
        {
            LerpCameraY();
            if(currentYDistance > maxYDistance)
            {
                cameraMove = false;
            }
        }
        else if((currentYDistance < maxYDistance || currentYDistance > maxYDistance) && !cameraMove)
        {
            cameraMove = true;
            targetY = player.transform.position.y;
        }
    }

    private void LerpCameraY()
    {
        Vector3 targetPos = new Vector3(transform.position.x, Mathf.Clamp(targetY, savedStartY, Mathf.Infinity), transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpCamera);
    }
}
