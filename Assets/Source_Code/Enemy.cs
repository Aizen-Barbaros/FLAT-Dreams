﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public LayerMask targetLayerMask;
    public AudioClip BaseSound;

    private float lastBaseSound;
    private Collider[] colliders;
    private Player player;


    protected void FixedUpdate()
    {
        colliders = Physics.OverlapSphere(transform.position, 30, targetLayerMask);

        if (!base.isFrozen)                                 //ON SERAIT PAS MIEUX DE METTRE SANS DANS LA CLASSE MERE? c'est le stun?
        {
            if (colliders.Length >= 1)
            {
                this.player = colliders[0].GetComponent<Player>();
                this.ChasePlayer(this.player.transform.position);
            }

            if (base.lastfreeze + base.freezeDuration <= Time.time && resetfreeze)
            {
                base.speed = base.normalSpeed;
                base.resetfreeze = false;
            }
        }
    }


    public void ChasePlayer(Vector3 target)
    {
        if(!source.isPlaying && Time.time-3>=lastBaseSound)    
        {
            base.source.PlayOneShot(BaseSound,0.05f);
            this.lastBaseSound = Time.time;
        }

        base.Move(target);
    }


    protected override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
        if (collision.contacts.Length > 4)
            base.Jump();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StunBall")
            base.Stunned();
    }
}
