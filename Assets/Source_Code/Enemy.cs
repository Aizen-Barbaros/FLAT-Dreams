using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public LayerMask targetLayerMask;
    
    private Collider[] colliders;
    private Player player;
    private Vector3 target;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        colliders = Physics.OverlapSphere(transform.position, 30, targetLayerMask);

        //Debug.Log(" Distance :" + colliders.Length);

        if (colliders.Length > 1)
        {
            this.player = colliders[0].GetComponent<Player>();
            this.target = this.player.transform.position;
            this.chasePlayer(target);
        }
    }


    public void chasePlayer(Vector3 target)
    {
        base.move(target);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag != "Ground" && collision.gameObject.tag != "Player")
        {
            /*isJumping = true;
            jump();*/
            //StartCoroutine(Jump(target));
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
