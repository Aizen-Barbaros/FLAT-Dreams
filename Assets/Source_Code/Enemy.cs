using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public LayerMask targetLayerMask;
    
    private Collider[] colliders;
    private Player player;


    protected void FixedUpdate()
    {
        colliders = Physics.OverlapSphere(transform.position, 30, targetLayerMask);

        if (!base.isFrozen)
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


    public float GetNormalSpeed()
    {
        return base.normalSpeed;
    }


    public void SetNormalSpeed(float normalSpeed)
    {
        base.normalSpeed = normalSpeed;
        base.speed = base.normalSpeed;
    }


    public bool GetIsFrozen()
    {
        return base.isFrozen;
    }


    public void SetIsFrozen(bool isFrozen)
    {
        base.isFrozen = isFrozen;
    }


    public void ChasePlayer(Vector3 target)
    {
        base.Move(target);
    }


    protected override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
        if (collision.contacts.Length > 5)
            base.Jump();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StunBall")
            base.Stunned();
    }
}
