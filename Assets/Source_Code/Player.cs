using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    private int Lives { get; set; }
    private int CurrentLives { get; set; }
    private int KeyCaught { get; set; }
    private Rigidbody Rb { get; set; }
    
    private Vector3 position;
    
    public float tempsActifSortVitesse;
    private bool resetSortVitesse;
    private float placeholder;
    public Text cooldown;
    private Transform spawner;

    public float jumper;


    private float dernierSortVitesse;
    private float cooldownSortVitesse;
 

    void Start()
    {
        spawner = this.transform;
        Rb = GetComponent<Rigidbody>();
        
        base.iniCamX = Input.mousePosition.x;
        base.iniCamY = Input.mousePosition.y;
        dernierSortVitesse = Time.time;
        cooldownSortVitesse = 0;
        base.cooldownDash = 0; 
        base.tempsActifDash = 0.3f;
        placeholder = base.cooldownDash + base.dernierDash - Time.time;
    }
	
	void Update ()
    {
        placeholder = (base.cooldownDash + base.dernierDash - Time.time);
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
        if (base.dernierDash + base.tempsActifDash <= Time.time && resetDash)
        {
            speed = 5;
            resetDash = false;
        }
        if (Input.GetKeyDown("space")&&(Rb.velocity.y>=-1&&Rb.velocity.y<=1))
        {
            Jump();
        }
        if(Input.GetKeyDown("q")&& base.cooldownDash + base.dernierDash <= Time.time)
        {
            base.dash();
        }
        if (Input.GetMouseButton(1) && base.cooldownStun + base.dernierStun <= Time.time)
        {
            base.stun();
        }
    }

    private void Jump()
    {
        Rb.AddRelativeForce(new Vector3(20f, jumper, 0), ForceMode.Impulse);
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
