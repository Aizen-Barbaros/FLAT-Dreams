using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour{

    private int Lives { get; set; }
    private int CurrentLives { get; set; }
    private int KeyCaught { get; set; }
    private Rigidbody Rb { get; set; }
    public float speed;
    public float jump;
    public GameObject StunBall;
    private float iniCamX;
    private float iniCamY;
    public Vector3 positionCam;
    
    public float tempsActifSortVitesse;
    private bool resetSortVitesse;
    private float placeholder;
    public Text cooldown;
    private Transform spawner;

    public float horizontalSpeed;

    private float dernierSortVitesse;
    private float cooldownSortVitesse;

    public float cooldownDash;
    private float dernierDash;
    private bool resetDash;
    private float tempsActifDash;

    public float cooldownStun;
    private float dernierStun;
    

    void Start()
    {
        spawner = this.transform;
        Rb = GetComponent<Rigidbody>();
        iniCamX = Input.mousePosition.x;
        iniCamY = Input.mousePosition.y;
        dernierSortVitesse = Time.time;
        cooldownSortVitesse = 0;
        cooldownDash = 0; 
        tempsActifDash = 1/4;
        placeholder = cooldownDash + dernierDash - Time.time;
        cooldownStun = 0;
    }
	
	void Update ()
    {
        placeholder = (cooldownDash + dernierDash - Time.time);
        cooldown.text = "Cooldown Dash: " + placeholder.ToString().Normalize();
        Move();
        
        if (Input.GetKeyDown("e")&& dernierSortVitesse + cooldownSortVitesse <= Time.time)
        { 
                SortVitesse();  
        }
        if (dernierSortVitesse+tempsActifSortVitesse<=Time.time && resetSortVitesse)
        {
            speed = 5;
            resetSortVitesse = false;
        }
        if (dernierDash + tempsActifDash <= Time.time && resetDash)
        {
            speed = 5;
            resetDash = false;
        }
        if (Input.GetKeyDown("space")&&Rb.velocity.y==0)
        {
            Jump();
        }
        if(Input.GetKeyDown("q")&&cooldownDash+dernierDash<= Time.time)
        {
            Dash();
        }
        if (Input.GetMouseButton(0) && cooldownStun + dernierStun <= Time.time)
        {
            Stun();
        }
    }

    private void Move()
    {
        
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(-z, 0, x);
        float rotX = Input.mousePosition.x - iniCamX;
        float rotY = Input.mousePosition.y - iniCamY;
        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, h, 0);
    }

    private void Jump()
    {
        Rb.AddForce(new Vector3(0, jump, 0), ForceMode.Impulse);
    }

    public void Caught()
    {

    }

    private void SortVitesse()
    {
        speed=10;
        dernierSortVitesse = Time.time;
        resetSortVitesse = true;
        cooldownSortVitesse =tempsActifSortVitesse+5;
    }

    private void Dash()
    {
        //Rb.AddRelativeForce(new Vector3(Input.GetAxis("Vertical") * -5000, 0, Input.GetAxis("Horizontal") * 5000), ForceMode.Impulse);
        speed = 500;
        dernierDash = Time.time;
        resetDash = true;
    }

    private void Stun()
    {
        Instantiate(StunBall,GetComponent<Transform>().position,Quaternion.identity);
        cooldownStun = 3;
        dernierStun = Time.time;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ennemy"))
        {
            Caught();
        }
    }


}
