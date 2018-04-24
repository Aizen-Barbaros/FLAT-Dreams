using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    private float rotationY;
    private float verticalSpeed;

    void Start ()
    {
        this.rotationY = 0.0f;
        this.verticalSpeed = 7.5f;
    }

    void Update ()
    {
        this.rotationY += Input.GetAxis("Mouse Y") * this.verticalSpeed;
        transform.localEulerAngles = new Vector3(-Mathf.Clamp(this.rotationY, -90, 90), transform.localEulerAngles.y, transform.localEulerAngles.z);
    }


    public void SetVerticalSpeed(float verticalSpeed)
    {
        this.verticalSpeed = verticalSpeed;
    }


    public float ClampAngle(float angle, float min, float max)
    {
        if (min < 0)
            min += 360;
        if (angle > 180)
            return Mathf.Clamp(angle, min, 360);
        else
            return Mathf.Clamp(angle, 0, max);
    }
}
