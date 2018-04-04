using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int currentLives;
    private int keyCaught;


    void Start()
    {
        base.iniCamX = Input.mousePosition.x;
        base.iniCamY = Input.mousePosition.y;

        this.currentLives = 3;
        this.keyCaught = 0;

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

        if (Input.GetKeyDown("space"))
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
    }


    public int GetCurrentLives()
    {
        return this.currentLives;
    }

    public int GetKeyCaught()
    {
        return this.keyCaught;
    }


    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ennemy")
        {
            this.currentLives--;
            this.gameObject.SetActive(false);
            Debug.Log(this.currentLives);
        }

        if (collision.gameObject.tag == "Key")
        {
            this.keyCaught++;
            GameObject.Destroy(collision.gameObject);
        }
    }
}
