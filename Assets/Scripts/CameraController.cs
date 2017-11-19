using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public Vector3 playerLookAtOffset;
    public float minimumSpeed, acceleration;
    private enum cameraStates { FollowingPlayer, PlayerStopped, InitialState};
    private cameraStates currentState;
    private Vector3 relatiuAPlayer;
	// Use this for initialization
	void Start () {
        relatiuAPlayer = player.transform.position - transform.position;
        transform.LookAt(player.transform.position + playerLookAtOffset);
        currentState = cameraStates.InitialState;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.transform.position - relatiuAPlayer;
        /*computeState();
        computeMovement();   */
	}
    
    void computeState()
    {
        if (currentState == cameraStates.FollowingPlayer)
        {
            if ((transform.position + relatiuAPlayer + playerLookAtOffset).z >= 
                (player.transform.position + playerLookAtOffset).z)
            {
                currentState = cameraStates.PlayerStopped;
            }
        }
        else if (player.GetComponent<PlayerController>().isMoving())
            currentState = cameraStates.FollowingPlayer;
    }
    void computeMovement()
    {
        if (currentState == cameraStates.FollowingPlayer)
        {

        }
        else if (currentState == cameraStates.FollowingPlayer)
        {

        }
    }
}
