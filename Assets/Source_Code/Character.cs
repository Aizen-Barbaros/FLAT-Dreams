using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    //CamSpeed
    protected float camSpeed;
    
    //Jump
    protected float speed;
    protected float jumpHeight;

    //Stun
    public GameObject StunBall;

    //Cam
    protected float iniCamX;
    protected float iniCamY;

    //Sort vitesse
    protected bool resetSpeedBoost;
    protected float lastSpeedBoost;
    protected float speedBoostDuration;
    protected float speedBoostCooldown;

    //Dash
    protected bool resetDash;
    protected float lastDash;
    protected float dashDuration;
    protected float dashCooldown;

    //Freeze
    protected bool resetfreeze;
    protected float lastfreeze;
    protected float freezeDuration;

    //Stun
    protected float lastStun;
    protected float stunCooldown;
    protected GameObject stunBall;

    //Sort Fog
    protected bool resetFog;
    protected float lastFog;
    protected float FogDuration;
    protected float FogCooldown;
    protected bool normalFog;

    //Rocket
    protected float lastRocket;
    protected float RocketCooldown;
    protected float RocketHeight;

    //Position of the mouse in the screen for the cam
    protected Vector3 position;

    protected bool IsJumping { get; set; }                              //TO DO

    protected bool isGrounded;
    private float step;


    private void Start()
    {
        this.speed = 10;
        this.jumpHeight = 1.5f;
        this.camSpeed = 10;
        this.iniCamX = Input.mousePosition.x;
        this.iniCamY = Input.mousePosition.y;
        this.dashCooldown = 0;
        this.dashDuration = 0.3f;
        this.stunCooldown = 0;
        this.freezeDuration = 5.0f;
    }

    protected virtual void OnCollisionStay(Collision collision) //TO DO
    {
        if(collision.gameObject.tag == "Ground")
        {
            this.isGrounded = true;
            //Debug.Log("Grounded");
        }
    }


    protected virtual void OnCollisionExit(Collision collision) //TO DO
    {
        if(collision.gameObject.tag == "Ground")
        {
            this.isGrounded = false;
            //Debug.Log("Leave Grounded");
        }
    }


    protected void Move()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * this.speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * this.speed;

        //float h = camSpeed * Input.GetAxis("Mouse X");
        //this.transform.Rotate(0,h,0);

        //Quaternion deltaRotation = Quaternion.Euler(new Vector3(h, 0, 0) * Time.deltaTime);
        //this.GetComponent<Rigidbody>().MoveRotation(this.GetComponent<Rigidbody>().rotation * deltaRotation);
        //this.GetComponent<Rigidbody>().MovePosition(this.transform.position + new Vector3(-z, 0, x));
        this.transform.Translate(-z, 0, x);

        float h = camSpeed * Input.GetAxis("Mouse X");
        this.transform.Rotate(0, h, 0);
    }


    protected void Move(Vector3 target)
    {
        this.step = this.speed * Time.deltaTime;

        //Follow the player
        //target.y = 0;                                                                           //A REVOIR
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, this.step);

        //Rotation facing toward the player
        target.y = this.transform.position.y;
        this.transform.LookAt(target);
    }


    protected void Jump()
    {
        if (this.isGrounded)
        {
            //Mathematic function who give the velocity for a specific jump height
            float velocity = Mathf.Sqrt(2 * Physics.gravity.y * this.jumpHeight * -1);
            //Apply a velocity vertically
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, velocity, 0);
        }
    }


    protected void SortVitesse()
    {
        this.speed *= 2;
        this.lastSpeedBoost = Time.time;
        this.speedBoostCooldown = this.speedBoostDuration + 5;
        this.resetSpeedBoost = true;
    }


    protected void Dash()
    {
        this.speed = 30;
        this.lastDash = Time.time;
        this.dashCooldown = 6;
        this.resetDash = true;
    }


    protected void Stun()
    {
        this.position = GetComponent<Transform>().position;
        Quaternion camOrientation = GetComponentInChildren<Camera>().transform.rotation;
        stunBall=Instantiate(StunBall,new Vector3(position.x, position.y+4, position.z),camOrientation);
        stunBall.GetComponent<Rigidbody>().AddRelativeForce(0,0,50,ForceMode.Impulse);
        this.stunCooldown = 3;
        this.lastStun = Time.time;
    }

    protected void HighSenses()
    {
        this.lastFog = Time.time;
        this.FogCooldown = 30;
        this.resetFog = true;
        if (RenderSettings.fog)
        {
            RenderSettings.fog = false;
            normalFog = true;
        }
        else
            normalFog = false;
    }

    protected void Rockets()
    {
        this.RocketCooldown = 60;
        this.lastRocket = Time.time;
        //Mathematic function who give the velocity for a specific jump height
        float velocity = Mathf.Sqrt(2 * Physics.gravity.y * RocketHeight * -1);
        //Apply a velocity vertically
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, velocity, 0);

    }

    public void Stunned()
    {
        Debug.Log("Stunned!");
        this.freezeDuration = 5.0f;
        this.speed = 0;
        this.lastfreeze = Time.time;
        this.resetfreeze = true;
    }
}