using UnityEngine;
using System;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour {

    private enum playerState { Moving, Idle, Dead,GodModeMoving,GodModeStatic};
    private playerState currentState;
    public float unitsPerSecond;
    public float unit;
    public GameObject levelManager;
    //private static float godModeSpeed = 160.0f;
    private Vector3 initialPosition;

    private class MovementObjective
    {
        private Vector3 movementDirection;
        private Vector3 movementDestination;

        public Vector3 MovementDirection
        {
            get
            {
                return movementDirection;
            }

            set
            {
                movementDirection = value;
            }
        }

        public Vector3 MovementDestination
        {
            get
            {
                return movementDestination;
            }

            set
            {
                movementDestination = value;
            }
        }
    }

    private LinkedList<MovementObjective> movementList;
	// Use this for initialization
	void Start () {
        currentState = playerState.Idle;
        initialPosition = transform.position;
        movementList = new LinkedList<MovementObjective>();
	}
	
	// Update is called once per frame
	void Update () {
        processInput();
        updatePosition();
	}

    void OnTriggerEnter(Collider other)
    {
        if (currentState != playerState.GodModeMoving &&
            currentState != playerState.GodModeStatic)
        {
            currentState = playerState.Dead;
            levelManager.GetComponent<LevelManager>().treatPlayerCollision();
        }
    }

    public bool isMoving()
    {
        return currentState == playerState.Moving;
    }

    private void processInput() {
        if (currentState != playerState.Dead)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MovementObjective nextObjective = new MovementObjective();
                nextObjective.MovementDirection = new Vector3(0, 0, 1);
                Vector3 newDestination;
                if (movementList.Count == 0)
                {
                    if (currentState != playerState.Idle)
                        throw new InvalidOperationException("The player state should be idle if the movement list is empty");
                    newDestination = transform.position + nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                }
                else
                {
                    newDestination = movementList.Last.Value.MovementDestination;
                    newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                }
                nextObjective.MovementDestination = newDestination;
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                    currentState = playerState.Moving;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MovementObjective nextObjective = new MovementObjective();
                nextObjective.MovementDirection = new Vector3(0, 0, -1);
                Vector3 newDestination;
                if (movementList.Count == 0)
                {
                    if (currentState != playerState.Idle)
                        throw new InvalidOperationException("The player state should be idle if the movement list is empty");
                    newDestination = transform.position + nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                }
                else
                {
                    newDestination = movementList.Last.Value.MovementDestination;
                    newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                }
                nextObjective.MovementDestination = newDestination;
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                    currentState = playerState.Moving;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MovementObjective nextObjective = new MovementObjective();
                nextObjective.MovementDirection = new Vector3(1, 0, 0);
                Vector3 newDestination;
                if (movementList.Count == 0)
                {
                    if (currentState != playerState.Idle)
                        throw new InvalidOperationException("The player state should be idle if the movement list is empty");
                    newDestination = transform.position + nextObjective.MovementDirection * LevelGenerator.UnitCube.x;
                }
                else
                {
                    newDestination = movementList.Last.Value.MovementDestination;
                    newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.x;
                }
                nextObjective.MovementDestination = newDestination;
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                    currentState = playerState.Moving;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MovementObjective nextObjective = new MovementObjective();
                nextObjective.MovementDirection = new Vector3(-1, 0, 0);
                Vector3 newDestination;
                if (movementList.Count == 0)
                {
                    if (currentState != playerState.Idle)
                        throw new InvalidOperationException("The player state should be idle if the movement list is empty");
                    newDestination = transform.position + nextObjective.MovementDirection * LevelGenerator.UnitCube.x;
                }
                else
                {
                    newDestination = movementList.Last.Value.MovementDestination;
                    newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.x;
                }
                nextObjective.MovementDestination = newDestination;
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                    currentState = playerState.Moving;
                }
            }
        }
        /*else if (Input.GetKeyDown(KeyCode.G))
        {
            currentState = playerState.GodModeStatic;
        }*/
        /*if (currentState == playerState.GodModeStatic)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                movingDirection = new Vector3(0, 0, 1);
                currentState = playerState.GodModeMoving;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                movingDirection = new Vector3(0, 0, -1);
                currentState = playerState.GodModeMoving;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movingDirection = new Vector3(-1, 0, 0);
                currentState = playerState.GodModeMoving;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                movingDirection = new Vector3(1, 0, 0);
                currentState = playerState.GodModeMoving;
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                backToNormalMode();
            }
        }
        else if (currentState == playerState.GodModeMoving)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                movingDirection = new Vector3(0, 0, 1);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                movingDirection = new Vector3(0, 0, -1);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                movingDirection = new Vector3(-1, 0, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                movingDirection = new Vector3(1, 0, 0);
            }
            else
            {
                currentState = playerState.GodModeStatic;
            }
        }*/
    }

    private void backToNormalMode()
    {
        currentState = playerState.Idle;
        float xTile = (transform.position.x - initialPosition.x) / LevelGenerator.UnitCube.x;
        float zTile = (transform.position.z - initialPosition.z) / LevelGenerator.UnitCube.z;
        int intXTile = Mathf.RoundToInt(xTile);
        int intZTile = Mathf.RoundToInt(zTile);
        transform.position = new Vector3(intXTile, transform.position.y, intZTile);
    }

    private void updatePosition()
    {
        if (currentState == playerState.Moving)
        {
            Vector3 updatedPosition = transform.position + movementList.First.Value.MovementDirection * unit * unitsPerSecond * Time.deltaTime;
            if (movementList.First.Value.MovementDirection == new Vector3(0,0,1))
            {
                if (updatedPosition.z >= movementList.First.Value.MovementDestination.z)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    movementList.RemoveFirst();
                    if (movementList.Count == 0)
                    {
                        currentState = playerState.Idle;
                    }
                }
                else
                    transform.position = updatedPosition;
            }  
            else if (movementList.First.Value.MovementDirection == new Vector3(0,0,-1))
            {
                if (updatedPosition.z <= movementList.First.Value.MovementDestination.z)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    movementList.RemoveFirst();
                    if (movementList.Count == 0)
                    {
                        currentState = playerState.Idle;
                    }
                }
                else
                    transform.position = updatedPosition;
            }
            else if (movementList.First.Value.MovementDirection == new Vector3(1, 0, 0))
            {
                if (updatedPosition.x >= movementList.First.Value.MovementDestination.x)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    movementList.RemoveFirst();
                    if (movementList.Count == 0)
                    {
                        currentState = playerState.Idle;
                    }
                }
                else
                    transform.position = updatedPosition;
            }
            else if (movementList.First.Value.MovementDirection == new Vector3(-1, 0, 0))
            {
                if (updatedPosition.x <= movementList.First.Value.MovementDestination.x)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    movementList.RemoveFirst();
                    if (movementList.Count == 0)
                    {
                        currentState = playerState.Idle;
                    }
                }
                else
                    transform.position = updatedPosition;
            }
        }
        /*else if (currentState == playerState.GodModeMoving)
        {
            transform.Translate(movingDirection * godModeSpeed * Time.deltaTime);
        }*/
    }
}
