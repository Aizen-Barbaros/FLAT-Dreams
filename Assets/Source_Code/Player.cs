using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Player : MonoBehaviour {
    public int life { get; set; }
    public int maxLife { get; set; }

    // Use this for initialization
    void Start () {
         
	}   

    public void lostLife()
    {
        //TODO
        if (life <= 0)
            wakeUp();
    }

    public void wakeUp()
    {
        //TODO
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
