using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    //Rigidbody
     //private Rigidbody entityRigidbody;

    //Sound
    protected AudioSource source;
    public AudioClip LandingSound;
    public AudioClip stunSound;

    //CamSpeed
    protected float camSpeed;

    //Speeds
    protected float normalSpeed;
    protected float speed;

    //Jump
    protected float jumpHeight;

    //Dash
    protected bool resetDash;
    protected float lastDash;
    protected float dashDuration;
    protected float dashCooldown;
    public AudioClip dashSound;

    //Freeze
    protected bool resetfreeze;
    protected float lastfreeze;
    protected float freezeDuration;

    //Position of the player
    protected Vector3 playerPosition; //Définir playerPosition à l'intérieur de la méthode pour qu'il se détruise quand elle est fini?

    protected bool isGrounded;
    protected bool landing;
    protected bool isFrozen;
    private float step;

    //Animation
    private Animator anim;

    private void Start()
    {
        print("type : " + this.gameObject.name);
        //Initialization
        this.jumpHeight = 1.5f;

        this.dashCooldown = 0;
        this.dashDuration = 0.3f;

       
        this.freezeDuration = 5.0f;
        this.isFrozen = false;

        this.source = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();

        //this.entityRigidbody = this.GetComponent<Rigidbody>();
    }

    //Call when the present collider enter in another collider
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            this.GetComponent<AudioSource>().PlayOneShot(this.LandingSound, 0.5f);
        }
    }

    //Call when the present collider stay collid on the same collider
    protected virtual void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
            this.isGrounded = true;
    }

    //Calle when the present collider leave a collider
    protected virtual void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
            this.isGrounded = false;
    }

    protected void Move()
    {
        //Getting input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float h = camSpeed * Input.GetAxis("Mouse X");

        //Applied rotation on the Rigidbody
        Quaternion rotation = Quaternion.Euler(0, h, 0);
        this.GetComponent<Rigidbody>().MoveRotation(this.GetComponent<Rigidbody>().rotation * rotation);

        if (this.isGrounded)
            this.GetComponent<Rigidbody>().velocity = new Vector3(0f, this.GetComponent<Rigidbody>().velocity.y, 0f);

        //Le -vertical est utiliser à cause de la position de la caméra dans le prefab, peut-être devrions nous la changer?
        Vector3 movement = (this.GetComponent<Rigidbody>().rotation * rotation) * new Vector3(-vertical, 0f, horizontal);
        movement = movement.normalized * this.speed * Time.deltaTime;
        this.GetComponent<Rigidbody>().MovePosition(this.transform.position + movement);
    }

    protected void Move(Vector3 target)
    {
        //Rotation facing toward the player
        target.y = this.transform.position.y;
        this.transform.LookAt(target);

        //Follow the player
        this.step = this.speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, this.step);

        //Animation
        anim.SetTrigger("isWalking");
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

    protected void Dash()
    {
        //Increase the entity's speed significantly for a realy short time
        this.speed *= 4;
        this.lastDash = Time.time;
        this.dashCooldown = 6;
        this.resetDash = true;
    }

    public void Stunned()
    {
        //Immobilize the character when called
        this.freezeDuration = 4.0f;
        source.PlayOneShot(stunSound,0.75f);
        this.speed = 0;
        this.lastfreeze = Time.time;
        this.resetfreeze = true;
    }


    public float GetNormalSpeed()
    {
        return this.normalSpeed;
    }


    public void SetNormalSpeed(float normalSpeed)
    {
        this.normalSpeed = normalSpeed;
        this.speed = this.normalSpeed;
    }


    public bool GetIsFrozen()
    {
        return this.isFrozen;
    }


    public void SetIsFrozen(bool isFrozen)
    {
        this.isFrozen = isFrozen;
    }


    public float GetTimeBeforeNextSpell(float time)
    {
        if (time < 0)
            return 0;
        return time;
    }


    public float GetDashTimeBeforeNext()
    {
        return GetTimeBeforeNextSpell(this.lastDash + this.dashCooldown - Time.time);
    }
}