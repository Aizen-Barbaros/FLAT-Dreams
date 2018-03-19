using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    private int Lives { get; set; }
    private int CurrentLives { get; set; }
    private int KeyCaught { get; set; }
    private Rigidbody Rb { get; set; }
    public GameObject StunBall;
    private float iniCamX;
    private float iniCamY;
    private Vector3 position;
    
    public float tempsActifSortVitesse;
    private bool resetSortVitesse;
    private float placeholder;
    public Text cooldown;
    private Transform spawner;


    private float dernierSortVitesse;
    private float cooldownSortVitesse;

    private float dernierDash;
    private bool resetDash;
    private float tempsActifDash;

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
        move();
        
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
            base.dash();
        }
        if (Input.GetMouseButton(1) && cooldownStun + dernierStun <= Time.time)
        {
            base.stun();
        }
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
        
    }

    private void Stun()
    {

    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ennemy"))
        {
            Caught();
        }
    }


}
