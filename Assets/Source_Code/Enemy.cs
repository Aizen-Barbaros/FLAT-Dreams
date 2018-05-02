using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    //The layerMask who's target by de Enemy
    public LayerMask targetLayerMask;

    //Player
    private Collider[] colliders;
    private Player player;

    //Sound
    public AudioClip BaseSound;
    private float lastBaseSound;


    //Call each 0,002 seconds
    protected void FixedUpdate()
    {
        //search a collider with the specific layerMaske in a radius of 30 blocks
        colliders = Physics.OverlapSphere(transform.position, 30, targetLayerMask);

        //If the game is on pause
        if (!base.isFrozen)
        {
            //If a collider with the specific layerMask exist
            if (colliders.Length >= 1)
            {
                //Start a coroutine wich call the moves methods for the enemy
                StopCoroutine("ChasePlayer");
                this.player = colliders[0].GetComponent<Player>();
                StartCoroutine("ChasePlayer", this.player.transform.position);
            }

            if (base.lastfreeze + base.freezeDuration <= Time.time && resetfreeze)
            {
                base.speed = base.normalSpeed;
                base.resetfreeze = false;
            }
        }
    }

    //Call when the present collider stay collid with another collider
    protected override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
        if (collision.contacts.Length > 4) //À revoir peut être éventuellement faire des classe pour chaque monstre et ajusté la valeur en conséquence du collider du monstre
            base.Jump();
    }

    //Call when the present collider collid with un triggered collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StunBall")
            base.Stunned();
    }

    //Coroutine who play the Chassing sound and call the function to move the Enemy
    IEnumerator ChasePlayer(Vector3 target)
    {
        if (source.isPlaying)
        {
            this.lastBaseSound = Time.time;
        }
        if (!source.isPlaying && Time.time - 3 >= lastBaseSound)
        {
            base.source.PlayOneShot(BaseSound, 0.05f);
        }

        base.Move(target);

        yield return null;
    }
}
