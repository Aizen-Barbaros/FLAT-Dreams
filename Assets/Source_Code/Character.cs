using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float speed;
    public float jumpSpeed;
    public float jumpHeight;

    public float horizontalSpeed;

    protected float iniCamX;
    protected float iniCamY;
    public GameObject StunBall;

    public float cooldownDash;
    protected float dernierDash;
    protected bool resetDash;
    protected float tempsActifDash;

    public float cooldownStun;
    protected float dernierStun;

    private float step;
    private float currentJump;
    private float jumpForce;

    protected Vector3 Position;

    protected bool isJumping { get; set; }
    
    private bool isGrounded;
    private bool isFalling;

    private void Start()
    {
        this.jumpForce = 0;
        iniCamX = Input.mousePosition.x;
        iniCamY = Input.mousePosition.y;
        cooldownDash = 0;
        tempsActifDash = 0.3f;
        cooldownStun = 0;

    }

    protected virtual void FixedUpdate()
    {
        Position = GetComponent<Transform>().position;
        if (isJumping && !isFalling)
        {
            jump();
        }

        if(!isJumping && isFalling)
        {
            fall();
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

    //Test
    protected IEnumerator Jump(Vector3 direction)
    {
        isJumping = true;
        Vector3 startPoint = transform.position;
        Vector3 targetPoint = startPoint + direction;
        float time = 0;
        float jumpProgress = 0;
        float velocityY = jumpStartV;
        float height = startPoint.y;

        while (isJumping)
        {
            jumpProgress = time / jumpDuration;

            if (jumpProgress > 1)
            {
                isJumping = false;
                jumpProgress = 1;
            }

            Vector3 currentPos = Vector3.Lerp(startPoint, targetPoint, jumpProgress);
            currentPos.y = height;
            transform.position = currentPos;

            //Wait until next frame.
            yield return null;

            height += velocityY * Time.deltaTime;
            velocityY += Time.deltaTime * Physics.gravity.y;
            time += Time.deltaTime;
        }

        transform.position = targetPoint;
        yield break;
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
