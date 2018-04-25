using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int currentLives;
    private int keyCaught;
    private bool isCaught;


    public void Start()
    {
        this.currentLives = 3;
        this.keyCaught = 0;
        this.isCaught = false;

        base.jumpHeight = 1.5f;

        base.camSpeed = 10;

        base.dashCooldown = 0;
        base.dashDuration = 0.3f;

        base.FogDuration = 2;

        base.speedBoostDuration = 2;
        base.lastSpeedBoost = Time.time;
        base.speedBoostCooldown = 0;

        base.RocketHeight = 60;
    }


    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isFrozen)
            {
                base.isFrozen = false;

                // Freeze Player
                this.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                // Freeze Camera
                this.GetComponentInChildren<CameraMouvement>().SetVerticalSpeed(7.5f);
            }

            else
            {
                base.isFrozen = true;

                // Freeze Player
                this.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                // Freeze Camera
                this.GetComponentInChildren<CameraMouvement>().SetVerticalSpeed(0);
            }
        }

        if (!base.isFrozen)
        {
            // ACTIVATIONS
            base.Move();

            if (Input.GetKeyDown(KeyCode.Space))
                base.Jump();
            
            if (Input.GetKeyDown(KeyCode.Alpha1) && base.GetSpeedBoostTimeBeforeNext() == 0)
                base.SortVitesse();

            if (Input.GetKeyDown(KeyCode.Alpha2) && base.GetDashTimeBeforeNext() == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(base.dashSound, 0.375f);
                base.Dash();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && base.GetRocketTimeBeforeNext() == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(base.RocketSound, 0.375f);
                base.Rockets();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) && base.GetFogTimeBeforeNext() == 0)
                base.HighSenses();

            if (Input.GetMouseButton(0) && base.GetStunTimeBeforeNext() == 0)
                base.Stun();

            //RESETS
            if (base.lastSpeedBoost + base.speedBoostDuration <= Time.time && base.resetSpeedBoost)
            {
                base.speed = base.normalSpeed;
                base.resetSpeedBoost = false;
            }

            if (base.lastDash + base.dashDuration <= Time.time && base.resetDash)
            {
                base.speed = base.normalSpeed;
                base.resetDash = false;
            }

            if (base.lastFog + base.FogDuration <= Time.time && base.resetFog)
            {
                if (base.normalFog)
                    RenderSettings.fog = true;
                base.resetFog = false;
            }
        }
    }


    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            this.keyCaught++;
            GameObject.Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Life")
        {
            if (this.currentLives < 3)
                this.currentLives++;
            GameObject.Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Ennemy")
            this.isCaught = true;
    }


    public int GetCurrentLives()
    {
        return this.currentLives;
    }


    public void SetCurrentLives(int currentLives)
    {
        this.currentLives = currentLives;
    }


    public int GetKeyCaught()
    {
        return this.keyCaught;
    }


    public void SetKeyCaught(int keyCaught)
    {
        this.keyCaught = keyCaught;
    }


    public bool GetCaught()
    {
        return this.isCaught;
    }


    public void SetCaught(bool isCaught)
    {
        this.isCaught = isCaught;
    }
}
