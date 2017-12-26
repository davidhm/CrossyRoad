using UnityEngine;
using System;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour {

    private enum playerState { Moving, Idle, Dead};
    private playerState currentState;
    public float playerSpeed;
    public GameObject levelManager;
    //private static float godModeSpeed = 160.0f;
    private Vector3 initialPosition;
    private bool godMode, playerMoved, willDrown, mustCheckTrunk;
    private class MovementObjective
    {
        public enum movType { Forwards, Backwards, LeftStrafe, RightStrafe}
        public enum targType { Water, Rest}
        private Vector3 movementDirection;        
        private Vector3 movementDestination;
        private Quaternion targetOrientation;
        private Vector3 movementOrigin;
        private Quaternion originOrientation;
        private movType movementType;
        private targType targetType;

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

        public movType MovementType
        {
            get
            {
                return movementType;
            }

            set
            {
                movementType = value;
            }
        }

        public targType TargetType
        {
            get
            {
                return targetType;
            }

            set
            {
                targetType = value;
            }
        }
    }

    private LinkedList<MovementObjective> movementList;

    public bool PlayerMoved
    {
        get
        {
            return playerMoved;
        }
    }

    // Use this for initialization
    void Start () {
        currentState = playerState.Idle;
        initialPosition = transform.position;
        movementList = new LinkedList<MovementObjective>();        
        godMode = false;
        playerMoved = false;
        willDrown = false;
        mustCheckTrunk = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (currentState != playerState.Dead)
        {
            processInput();
            updatePosition();
        }
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
        if (levelManager != null)
        levelManager.GetComponent<LevelManager>().treatPlayerInvisible();
    }

    private void processInput() {        
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
            nextObjective.MovementType = MovementObjective.movType.Forwards;
            nextObjective.TargetType = MovementObjective.targType.Rest;
            rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
            rowType currentType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(transform.position);
            float targetHeight = levelManager.GetComponent<LevelManager>().getTargetPositionHeight(newDestination);
            nextObjective.MovementDestination = new Vector3(nextObjective.MovementDestination.x,
                targetHeight,
                nextObjective.MovementDestination.z);
            if (currentType == rowType.Water)
            {
                transform.parent = null;
            }
            if (targetType == rowType.Water)
            {               
                nextObjective.TargetType = MovementObjective.targType.Water;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);
            }
            else
            {
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                }
            }
            playerMoved = true;
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
            nextObjective.MovementType = MovementObjective.movType.Backwards;
            nextObjective.TargetType = MovementObjective.targType.Rest;
            rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
            rowType currentType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(transform.position);
            float targetHeight = levelManager.GetComponent<LevelManager>().getTargetPositionHeight(newDestination);
            nextObjective.MovementDestination = new Vector3(nextObjective.MovementDestination.x,
                targetHeight,
                nextObjective.MovementDestination.z);
            if (currentType == rowType.Water)
            {
                transform.parent = null;
            }
            if (targetType == rowType.Water)
            {
                nextObjective.TargetType = MovementObjective.targType.Water;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);
            }
            else
            {
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                }
            }
            playerMoved = true;
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
            nextObjective.MovementType = MovementObjective.movType.RightStrafe;
            nextObjective.TargetType = MovementObjective.targType.Rest;
            rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
            rowType currentType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(transform.position);
            float targetHeight = levelManager.GetComponent<LevelManager>().getTargetPositionHeight(newDestination);
            nextObjective.MovementDestination = new Vector3(nextObjective.MovementDestination.x,
                targetHeight,
                nextObjective.MovementDestination.z);
            if (targetType == rowType.Water)
            {
                nextObjective.TargetType = MovementObjective.targType.Water;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);
            }
            if (currentType == rowType.Water)
            {
                transform.parent = null;
            }
            else
            {
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                }
            }
            playerMoved = true;
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
            nextObjective.MovementType = MovementObjective.movType.LeftStrafe;
            nextObjective.TargetType = MovementObjective.targType.Rest;
            rowType targetType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(nextObjective.MovementDestination);
            rowType currentType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(transform.position);
            float targetHeight = levelManager.GetComponent<LevelManager>().getTargetPositionHeight(newDestination);
            nextObjective.MovementDestination = new Vector3(nextObjective.MovementDestination.x,
                targetHeight,
                nextObjective.MovementDestination.z);
            if (targetType == rowType.Water)
            {
                nextObjective.TargetType = MovementObjective.targType.Water;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);
            }
            if (currentType == rowType.Water)
            {
                transform.parent = null;
            }
            else
            {
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                }
            }
            playerMoved = true;
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = true;
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
        if (movementList.Count > 0)
        {
            MovementObjective nextObjective = movementList.First.Value;
            if (mustCheckTrunk && nextObjective.TargetType == MovementObjective.targType.Water &&
                levelManager.GetComponent<LevelManager>().checkIfTrunkInPosition(nextObjective.MovementDestination))
            {
                Vector3 futurePosition = levelManager.GetComponent<LevelManager>().getFutureTrunkPosition(nextObjective.MovementDestination,
                    1.0f / playerSpeed);
                nextObjective.MovementDestination = futurePosition;
                nextObjective.MovementDirection = (futurePosition - transform.position).normalized;
                mustCheckTrunk = false;
            }
            else if (mustCheckTrunk && nextObjective.TargetType == MovementObjective.targType.Water)
            {
                willDrown = true;
                mustCheckTrunk = false;
            }
            Vector3 updatedPosition = transform.position + movementList.First.Value.MovementDirection * LevelGenerator.UnitCube.x * playerSpeed * Time.deltaTime;
            if (movementList.First.Value.MovementType == MovementObjective.movType.Forwards)
            {
                if (updatedPosition.z >= movementList.First.Value.MovementDestination.z)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    transform.rotation = movementList.First.Value.TargetOrientation; 
                    if (movementList.First.Value.TargetType == MovementObjective.targType.Water && !willDrown)
                    {
                        levelManager.GetComponent<LevelManager>().attachPlayerToTrunk(gameObject);
                    }                   
                    else if (willDrown)
                    {
                        currentState = playerState.Dead;
                        treatDrowningState();
                    }
                    movementList.RemoveFirst();
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
            else if (movementList.First.Value.MovementType == MovementObjective.movType.Backwards)
            {
                if (updatedPosition.z <= movementList.First.Value.MovementDestination.z)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    transform.rotation = movementList.First.Value.TargetOrientation;
                    if (movementList.First.Value.TargetType == MovementObjective.targType.Water && !willDrown)
                    {
                        levelManager.GetComponent<LevelManager>().attachPlayerToTrunk(gameObject);
                    }
                    else if (willDrown)
                    {
                        currentState = playerState.Dead;
                        treatDrowningState();
                    }           
                    movementList.RemoveFirst();
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
            else if (movementList.First.Value.MovementType == MovementObjective.movType.RightStrafe)
            {
                if (updatedPosition.x >= movementList.First.Value.MovementDestination.x)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    transform.rotation = movementList.First.Value.TargetOrientation;
                    if (movementList.First.Value.TargetType == MovementObjective.targType.Water && !willDrown)
                    {
                        levelManager.GetComponent<LevelManager>().attachPlayerToTrunk(gameObject);
                    }
                    else if (willDrown)
                    {
                        currentState = playerState.Dead;
                        treatDrowningState();
                    }
                    movementList.RemoveFirst();
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
            else if (movementList.First.Value.MovementType == MovementObjective.movType.LeftStrafe)
            {
                if (updatedPosition.x <= movementList.First.Value.MovementDestination.x)
                {
                    transform.position = movementList.First.Value.MovementDestination;
                    transform.rotation = movementList.First.Value.TargetOrientation;
                    if (movementList.First.Value.TargetType == MovementObjective.targType.Water && !willDrown)
                    {
                        levelManager.GetComponent<LevelManager>().attachPlayerToTrunk(gameObject);
                    }
                    else if (willDrown)
                    {
                        currentState = playerState.Dead;
                        treatDrowningState();
                    }
                    movementList.RemoveFirst();
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
