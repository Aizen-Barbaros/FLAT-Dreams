using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public LayerMask targetLayerMask;
    
    private Collider[] colliders;
    private Player player;

    private void Start()
    {
        base.speed = 5;
    }

    protected void FixedUpdate()
    {
        colliders = Physics.OverlapSphere(transform.position, 30, targetLayerMask);

        if (colliders.Length >= 1)
        {
            this.player = colliders[0].GetComponent<Player>();
            this.ChasePlayer(this.player.transform.position);
        }

        if (base.lastfreeze + base.freezeDuration <= Time.time && resetfreeze)
        {
            Debug.Log("Stun finished");
            base.speed = 5;
            base.resetfreeze = false;
        }
    }


    public void ChasePlayer(Vector3 target)
    {
        base.Move(target);
    }

    
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
            base.Jump();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StunBall")
            base.Stunned();
    }
}
