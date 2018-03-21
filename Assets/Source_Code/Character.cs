using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Jump
    protected float speed;
    protected float jumpHeight;

    //CamSpeed
    protected float camSpeed;

    //Stun
    protected GameObject StunBall;

    //Cam
    protected float iniCamX;
    protected float iniCamY;

    //Dash
    protected bool resetDash;
    protected float lastDash;
    protected float dashDuration;
    protected float dashCooldown;

    //Stun
    protected float lastStun;
    protected float stunCooldown;

    //Sort vitesse
    protected bool resetSpeedBoost;
    protected float lastSpeedBoost;
    protected float speedBoostDuration;
    protected float speedBoostCooldown;

    //Position of the mouse in the screen for the cam
    protected Vector3 position;

    protected bool IsJumping { get; set; }                              //TO DO

    protected bool isGrounded;
    private float step;

    private void Start()
    {
        this.iniCamX = Input.mousePosition.x;
        this.iniCamY = Input.mousePosition.y;
        this.dashCooldown = 0;
        this.dashDuration = 0.3f;
        this.stunCooldown = 0;
    }

    private void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        this.position = GetComponent<Transform>().position; // C'EST QUOI?
    }

    protected virtual void OnCollisionEnter(Collision collision) //TO DO
    {
        if(collision.gameObject.tag == "Ground")
        {
            this.isGrounded = true;
            Debug.Log("Grounded");
        }
    }

    protected virtual void OnCollisionExit(Collision collision) //TO DO
    {
        if(collision.gameObject.tag == "Ground")
        {
            this.isGrounded = false;
            Debug.Log("Leave Grounded");
        }
    }

    protected void Move()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * this.speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * this.speed;
        Debug.Log(position.y);

        this.transform.Translate(-z, 0, x);

        float h = camSpeed * Input.GetAxis("Mouse X");
        this.transform.Rotate(0, h, 0);
    }

    protected void Move(Vector3 target)
    {
        this.step = this.speed * Time.deltaTime;

        //Follow the player
        target.y = 0;                                                                           //A REVOIR
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, this.step);

        //Rotation facing toward the player
        target.y = this.transform.position.y;
        this.transform.LookAt(target);
    }

    protected void Jump()
    {
        //Mathematic function who give the velocity for a specific jump height
        float velocity = Mathf.Sqrt(2 * Physics.gravity.y * this.jumpHeight * -1);
        //Apply a velocity vertically
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, velocity, 0);
    }

    protected void SortVitesse()
    {
        this.speed = 10;
        this.lastSpeedBoost = Time.time;
        this.speedBoostCooldown = this.speedBoostDuration + 5;
        this.resetSpeedBoost = true;
    }

    protected void Dash()   // MEME CHOSE QUE LE SORT DE VITESSE
    {
        this.speed = 40;
        this.lastDash = Time.time;
        this.dashCooldown = 6;
        this.resetDash = true;
    }

    protected void Stun()
    {
        this.position = GetComponent<Transform>().position;
        Instantiate(StunBall,new Vector3(position.x, position.y+4, position.z), Quaternion.identity);
        this.stunCooldown = 3;
        this.lastStun = Time.time;
        Debug.Log("Stun");
    }
}