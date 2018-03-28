using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    private float verticalSpeed;
    private float iniCamY;
    private Vector3 positionCam;

    void Start ()
    {
        this.verticalSpeed = 7.5f;
        this.iniCamY = Input.mousePosition.y;
        this.positionCam = transform.position;
    }

    void Update ()
    {
        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(-v, 0, 0);
        positionCam = transform.position;
        //Debug.Log(this.gameObject.transform.rotation.eulerAngles.y);
    }
}
