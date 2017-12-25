using UnityEngine;
using System;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour {

    private enum playerState { Moving, Idle, Dead,DrownWalk,Drowning};
    private playerState currentState;
    public float unitsPerSecond;
    public float unit;
    public GameObject levelManager;
    //private static float godModeSpeed = 160.0f;
    private Vector3 initialPosition;
    private bool debugIdle;
    private bool godMode;
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
        debugIdle = true;
        godMode = false;
	}
	
	// Update is called once per frame
	void Update () {
        processInput();
        updatePosition();
	}

    void OnTriggerEnter(Collider other)
    {
        if (!godMode)
        {
            currentState = playerState.Dead;
            levelManager.GetComponent<LevelManager>().treatPlayerCollision();
        }
    }

    void OnBecameInvisible()
    {
        currentState = playerState.Dead;
        levelManager.GetComponent<LevelManager>().treatPlayerInvisible();
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
                rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
                if (targetType == rowType.Water)
                {
                    if (levelManager.GetComponent<LevelManager>().checkIfTrunkInPosition(nextObjective.MovementDestination))
                    {
                        currentState = playerState.Moving;
                    }
                    else
                    {
                        currentState = playerState.DrownWalk;
                    }
                    movementList.AddLast(nextObjective);
                }
                else
                {
                    if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                    {
                        movementList.AddLast(nextObjective);
                        currentState = playerState.Moving;
                    }
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
                rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
                if (targetType == rowType.Water)
                {
                    if (levelManager.GetComponent<LevelManager>().checkIfTrunkInPosition(nextObjective.MovementDestination))
                    {
                        currentState = playerState.Moving;
                    }
                    else
                    {
                        currentState = playerState.DrownWalk;
                    }
                    movementList.AddLast(nextObjective);
                }
                else
                {
                    if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                    {
                        movementList.AddLast(nextObjective);
                        currentState = playerState.Moving;
                    }
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
                rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
                if (targetType == rowType.Water)
                {
                    if (levelManager.GetComponent<LevelManager>().checkIfTrunkInPosition(nextObjective.MovementDestination))
                    {
                        currentState = playerState.Moving;
                    }
                    else
                    {
                        currentState = playerState.DrownWalk;
                    }
                    movementList.AddLast(nextObjective);
                }
                else
                {
                    if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                    {
                        movementList.AddLast(nextObjective);
                        currentState = playerState.Moving;
                    }
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
                rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
                if (targetType == rowType.Water)
                {
                    if (levelManager.GetComponent<LevelManager>().checkIfTrunkInPosition(nextObjective.MovementDestination))
                    {
                        currentState = playerState.Moving;
                    }
                    else
                    {
                        currentState = playerState.DrownWalk;
                    }
                    movementList.AddLast(nextObjective);
                }
                else
                {
                    if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                    {
                        movementList.AddLast(nextObjective);
                        currentState = playerState.Moving;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                godMode = true;
            }
        }        
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
        if (currentState == playerState.Moving || currentState == playerState.DrownWalk)
        {
            Vector3 updatedPosition = transform.position + movementList.First.Value.MovementDirection * unit * unitsPerSecond * Time.deltaTime;
            if (movementList.First.Value.MovementDirection == new Vector3(0,0,1))
            {
                if (updatedPosition.z >= movementList.First.Value.MovementDestination.z)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    movementList.RemoveFirst();
                    if (currentState == playerState.DrownWalk)
                    {
                        currentState = playerState.Drowning;
                        treatDrowningState();
                    }
                    else if (movementList.Count == 0)
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
                    if (currentState == playerState.DrownWalk)
                    {
                        currentState = playerState.Drowning;
                        treatDrowningState();
                    }
                    else if (movementList.Count == 0)
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
                    if (currentState == playerState.DrownWalk)
                    {
                        currentState = playerState.Drowning;
                        treatDrowningState();
                    }
                    else if (movementList.Count == 0)
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
                    if (currentState == playerState.DrownWalk)
                    {
                        currentState = playerState.Drowning;
                        treatDrowningState();
                    }
                    else if (movementList.Count == 0)
                    {
                        currentState = playerState.Idle;
                    }
                }
                else
                    transform.position = updatedPosition;
            }
        }
    }

    private void treatDrowningState()
    {
        currentState = playerState.Dead;
        levelManager.GetComponent<LevelManager>().treatPlayerDrowned();
    }
}
