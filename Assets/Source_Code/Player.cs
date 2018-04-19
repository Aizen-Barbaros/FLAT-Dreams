using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int currentLives;
    private int keyCaught;
    private bool isCaught;
    private bool isFrozen;


    void Start()
    {
        this.currentLives = 3;
        this.keyCaught = 0;
        this.isCaught = false;
        this.isFrozen = false;

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

    private void Update()
    {
        base.Move();    
    }
    
    void FixedUpdate ()
    {
        if (Input.GetKeyDown("space"))
            base.Jump();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isFrozen)
            {
                this.isFrozen = false;

                // Freeze Player
                this.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                // Freeze Camera
                this.GetComponentInChildren<Camera>().clearFlags = CameraClearFlags.Skybox;
                this.GetComponentInChildren<Camera>().cullingMask = 1;
            }

            else
            {
                this.isFrozen = true;

                // Freeze Player
                this.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                // Freeze Camera
                this.GetComponentInChildren<Camera>().clearFlags = CameraClearFlags.Nothing;
                this.GetComponentInChildren<Camera>().cullingMask = 0;
            }
        }

        if (Input.GetKeyDown("1") && base.lastSpeedBoost + base.speedBoostCooldown <= Time.time)
            base.SortVitesse();

        if (Input.GetKeyDown("2") && base.dashCooldown + base.lastDash <= Time.time)
        {
            GetComponent<AudioSource>().PlayOneShot(base.dashSound, 1f);
            base.Dash();
        }
        if (Input.GetMouseButton(0) && base.stunCooldown + base.lastStun <= Time.time)
            base.Stun();

        if (Input.GetKeyDown("4") && base.FogCooldown + base.lastFog <= Time.time)
            base.HighSenses();

        if (Input.GetKeyDown("3") && base.RocketCooldown + base.lastRocket <= Time.time)
            base.Rockets();

        if (base.lastSpeedBoost + base.speedBoostDuration <= Time.time && resetSpeedBoost)
        {
            base.speed = base.normalSpeed;
            base.resetSpeedBoost = false;
        }

        if (base.lastFog + base.FogDuration <= Time.time && resetFog)
        {
            if(normalFog)
                RenderSettings.fog = true;
            base.resetFog = false;
        }

        if (base.lastDash + base.dashDuration <= Time.time && resetDash)
        {
            base.speed = base.normalSpeed;
            base.resetDash = false;
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
        return this.isFrozen;
    }


    public void SetIsFrozen(bool isFrozen)
    {
        this.isFrozen = isFrozen;
    }
}
