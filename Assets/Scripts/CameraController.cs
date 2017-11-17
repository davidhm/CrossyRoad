using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public Vector3 playerLookAtOffset;
    private Vector3 posicio;
	// Use this for initialization
	void Start () {
        posicio = transform.position - player.transform.position;
        //transform.LookAt()
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.transform.position + posicio;
	}
}
