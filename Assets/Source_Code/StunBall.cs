﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunBall : MonoBehaviour {

    private float Speed;
    // Use this for initialization
    void Start()
    {
        transform.Translate(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(1, 0, 0);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
         
        }
        else
        {
            Destroy(this.gameObject);
            Debug.Log("hit"+collision.gameObject.tag);
        }
    }
}
