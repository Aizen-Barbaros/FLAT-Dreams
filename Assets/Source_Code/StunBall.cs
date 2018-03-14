using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunBall : MonoBehaviour {
    public float Speed;
    // Use this for initialization
    void Start()
    {
        transform.Translate(Speed, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Speed, 0, 0);
    }
}
