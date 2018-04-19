using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    private float verticalSpeed;
    private Vector3 positionCam;

    void Start ()
    {
        this.verticalSpeed = 7.5f;
        this.positionCam = transform.position;
    }

    void Update ()
    {
        transform.Rotate(-this.verticalSpeed * Input.GetAxis("Mouse Y"), 0, 0);
        positionCam = transform.position;
    }
}
