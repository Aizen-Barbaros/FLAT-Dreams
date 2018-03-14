using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour {
    public float verticalSpeed;
    private float iniCamY;
    public Vector3 positionCam;
    // Use this for initialization
    void Start () {
        iniCamY = Input.mousePosition.y;
        positionCam = transform.position;
    }

    // Update is called once per frame
    void Update () {
        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(-v, 0, 0);
        positionCam = transform.position;
    }
}
