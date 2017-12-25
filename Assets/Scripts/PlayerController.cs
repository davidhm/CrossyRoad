using UnityEngine;
using System;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour {

    private enum playerState { Moving, Idle, Dead,DrownWalk,Drowning,MovingToTrunk};
    private playerState currentState;
    public float playerSpeed;
    public GameObject levelManager;
    //private static float godModeSpeed = 160.0f;
    private Vector3 initialPosition;
    private bool debugIdle;
    private bool godMode;
    private class MovementObjective
    {
        private Vector3 movementDirection;
        private Vector3 movementDestination;
        private Quaternion targetOrientation;
        private Vector3 movementOrigin;
        private Quaternion originOrientation;

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

        public Quaternion TargetOrientation
        {
            get
            {
                return targetOrientation;
            }

            set
            {
                targetOrientation = value;
            }
        }

        public Vector3 MovementOrigin
        {
            get
            {
                return movementOrigin;
            }

            set
            {
                movementOrigin = value;
            }
        }

        public Quaternion OriginOrientation
        {
            get
            {
                return originOrientation;
            }

            set
            {
                originOrientation = value;
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
                Quaternion targetOrientation = Quaternion.identity;
                Quaternion originOrientation;
                Vector3 previousDestination;
                if (movementList.Count == 0)
                {
                    if (currentState != playerState.Idle)
                        throw new InvalidOperationException("The player state should be idle if the movement list is empty");
                    newDestination = transform.position + nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                    previousDestination = transform.position;
                    originOrientation = transform.rotation;
                }
                else
                {
                    newDestination = movementList.Last.Value.MovementDestination;
                    newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                    previousDestination = movementList.Last.Value.MovementDestination;
                    originOrientation = movementList.Last.Value.TargetOrientation;
                }
                nextObjective.MovementDestination = newDestination;
                nextObjective.TargetOrientation = targetOrientation;
                nextObjective.MovementOrigin = previousDestination;
                nextObjective.OriginOrientation = originOrientation;
                rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
                float targetHeight = levelManager.GetComponent<LevelManager>().getTargetPositionHeight(newDestination);
                nextObjective.MovementDestination = new Vector3(nextObjective.MovementDestination.x,
                    targetHeight,
                    nextObjective.MovementDestination.z);
                if (targetType == rowType.Water)
                {
                    if (levelManager.GetComponent<LevelManager>().checkIfTrunkInPosition(nextObjective.MovementDestination))
                    {
                        currentState = playerState.MovingToTrunk;
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
                Quaternion targetOrientation = Quaternion.Euler(0, 180, 0);
                Quaternion originOrientation;
                Vector3 previousDestination;
                if (movementList.Count == 0)
                {
                    if (currentState != playerState.Idle)
                        throw new InvalidOperationException("The player state should be idle if the movement list is empty");
                    newDestination = transform.position + nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                    previousDestination = transform.position;
                    originOrientation = transform.rotation;
                }
                else
                {
                    newDestination = movementList.Last.Value.MovementDestination;
                    newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                    previousDestination = movementList.Last.Value.MovementDestination;
                    originOrientation = movementList.Last.Value.TargetOrientation;
                }
                nextObjective.MovementDestination = newDestination;
                nextObjective.TargetOrientation = targetOrientation;
                nextObjective.MovementOrigin = previousDestination;
                nextObjective.OriginOrientation = originOrientation;
                rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
                if (targetType == rowType.Water)
                {
                    if (levelManager.GetComponent<LevelManager>().checkIfTrunkInPosition(nextObjective.MovementDestination))
                    {
                        currentState = playerState.MovingToTrunk;
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
                Quaternion targetOrientation = Quaternion.Euler(0, 90, 0);
                Quaternion originOrientation;
                Vector3 previousDestination;
                if (movementList.Count == 0)
                {
                    if (currentState != playerState.Idle)
                        throw new InvalidOperationException("The player state should be idle if the movement list is empty");
                    newDestination = transform.position + nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                    previousDestination = transform.position;
                    originOrientation = transform.rotation;
                }
                else
                {
                    newDestination = movementList.Last.Value.MovementDestination;
                    newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                    previousDestination = movementList.Last.Value.MovementDestination;
                    originOrientation = movementList.Last.Value.TargetOrientation;
                }
                nextObjective.MovementDestination = newDestination;
                nextObjective.TargetOrientation = targetOrientation;
                nextObjective.MovementOrigin = previousDestination;
                nextObjective.OriginOrientation = originOrientation;
                rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
                if (targetType == rowType.Water)
                {
                    if (levelManager.GetComponent<LevelManager>().checkIfTrunkInPosition(nextObjective.MovementDestination))
                    {
                        currentState = playerState.MovingToTrunk;
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
                Quaternion targetOrientation = Quaternion.Euler(0, -90, 0);
                Quaternion originOrientation;
                Vector3 previousDestination;
                if (movementList.Count == 0)
                {
                    if (currentState != playerState.Idle)
                        throw new InvalidOperationException("The player state should be idle if the movement list is empty");
                    newDestination = transform.position + nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                    previousDestination = transform.position;
                    originOrientation = transform.rotation;
                }
                else
                {
                    newDestination = movementList.Last.Value.MovementDestination;
                    newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                    previousDestination = movementList.Last.Value.MovementDestination;
                    originOrientation = movementList.Last.Value.TargetOrientation;
                }
                nextObjective.MovementDestination = newDestination;
                nextObjective.TargetOrientation = targetOrientation;
                nextObjective.MovementOrigin = previousDestination;
                nextObjective.OriginOrientation = originOrientation;
                rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
                if (targetType == rowType.Water)
                {
                    if (levelManager.GetComponent<LevelManager>().checkIfTrunkInPosition(nextObjective.MovementDestination))
                    {
                        currentState = playerState.MovingToTrunk;
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
        if (currentState == playerState.Moving || currentState == playerState.DrownWalk || currentState == playerState.MovingToTrunk)
        {
            Vector3 updatedPosition = transform.position + movementList.First.Value.MovementDirection * LevelGenerator.UnitCube.x * playerSpeed * Time.deltaTime;            
            if (movementList.First.Value.MovementDirection == new Vector3(0,0,1))
            {
                if (updatedPosition.z >= movementList.First.Value.MovementDestination.z)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    transform.rotation = movementList.First.Value.TargetOrientation;
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
                {
                    float distance = (transform.position.z - movementList.First.Value.MovementOrigin.z)/LevelGenerator.UnitCube.z;
                    float heightOffset = LevelGenerator.UnitCube.y * Mathf.Sin(Mathf.PI * distance);
                    transform.position = new Vector3(updatedPosition.x, 
                        levelManager.GetComponent<LevelManager>().InitialPlayerPosition.y + heightOffset, updatedPosition.z);
                    transform.rotation = Quaternion.Slerp(movementList.First.Value.OriginOrientation,
                        movementList.First.Value.TargetOrientation, distance);
                }
            }  
            else if (movementList.First.Value.MovementDirection == new Vector3(0,0,-1))
            {
                if (updatedPosition.z <= movementList.First.Value.MovementDestination.z)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    transform.rotation = movementList.First.Value.TargetOrientation;
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
                {
                    float distance = (movementList.First.Value.MovementOrigin.z - transform.position.z)/LevelGenerator.UnitCube.z;
                    float heightOffset = LevelGenerator.UnitCube.y * Mathf.Sin(Mathf.PI * distance);
                    transform.position = new Vector3(updatedPosition.x,
                        levelManager.GetComponent<LevelManager>().InitialPlayerPosition.y + heightOffset, updatedPosition.z);
                    transform.rotation = Quaternion.Slerp(movementList.First.Value.OriginOrientation,
                        movementList.First.Value.TargetOrientation, distance);
                }
            }
            else if (movementList.First.Value.MovementDirection == new Vector3(1, 0, 0))
            {
                if (updatedPosition.x >= movementList.First.Value.MovementDestination.x)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    transform.rotation = movementList.First.Value.TargetOrientation;
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
                {
                    float distance = (transform.position.x - movementList.First.Value.MovementOrigin.x)/LevelGenerator.UnitCube.z;
                    float heightOffset = LevelGenerator.UnitCube.y * Mathf.Sin(Mathf.PI * distance);
                    transform.position = new Vector3(updatedPosition.x,
                        levelManager.GetComponent<LevelManager>().InitialPlayerPosition.y + heightOffset, updatedPosition.z);
                    transform.rotation = Quaternion.Slerp(movementList.First.Value.OriginOrientation,
                        movementList.First.Value.TargetOrientation, distance);
                }
            }
            else if (movementList.First.Value.MovementDirection == new Vector3(-1, 0, 0))
            {
                if (updatedPosition.x <= movementList.First.Value.MovementDestination.x)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    transform.rotation = movementList.First.Value.TargetOrientation;
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
                {
                    float distance = (movementList.First.Value.MovementOrigin.x - transform.position.x)/LevelGenerator.UnitCube.z;
                    float heightOffset = LevelGenerator.UnitCube.y * Mathf.Sin(Mathf.PI * distance);
                    transform.position = new Vector3(updatedPosition.x,
                        levelManager.GetComponent<LevelManager>().InitialPlayerPosition.y + heightOffset, updatedPosition.z);
                    transform.rotation = Quaternion.Slerp(movementList.First.Value.OriginOrientation,
                        movementList.First.Value.TargetOrientation, distance);
                }
            }
        }
    }

    private void treatDrowningState()
    {
        currentState = playerState.Dead;
        levelManager.GetComponent<LevelManager>().treatPlayerDrowned();
    }
}
