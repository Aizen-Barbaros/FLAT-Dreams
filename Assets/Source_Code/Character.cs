using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    //CamSpeed
    protected float camSpeed;

    //Speeds
    protected float normalSpeed;
    protected float speed;

    //Jump
    protected float jumpHeight;

    //Stun
    public GameObject StunBall;

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

    //Position of the player
    protected Vector3 playerPosition;                                         

    protected bool isGrounded;
    private float step;


    private void Start()
    {
        //this.speed = 10; //Pourquoi ic? il n'est pas initialisé dans les classes filles?
        this.jumpHeight = 1.5f;
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
        //Gettting input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float h = camSpeed * Input.GetAxis("Mouse X");

        //Applied rotation on the Rigidbody
        Quaternion rotation = Quaternion.Euler(0, h, 0);
        this.GetComponent<Rigidbody>().MoveRotation(this.GetComponent<Rigidbody>().rotation * rotation);

        if (horizontal != 0 || vertical != 0)
        {
            //Le -vertical est utiliser à cause de la position de la caméra dans le prefab, peut-être devrions nous la changer?
            Vector3 movement = (this.GetComponent<Rigidbody>().rotation * rotation) * new Vector3(-vertical, 0f, horizontal);
            movement = movement.normalized * this.speed * Time.deltaTime;
            this.GetComponent<Rigidbody>().MovePosition(this.transform.position + movement);
        }
        else
        {
            if(this.isGrounded)
                this.GetComponent<Rigidbody>().velocity = new Vector3(0f, this.GetComponent<Rigidbody>().velocity.y,0f);
        }
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
            this.GetComponent<Rigidbody>().velocity = new Vector3(0f, velocity, 0f);
        }
    }


    protected void SortVitesse()
    {
        //Increase the player's speed for a short time
        this.speed *= 2;
        this.lastSpeedBoost = Time.time;
        this.speedBoostCooldown = this.speedBoostDuration + 5;
        this.resetSpeedBoost = true;
    }


    protected void Dash()
    {
        //Increase the player's speed significantly for a realy short time
        this.speed *= 4;
        this.lastDash = Time.time;
        this.dashCooldown = 6;
        this.resetDash = true;
    }


    protected void Stun()
    {
        //Create a StunBall which immobilize ennemies on contact 

        //Take the player's position
        this.playerPosition = GetComponent<Transform>().position;
        //Take the camera's orientation
        Quaternion camOrientation = GetComponentInChildren<Camera>().transform.rotation;
        //Create the StunBall at the player's position and with the camera's orientation
        stunBall =Instantiate(StunBall,new Vector3(playerPosition.x, playerPosition.y+4, playerPosition.z),camOrientation);
        //Apply mouvement to the StunBall
        stunBall.GetComponent<Rigidbody>().AddRelativeForce(0,0,50,ForceMode.Impulse);
        this.stunCooldown = 3;
        this.lastStun = Time.time;
    }

    protected void HighSenses()
    {
        //delete the fog of the world if it exists for a short time
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
        //Make the player go up very high
        this.RocketCooldown = 60;
        this.lastRocket = Time.time;
        //Mathematic function who give the velocity for a specific jump height
        float velocity = Mathf.Sqrt(2 * Physics.gravity.y * RocketHeight * -1);
        //Apply a velocity vertically
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, velocity, 0);

    }

    public void Stunned()
    {
        //Immobilize the character when called
        this.freezeDuration = 5.0f;
        this.speed = 0;
        this.lastfreeze = Time.time;
        this.resetfreeze = true;
    }
}