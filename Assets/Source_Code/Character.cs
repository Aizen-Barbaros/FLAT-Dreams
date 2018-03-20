using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    //============================================================
    //Public
    //============================================================
    //Jump
    public float speed;
    public float jumpSpeed;
    public float jumpHeight;

    //Speed
    public float horizontalSpeed;

    //Stun
    public GameObject StunBall;

    //Cooldown
    public float cooldownDash;
    public float cooldownStun;
    //============================================================

    //============================================================
    //Protected
    //============================================================
    //Cam
    protected float iniCamX;
    protected float iniCamY;

    //Dash
    protected float dernierDash;
    protected bool resetDash;
    protected float tempsActifDash;

    //Stun
    protected float dernierStun;

    //Position of the mouse in the screen for the cam
    protected Vector3 Position;

    protected bool isJumping { get; set; }
    //============================================================

    //============================================================
    //Private
    //============================================================
    private float step;
    private float currentJump;

    private bool isGrounded;
    private bool isFalling;
    //============================================================
    
    private void Start()
    {
        iniCamX = Input.mousePosition.x;
        iniCamY = Input.mousePosition.y;
        cooldownDash = 0;
        tempsActifDash = 0.3f;
        cooldownStun = 0;
    }

    protected virtual void FixedUpdate()
    {
        Position = GetComponent<Transform>().position; // C'EST QUOI?

        
        if (isJumping && !isFalling)
        {
            jump();
        }

        if(!isJumping && isFalling)
        {
            //fall();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Grounded")
        {
            isGrounded = false;
            Debug.Log("Leave Grounded");
        }
    }

    protected void move()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        Debug.Log(Position.y);
        transform.Translate(-z, 0, x);
        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, h, 0);
    }

    protected void move(Vector3 target)
    {
        this.step = this.speed * Time.deltaTime;

        //Follow the player
        target.y = 0;
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, step);

        //Rotation facing toward the player
        target.y = this.transform.position.y;
        this.transform.LookAt(target);
    }

    protected void jump()
    {
        Debug.Log("Jump -> currentJump : "+ currentJump +" < jumpHeight : "+jumpHeight);
        if (currentJump < jumpHeight)
        {
            float force = 0;
            force = Mathf.Sin(Time.deltaTime) * jumpSpeed; //The function who describe the mouvement
            currentJump += force;
            //GetComponent<Rigidbody>().AddForce(new Vector3(0, force * jumpSpeed, 0), ForceMode.Impulse);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 7, 0);
        }
        else
        {
            isJumping = false;
            isFalling = true;
        }
    }

    protected void fall()
    {
        Debug.Log("Fall -> !isGrounded : "+ !isGrounded);
        if (!isGrounded)
        {
            /*float force = 0;
            force = Mathf.Sin(Time.deltaTime) * jumpSpeed; //The function who describe the mouvement
            currentJump -= force;
            GetComponent<Rigidbody>().AddForce(new Vector3(0, force * jumpSpeed * -1, 0), ForceMode.Impulse);*/

        }
        else
        {
            isFalling = false;
            currentJump = 0;
        }
    }

    protected void dash()
    {
        speed = 40;
        dernierDash = Time.time;
        cooldownDash = 6;
        resetDash = true;
    }

    protected void stun()
    {
        Position = GetComponent<Transform>().position;
        Instantiate(StunBall,new Vector3(Position.x,Position.y+4,Position.z),Quaternion.identity);
        cooldownStun = 3;
        dernierStun = Time.time;
        Debug.Log("Stun");
    }

    private void cooldown(float time)
    {

    }
}
