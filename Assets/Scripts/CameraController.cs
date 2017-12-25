using UnityEngine;
public enum cameraStates { GameStarted, PlayerDead, InitialState, CameraPaused };

public class CameraController : MonoBehaviour {
    public GameObject player;
    public Vector3 playerLookAtOffset;
    public float idleForwardSpeed, maxPlayerForwardDistance;
    private float currentForwardSpeed, playerToCenterDistance, maxForwardSpeed;    
    private bool gameStarted;
    private cameraStates currentState;
    private Vector3 cameraLookAt;

    public cameraStates CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
        }
    }

    // Use this for initialization
    void Start () {
        cameraLookAt = (player.transform.position + playerLookAtOffset) - transform.position;
        transform.LookAt(player.transform.position + playerLookAtOffset);
        currentState = cameraStates.InitialState;
        playerToCenterDistance = playerLookAtOffset.z;
        maxForwardSpeed = LevelGenerator.UnitCube.x * player.GetComponent<PlayerController>().playerSpeed;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        computeState();
        computeMovement();
	}
    
    void computeState()
    {
        if (currentState == cameraStates.InitialState && player.GetComponent<PlayerController>().isMoving())
        {
            currentState = cameraStates.GameStarted;
            currentForwardSpeed = idleForwardSpeed;
        }
        else if (currentState == cameraStates.GameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                currentState = cameraStates.CameraPaused;
            }
            else
            {
                if (player.transform.position.z + playerToCenterDistance > (transform.position + cameraLookAt).z)
                {
                    float difference = (player.transform.position.z + playerToCenterDistance) - (transform.position + cameraLookAt).z;
                    currentForwardSpeed = Mathf.Lerp(idleForwardSpeed, maxForwardSpeed, difference / maxPlayerForwardDistance);
                }
                else
                {
                    currentForwardSpeed = idleForwardSpeed;
                }
            }
        }
    }
    void computeMovement()
    {
        if (currentState == cameraStates.GameStarted)
        {
            Vector3 increment = new Vector3();
            increment.z = Time.deltaTime * currentForwardSpeed;
            transform.Translate(increment,Space.World);
        }
    }
}
