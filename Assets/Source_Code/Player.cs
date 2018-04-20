using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int currentLives;
    private int keyCaught;
    private bool isCaught;


    void Start()
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

    
    void FixedUpdate ()
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
            base.Move();

            if (Input.GetKeyDown(KeyCode.Space))
                base.Jump();

            if (Input.GetKeyDown(KeyCode.Alpha1) && base.lastSpeedBoost + base.speedBoostCooldown <= Time.time)
                base.SortVitesse();

            if (Input.GetKeyDown(KeyCode.Alpha2) && base.dashCooldown + base.lastDash <= Time.time)
            {
                GetComponent<AudioSource>().PlayOneShot(base.dashSound, 1f);
                base.Dash();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && base.RocketCooldown + base.lastRocket <= Time.time)
            {
                GetComponent<AudioSource>().PlayOneShot(base.RocketSound, 1f);
                base.Rockets();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) && base.FogCooldown + base.lastFog <= Time.time)
                base.HighSenses();

            if (Input.GetMouseButton(0) && base.stunCooldown + base.lastStun <= Time.time)
                base.Stun();

            if (base.lastSpeedBoost + base.speedBoostDuration <= Time.time && resetSpeedBoost)
            {
                base.speed = base.normalSpeed;
                base.resetSpeedBoost = false;
            }

            if (base.lastFog + base.FogDuration <= Time.time && resetFog)
            {
                if (normalFog)
                    RenderSettings.fog = true;
                base.resetFog = false;
            }

            if (base.lastDash + base.dashDuration <= Time.time && resetDash)
            {
                base.speed = base.normalSpeed;
                base.resetDash = false;
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
        {
            this.currentLives--;
            this.isCaught = true;
        }
    }


    public float GetNormalSpeed()
    {
        return base.normalSpeed;
    }


    public void SetNormalSpeed(float normalSpeed)
    {
        base.normalSpeed = normalSpeed;
        base.speed = base.normalSpeed;
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


    public bool GetIsFrozen()
    {
        return base.isFrozen;
    }


    public void SetIsFrozen(bool isFrozen)
    {
        base.isFrozen = isFrozen;
    }
}
