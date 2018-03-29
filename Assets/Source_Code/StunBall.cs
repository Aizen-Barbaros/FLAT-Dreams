using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunBall : MonoBehaviour {
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Terrain")|| collision.gameObject.CompareTag("Ennemy")||collision.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
            Debug.Log("hit" + collision.gameObject.tag);
        }
        else
        {

        }
    }
}
