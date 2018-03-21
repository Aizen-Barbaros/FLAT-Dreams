using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunBall : MonoBehaviour {

    private float Speed;
    // Use this for initialization
    void Start()
    {
    }
	
	// Update is called once per frame
	void Update () {
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
         
        }
        else
        {
            //Destroy(this.gameObject);
            Debug.Log("hit"+collision.gameObject.tag);
        }
    }
}
