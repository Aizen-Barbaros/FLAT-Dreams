using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int lives;
    private int currentLives;
    private int keyCaught=0;


    void Start()
    {
        base.iniCamX = Input.mousePosition.x;
        base.iniCamY = Input.mousePosition.y;

        base.jumpHeight = 1.5f;

        base.camSpeed = 10;

        base.speed = 8;

        base.dashCooldown = 0;
        base.dashDuration = 0.3f;

        base.speedBoostDuration = 2;
        base.lastSpeedBoost = Time.time;
        base.speedBoostCooldown = 0;
    }


    void FixedUpdate ()
    {
        base.Move();
        if (Input.GetKeyDown("space"))//&& base.isGrounded)
        {
            base.Jump();
        }
            

        if (Input.GetKeyDown("e") && base.lastSpeedBoost + base.speedBoostCooldown <= Time.time)
            base.SortVitesse();

        if (Input.GetKeyDown("q") && base.dashCooldown + base.lastDash <= Time.time)
            base.Dash();

        if (Input.GetMouseButton(0) && base.stunCooldown + base.lastStun <= Time.time)
            base.Stun();

        if (base.lastSpeedBoost + base.speedBoostDuration <= Time.time && resetSpeedBoost)
        {
            base.speed = 8;
            base.resetSpeedBoost = false;
        }

        if (base.lastDash + base.dashDuration <= Time.time && resetDash)
        {
            base.speed = 8;
            base.resetDash = false;
        }
        if (keyCaught>=3)
        {
            Debug.Log("CONGRAT");
        }
    }


    public void Caught()
    {

    }


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ennemy")
            Caught();
        if (collision.gameObject.tag == "Key")
        {
            keyCaught++;
            GameObject.Destroy(collision.gameObject);
            Debug.Log(keyCaught);
        }
    }
}
