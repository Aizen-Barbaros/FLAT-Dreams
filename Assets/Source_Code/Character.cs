using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    //============================================================
    //Public
    //============================================================
    //Jump
    public float speed;                                             //Speed de GAB
    public float jumpHeight;

    //Speed
    public float horizontalSpeed;                                   //Speed de Félix; Même speed que le premier à revoir

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

    protected bool isJumping { get; set; }                              //TO DO
    //============================================================

    //============================================================
    //Private
    //============================================================
    private float step;

    private bool isGrounded;
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
    }

    protected virtual void OnCollisionEnter(Collision collision) //TO DO
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
    }

    protected virtual void OnCollisionExit(Collision collision) //TO DO
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
        target.y = 0;                                                                           //A REVOIR
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, step);

        //Rotation facing toward the player
        target.y = this.transform.position.y;
        this.transform.LookAt(target);
    }

    protected void jump()
    {
        //Mathematic function who give the velocity for a specific jump height
        float velocity = Mathf.Sqrt(2 * Physics.gravity.y * jumpHeight *-1);
        //Apply a velocity vertically
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, velocity, 0);
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

    private void cooldown(float time) //Trouver un moyen de faire un cooldown commun?
    {

    }
}

//NOTE
// notation des membre de la classe utilisation du this.
