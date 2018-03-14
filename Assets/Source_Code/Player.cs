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
    private Vector3 position;
    
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
        tempsActifDash = 0.3f;
        placeholder = cooldownDash + dernierDash - Time.time;
        cooldownStun = 0;
    }
	
	void Update ()
    {
        position = GetComponent<Transform>().position;
        placeholder = (cooldownDash + dernierDash - Time.time);
        //cooldown.text = "Cooldown Dash: " + placeholder.ToString().Normalize();
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
        if (Input.GetKeyDown("space")&&(Rb.velocity.y>=-1&&Rb.velocity.y<=1))
        {
            Jump();
        }
        if(Input.GetKeyDown("q")&&cooldownDash+dernierDash<= Time.time)
        {
            Dash();
        }
        if (Input.GetMouseButton(1) && cooldownStun + dernierStun <= Time.time)
        {
            Stun();
        }
    }

    private void Move()
    {
        
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        Debug.Log(position.y);
        transform.Translate(-z, 0, x);
        float rotX = Input.mousePosition.x - iniCamX;
        float rotY = Input.mousePosition.y - iniCamY;
        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, h, 0);
    }

    private void Jump()
    {
        Rb.AddRelativeForce(new Vector3(20f, jump, 0), ForceMode.Impulse);
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
        speed = 40;
        dernierDash = Time.time;
        cooldownDash = 6;
        resetDash = true;
    }

    private void Stun()
    {
        position = GetComponent<Transform>().position;
        //Instantiate(StunBall,new Vector3(position.x,position.y+4,position.z),Quaternion.identity);
        cooldownStun = 3;
        dernierStun = Time.time;
        Debug.Log("Stun");
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ennemy"))
        {
            Caught();
        }
    }


}
