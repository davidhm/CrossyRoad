using UnityEngine;

public class PlayerController : MonoBehaviour {

    public enum playerState { Moving, Idle, Dead};
    private playerState currentState;
    private Vector3 newPosition;
    private Vector3 movingDirection;
    public float unitsPerSecond;
    public float unit;
    public GameObject levelManager;
	// Use this for initialization
	void Start () {
        currentState = playerState.Idle;
        newPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        processInput();
        updatePosition();
	}

    void OnTriggerEnter(Collider other)
    {
        currentState = playerState.Dead;
        levelManager.GetComponent<LevelManager>().treatPlayerCollision();
    }

    public void setPlayerState(playerState newState)
    {
        currentState = newState;
    }

    public bool isMoving()
    {
        return currentState == playerState.Moving;
    }

    public Vector3 getNewPosition()
    {
        return newPosition;
    }

    private void processInput() {
        if (currentState == playerState.Idle && Input.GetKeyDown(KeyCode.UpArrow))
        {
            newPosition.z += unit;
            currentState = playerState.Moving;
            movingDirection = new Vector3(0, 0, 1);  
        }
        else if (currentState == playerState.Idle && Input.GetKeyDown(KeyCode.DownArrow))
        {
            newPosition.z -= unit;
            currentState = playerState.Moving;
            movingDirection = new Vector3(0, 0, -1);
        }
        else if (currentState == playerState.Idle && Input.GetKeyDown(KeyCode.RightArrow))
        {
            newPosition.x += unit;
            currentState = playerState.Moving;
            movingDirection = new Vector3(1, 0, 0);
        }
        else if (currentState == playerState.Idle && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newPosition.x -= unit;
            currentState = playerState.Moving;
            movingDirection = new Vector3(-1, 0, 0);
        }
    }

    private void updatePosition()
    {
        if (currentState == playerState.Moving)
        {
            Vector3 updatedPosition = transform.position + movingDirection * unit * unitsPerSecond * Time.deltaTime;
            if (movingDirection == new Vector3(0,0,1))
            {
                if (updatedPosition.z >= newPosition.z)
                {
                    transform.position = newPosition;
                    currentState = playerState.Idle;
                }
                else
                    transform.position = updatedPosition;
            }  
            else if (movingDirection == new Vector3(0,0,-1))
            {
                if (updatedPosition.z <= newPosition.z)
                {
                    transform.position = newPosition;
                    currentState = playerState.Idle;
                }
                else
                    transform.position = updatedPosition;
            }
            else if (movingDirection == new Vector3(1, 0, 0))
            {
                if (updatedPosition.x >= newPosition.x)
                {
                    transform.position = newPosition;
                    currentState = playerState.Idle;
                }
                else
                    transform.position = updatedPosition;
            }
            else if (movingDirection == new Vector3(-1, 0, 0))
            {
                if (updatedPosition.x <= newPosition.x)
                {
                    transform.position = newPosition;
                    currentState = playerState.Idle;
                }
                else
                    transform.position = updatedPosition;
            }
        }
    }
}
