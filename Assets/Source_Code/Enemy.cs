using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public LayerMask targetLayerMask;
    
    private Collider[] colliders;
    private Player player;

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();
        colliders = Physics.OverlapSphere(transform.position, 30, targetLayerMask);

        if (colliders.Length > 1)
        {
            this.player = colliders[0].GetComponent<Player>();
            this.chasePlayer(this.player.transform.position);
        }
    }


    public void chasePlayer(Vector3 target)
    {
        base.Move(target);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag != "Player")
        {
            base.Jump();
        }
    }

    private void shortPath()
    {

    }

    private void onCatch()
    {

    }

    public void stunned()
    {

    }
}
