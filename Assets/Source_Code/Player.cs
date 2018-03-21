using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int lives;
    private int currentLives;
    private int keyCaught;


    void Start()
    {
        base.iniCamX = Input.mousePosition.x;
        base.iniCamY = Input.mousePosition.y;

        base.jumpHeight = 10;

        base.camSpeed = 10;

        base.speed = 5;

        base.dashCooldown = 0;
        base.dashDuration = 0.3f;

        base.speedBoostDuration = 2;
        base.lastSpeedBoost = Time.time;
        base.speedBoostCooldown = 0;
    }


    void Update ()
    {
        base.Move();
        Debug.Log("OK");
        if (Input.GetKeyDown("space")&& base.isGrounded)
            base.Jump();

        if (Input.GetKeyDown("e") && base.lastSpeedBoost + base.speedBoostCooldown <= Time.time)
            base.SortVitesse();

        if (Input.GetKeyDown("q") && base.dashCooldown + base.lastDash <= Time.time)
            base.Dash();

        if (Input.GetMouseButton(1) && base.stunCooldown + base.lastStun <= Time.time)
            base.Stun();

        if (base.lastSpeedBoost + base.speedBoostDuration <= Time.time && resetSpeedBoost)
        {
            base.speed = 5;
            base.resetSpeedBoost = false;
        }

        if (base.lastDash + base.dashDuration <= Time.time && resetDash)
        {
            base.speed = 5;
            base.resetDash = false;
        }        
    }


    public void Caught()
    {

    }


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ennemy")
            Caught();
    }
}
