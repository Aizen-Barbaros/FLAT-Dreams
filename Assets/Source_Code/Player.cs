using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int currentLives;
    private int keyCaught;
    private bool isCaught;

    //Sort vitesse
    protected bool resetSpeedBoost;
    protected float lastSpeedBoost;
    protected float speedBoostDuration;
    protected float speedBoostCooldown;

    //Stun
    protected float lastStun;
    protected float stunCooldown;
    protected GameObject stunBall;
    public GameObject StunBall;

    //Sort Fog
    protected bool resetFog;
    protected float lastFog;
    protected float FogDuration;
    protected float FogCooldown;
    protected bool normalFog;

    //Rocket
    protected float lastRocket;
    protected float RocketCooldown;
    protected float RocketHeight;
    public AudioClip RocketSound;

    public void Start()
    {
        this.currentLives = 3;
        this.keyCaught = 0;
        this.isCaught = false;

        base.jumpHeight = 1.5f;

        base.camSpeed = 10;

        base.dashCooldown = 0;
        base.dashDuration = 0.3f;

        this.FogDuration = 2;

        this.speedBoostDuration = 2;
        this.lastSpeedBoost = Time.time;
        this.speedBoostCooldown = 0;

        this.RocketHeight = 60;

    }

    public void Update()
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

            if (Input.GetKeyDown(KeyCode.Alpha1) && this.GetSpeedBoostTimeBeforeNext() == 0)
                this.SpeedBoost();

            if (Input.GetKeyDown(KeyCode.Alpha2) && base.GetDashTimeBeforeNext() == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(base.dashSound, 0.375f);
                base.Dash();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && this.GetRocketTimeBeforeNext() == 0)
            {
                GetComponent<AudioSource>().PlayOneShot(this.RocketSound, 0.375f);
                this.Rockets();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) && this.GetFogTimeBeforeNext() == 0)
                this.HighSenses();

            if (Input.GetMouseButton(0) && this.GetStunTimeBeforeNext() == 0)
                this.Stun();

            //RESETS
            if (this.lastSpeedBoost + this.speedBoostDuration <= Time.time && this.resetSpeedBoost)
            {
                base.speed = base.normalSpeed;
                this.resetSpeedBoost = false;
            }

            if (base.lastDash + base.dashDuration <= Time.time && base.resetDash)
            {
                base.speed = base.normalSpeed;
                base.resetDash = false;
            }

            if (this.lastFog + this.FogDuration <= Time.time && this.resetFog)
            {
                if (this.normalFog)
                    RenderSettings.fog = true;
                this.resetFog = false;
            }
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

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

    protected void SpeedBoost() //Changer nom de méthode? En anglais
    {
        //Increase the entity's speed for a short time
        this.speed *= 2;
        this.lastSpeedBoost = Time.time;
        this.speedBoostCooldown = this.speedBoostDuration + 5;
        this.resetSpeedBoost = true;
    }

    protected void Stun()
    {
        //Create a StunBall which immobilize ennemies on contact 

        //Take the player's position
        this.playerPosition = GetComponent<Transform>().position; //Remplacer par this.transform.position? au lieu du getComponent? Définir playerPosition à l'intérieur de la méthode pour qu'il se détruise quand elle est fini?
        //Take the camera's orientation
        Quaternion camOrientation = GetComponentInChildren<Camera>().transform.rotation;
        //Create the StunBall at the player's position and with the camera's orientation
        stunBall = Instantiate(StunBall, new Vector3(playerPosition.x, playerPosition.y + 2, playerPosition.z), camOrientation);
        //Apply mouvement to the StunBall
        stunBall.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 50, ForceMode.Impulse);
        this.stunCooldown = 5;
        this.lastStun = Time.time;
    }

    protected void HighSenses() //Mettre dans player car mob jamais utiliser ça?
    {
        //delete the fog of the world if it exists for a short time
        this.lastFog = Time.time;
        this.FogCooldown = 30;
        this.resetFog = true;
        if (RenderSettings.fog)
        {
            RenderSettings.fog = false;
            normalFog = true;
        }
        else
            normalFog = false;
    }

    protected void Rockets() //Mettre dans player car mob jamais utiliser ça?
    {
        //Make the player go up very high
        this.RocketCooldown = 60;
        this.lastRocket = Time.time;
        //Mathematic function who give the velocity for a specific jump height
        float velocity = Mathf.Sqrt(2 * Physics.gravity.y * RocketHeight * -1);
        //Apply a velocity vertically
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, velocity, 0);

    }



    public float GetSpeedBoostTimeBeforeNext()
    {
        return GetTimeBeforeNextSpell(this.lastSpeedBoost + this.speedBoostCooldown - Time.time);
    }

    public float GetStunTimeBeforeNext()
    {
        return GetTimeBeforeNextSpell(this.lastStun + this.stunCooldown - Time.time);
    }

    public float GetFogTimeBeforeNext()
    {
        return GetTimeBeforeNextSpell(this.lastFog + this.FogCooldown - Time.time);
    }

    public float GetRocketTimeBeforeNext()
    {
        return GetTimeBeforeNextSpell(this.lastRocket + this.RocketCooldown - Time.time);
    }
}
