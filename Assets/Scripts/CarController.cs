using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    public float angularFreq;
    private Vector3 initialPosition;
	// Use this for initialization
	void Start () {
        angularFreq = Mathf.PI ;
        initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float xPosition = 4 * Mathf.Sin(angularFreq * Time.realtimeSinceStartup);
        transform.position = new Vector3(xPosition + initialPosition.x, transform.position.y,
            transform.position.z);
	}
}
