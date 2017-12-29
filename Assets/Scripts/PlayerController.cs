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
    private bool godMode, playerMoved, willDrown, mustCheckTrunk, justDeletedMovement, inTrunk;
    private GameObject attachedTrunk;
    private bool[] playerPositionInTrunk;
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
        private targType originType;

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

        public targType OriginType
        {
            get
            {
                return originType;
            }

            set
            {
                originType = value;
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
        justDeletedMovement = true;
        inTrunk = false;
        playerPositionInTrunk = new bool[] { false, false, false };
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
                nextObjective.OriginType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(transform.position) == rowType.Water ?
                    MovementObjective.targType.Water : MovementObjective.targType.Rest;
            }
            else
            {
                newDestination = movementList.Last.Value.MovementDestination;
                newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                previousDestination = movementList.Last.Value.MovementDestination;
                originOrientation = movementList.Last.Value.TargetOrientation;
                nextObjective.OriginType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(previousDestination) == rowType.Water ?
                    MovementObjective.targType.Water : MovementObjective.targType.Rest;
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
            bool addedTarget = false;
            if (targetType == rowType.Water)
            {
                nextObjective.TargetType = MovementObjective.targType.Water;                
                addedTarget = true;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);                
            }
            else
            {
                addedTarget = true;
                movementList.AddLast(nextObjective);
            }
            if (addedTarget && currentType == rowType.Water)
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
                nextObjective.OriginType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(transform.position) == rowType.Water ?
                    MovementObjective.targType.Water : MovementObjective.targType.Rest;
            }
            else
            {
                newDestination = movementList.Last.Value.MovementDestination;
                newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                previousDestination = movementList.Last.Value.MovementDestination;
                originOrientation = movementList.Last.Value.TargetOrientation;
                nextObjective.OriginType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(previousDestination) == rowType.Water ?
                    MovementObjective.targType.Water : MovementObjective.targType.Rest;
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
            bool addedTarget = false;
            if (targetType == rowType.Water)
            {
                nextObjective.TargetType = MovementObjective.targType.Water;                
                addedTarget = true;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);                
            }
            else
            {
                addedTarget = true;
                movementList.AddLast(nextObjective);
            }
            if (addedTarget && currentType == rowType.Water)
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
                nextObjective.OriginType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(transform.position) == rowType.Water ?
                    MovementObjective.targType.Water : MovementObjective.targType.Rest;
            }
            else
            {
                newDestination = movementList.Last.Value.MovementDestination;
                newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                previousDestination = movementList.Last.Value.MovementDestination;
                originOrientation = movementList.Last.Value.TargetOrientation;
                nextObjective.OriginType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(previousDestination) == rowType.Water ?
                    MovementObjective.targType.Water : MovementObjective.targType.Rest;
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
            bool addedTarget = false;
            if (targetType == rowType.Water)
            {
                nextObjective.TargetType = MovementObjective.targType.Water;                
                addedTarget = true;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);
            }
            else
            {
                addedTarget = true;
                movementList.AddLast(nextObjective);
            }
            if (addedTarget && currentType == rowType.Water)
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
                nextObjective.OriginType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(transform.position) == rowType.Water ?
                    MovementObjective.targType.Water : MovementObjective.targType.Rest;
            }
            else
            {
                newDestination = movementList.Last.Value.MovementDestination;
                newDestination += nextObjective.MovementDirection * LevelGenerator.UnitCube.z;
                previousDestination = movementList.Last.Value.MovementDestination;
                originOrientation = movementList.Last.Value.TargetOrientation;
                nextObjective.OriginType = levelManager.GetComponent<LevelManager>().getRowTypeFromPosition(previousDestination) == rowType.Water ?
                    MovementObjective.targType.Water : MovementObjective.targType.Rest;
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
            bool addedTarget = false;
            if (targetType == rowType.Water)
            {
                nextObjective.TargetType = MovementObjective.targType.Water;
                addedTarget = true;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);                
            }
            else
            {
                addedTarget = true;
                movementList.AddLast(nextObjective);
            }
            if (addedTarget && currentType == rowType.Water)
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

    private bool checkWithinLimits(MovementObjective nextObjective)
    {
        GameObject trunk = levelManager.GetComponent<LevelManager>().getTrunkInPosition(nextObjective.MovementDestination);
        return trunk.GetComponent<TrunkController>().willFallOutOfLimits(1.0f/playerSpeed);
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

    private Vector3 getDiscretePosition(Vector3 continousPosition)
    {
        float discreteX = Mathf.Round((continousPosition.x - levelManager.GetComponent<LevelManager>().InitialPlayerPosition.x)/
            LevelGenerator.UnitCube.x);
        discreteX = discreteX * LevelGenerator.UnitCube.x + levelManager.GetComponent<LevelManager>().InitialPlayerPosition.x;
        float discreteZ = Mathf.Round((continousPosition.z - levelManager.GetComponent<LevelManager>().InitialPlayerPosition.z) /
            LevelGenerator.UnitCube.z);
        discreteZ = discreteZ * LevelGenerator.UnitCube.z + levelManager.GetComponent<LevelManager>().InitialPlayerPosition.z;
        return new Vector3(discreteX,continousPosition.y,discreteZ);
    }

    private void treatReachedTarget()
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
        if (movementList.First.Value.TargetType == MovementObjective.targType.Rest)
        {
            inTrunk = false;
            attachedTrunk = null;
        }
        movementList.RemoveFirst();
        justDeletedMovement = true;
        if (movementList.Count > 0)
        {
            if (movementList.First.Value.OriginType == MovementObjective.targType.Water)
            {
                transform.parent = null;
            }
        }
    }

    private void treatTargetNotReachedYet(Vector3 updatedPosition, MovementObjective.movType movementType)
    {
        justDeletedMovement = false;
        float distance = 0.0f;
        switch (movementType)
        {
            case MovementObjective.movType.Forwards:
                distance = (transform.position.z - movementList.First.Value.MovementOrigin.z) / LevelGenerator.UnitCube.z;
                break;
            case MovementObjective.movType.Backwards:
                distance = (movementList.First.Value.MovementOrigin.z - transform.position.z) / LevelGenerator.UnitCube.z;
                break;
            case MovementObjective.movType.LeftStrafe:
                distance = (movementList.First.Value.MovementOrigin.x - transform.position.x) / LevelGenerator.UnitCube.z;
                break;
            case MovementObjective.movType.RightStrafe:
                distance = (transform.position.x - movementList.First.Value.MovementOrigin.x) / LevelGenerator.UnitCube.z;
                break;
        }        
        float heightOffset = LevelGenerator.UnitCube.y * Mathf.Sin(Mathf.PI * distance);
        transform.position = new Vector3(updatedPosition.x,
            movementList.First.Value.MovementOrigin.y + heightOffset, updatedPosition.z);
        transform.rotation = Quaternion.Slerp(movementList.First.Value.OriginOrientation,
            movementList.First.Value.TargetOrientation, distance);
    }   

    private void treatMustCheckTrunk()
    {
        if (inTrunk)
        {
            treatAlreadyHaveTrunk();
        }
        else
        {
            treatHaveToFindTrunk();
        }
    }

    private void treatHaveToFindTrunk()
    {
        GameObject trunk = levelManager.GetComponent<LevelManager>().getTrunkInPosition(movementList.First.Value.MovementDestination);
        if (trunk == null)
            treatNoTrunkAvailable();
        else
            treatNewfoundTrunk(trunk);
    }

    private void treatAlreadyHaveTrunk()
    {
        TrunkController.TrunkSize currentSize = attachedTrunk.GetComponent<TrunkController>().TrunkSizeProperty;
        if (currentSize == TrunkController.TrunkSize.Small)
        {             
            treatHaveToFindTrunk();
        }
        else if (currentSize == TrunkController.TrunkSize.Medium)
        {
            if (playerPositionInTrunk[1] && movementList.First.Value.MovementType != MovementObjective.movType.RightStrafe ||
                playerPositionInTrunk[2] && movementList.First.Value.MovementType != MovementObjective.movType.LeftStrafe)
                treatHaveToFindTrunk();
            else if (playerPositionInTrunk[1] && movementList.First.Value.MovementType == MovementObjective.movType.RightStrafe)
            {
                playerPositionInTrunk[1] = false;
                playerPositionInTrunk[2] = true;
                findFuturePositionInTrunk();
            }
            else if (playerPositionInTrunk[2] && movementList.First.Value.MovementType == MovementObjective.movType.LeftStrafe)
            {
                playerPositionInTrunk[1] = true;
                playerPositionInTrunk[2] = false;
                findFuturePositionInTrunk();
            }            
        }
        else if (currentSize == TrunkController.TrunkSize.Large)
        {
            if (movementList.First.Value.MovementType == MovementObjective.movType.LeftStrafe)
            {
                if (playerPositionInTrunk[1])
                {
                    playerPositionInTrunk[1] = false;
                    playerPositionInTrunk[0] = true;
                    findFuturePositionInTrunk();
                }
                else if (playerPositionInTrunk[2])
                {
                    playerPositionInTrunk[2] = false;
                    playerPositionInTrunk[1] = true;
                    findFuturePositionInTrunk();
                }
                else
                {
                    treatHaveToFindTrunk();
                }
            }
            else if (movementList.First.Value.MovementType == MovementObjective.movType.RightStrafe)
            {
                if (playerPositionInTrunk[0])
                {
                    playerPositionInTrunk[0] = false;
                    playerPositionInTrunk[1] = true;
                    findFuturePositionInTrunk();
                }
                else if (playerPositionInTrunk[1])
                {
                    playerPositionInTrunk[1] = false;
                    playerPositionInTrunk[2] = true;
                    findFuturePositionInTrunk();
                }
                else
                {
                    treatHaveToFindTrunk();
                }
            }
        }
    }

    private float findFuturePositionInTrunk()
    {
        Vector3 continousPosition = movementList.First.Value.MovementDestination;
        float newPositionX = 
            Mathf.Round((continousPosition.x - attachedTrunk.transform.position.x) / LevelGenerator.UnitCube.x);
        newPositionX = newPositionX * LevelGenerator.UnitCube.x + attachedTrunk.transform.position.x;        
        if (attachedTrunk.GetComponent<TrunkController>().TrunkSizeProperty == TrunkController.TrunkSize.Medium)
        {
            if (newPositionX >= attachedTrunk.transform.position.x)
                newPositionX -= LevelGenerator.UnitCube.x / 2.0f ;
            else
            {
                newPositionX += LevelGenerator.UnitCube.x / 2.0f;
            }
        }
        float res = newPositionX;
        if (attachedTrunk.GetComponent<TrunkController>().IncomingFromLeft)
        {
            newPositionX += attachedTrunk.GetComponent<TrunkController>().SlowSpeed * (1 / playerSpeed);
        }
        else
        {
            newPositionX -= attachedTrunk.GetComponent<TrunkController>().SlowSpeed * (1 / playerSpeed);
        }
        Vector3 targetDestination = movementList.First.Value.MovementDestination;
        targetDestination = new Vector3(newPositionX, targetDestination.y, targetDestination.z);
        movementList.First.Value.MovementDirection = (targetDestination - movementList.First.Value.MovementOrigin).normalized;
        return res;
    }

    private void treatNoTrunkAvailable()
    {
        willDrown = true;
        inTrunk = false;
    }

    private void treatNewfoundTrunk(GameObject trunk)
    {
        attachedTrunk = trunk;
        inTrunk = true;
        treatJustFoundTrunk();                
    }

    private void treatJustFoundTrunk()
    {
        if (attachedTrunk.GetComponent<TrunkController>().TrunkSizeProperty == TrunkController.TrunkSize.Small)
        {
            playerPositionInTrunk[0] = false;
            playerPositionInTrunk[1] = true;
            playerPositionInTrunk[2] = false;
            findFuturePositionInTrunk(); 
        }
        else if (attachedTrunk.GetComponent<TrunkController>().TrunkSizeProperty == TrunkController.TrunkSize.Medium) {
            float position = findFuturePositionInTrunk();
            if (position < attachedTrunk.transform.position.x)
            {
                playerPositionInTrunk[0] = false;
                playerPositionInTrunk[1] = true;
                playerPositionInTrunk[2] = false;
            }
            else
            {
                playerPositionInTrunk[0] = false;
                playerPositionInTrunk[1] = false;
                playerPositionInTrunk[2] = true;
            }
        }
        else
        {
            float position = findFuturePositionInTrunk();
            if (position < attachedTrunk.transform.position.x)
            {
                playerPositionInTrunk[0] = true;
                playerPositionInTrunk[1] = false;
                playerPositionInTrunk[2] = false;
            }
            else if (position > attachedTrunk.transform.position.x)
            {
                playerPositionInTrunk[0] = false;
                playerPositionInTrunk[1] = false;
                playerPositionInTrunk[2] = true;
            }
            else
            {
                playerPositionInTrunk[0] = false;
                playerPositionInTrunk[1] = true;
                playerPositionInTrunk[2] = false;
            }
        }
    }

    private void treatCameBackFromWater()
    {
        MovementObjective nextObjective = movementList.First.Value;
        nextObjective.MovementDestination = getDiscretePosition(nextObjective.MovementDestination);
        nextObjective.MovementDirection = (nextObjective.MovementDestination - nextObjective.MovementOrigin).normalized;
    }

    private void updatePositionDependingOnMovementType(MovementObjective.movType type, Vector3 updatedPosition)
    {
        if (type == MovementObjective.movType.Forwards)
        {
            if (updatedPosition.z >= movementList.First.Value.MovementDestination.z)
            {
                treatReachedTarget();
            }
            else
            {
                treatTargetNotReachedYet(updatedPosition, type);
            }
        }
        else if (type == MovementObjective.movType.Backwards)
        {
            if (updatedPosition.z <= movementList.First.Value.MovementDestination.z)
            {
                treatReachedTarget();
            }
            else
            {
                treatTargetNotReachedYet(updatedPosition, type);
            }
        }
        else if (type == MovementObjective.movType.RightStrafe)
        {
            if (updatedPosition.x >= movementList.First.Value.MovementDestination.x)
            {
                treatReachedTarget();
            }
            else
            {
                treatTargetNotReachedYet(updatedPosition, type);
            }
        }
        else if (type == MovementObjective.movType.LeftStrafe)
        {
            if (updatedPosition.x <= movementList.First.Value.MovementDestination.x)
            {
                treatReachedTarget();
            }
            else
            {
                treatTargetNotReachedYet(updatedPosition, type);
            }
        }
    }

    private void updatePosition()
    {
        if (movementList.Count > 0)
        {
            MovementObjective nextObjective = movementList.First.Value;
            if (justDeletedMovement && nextObjective.TargetType == MovementObjective.targType.Water)
            {
                treatMustCheckTrunk();
            }
            if (justDeletedMovement &&
                nextObjective.TargetType == MovementObjective.targType.Rest && 
                nextObjective.OriginType == MovementObjective.targType.Water)
            {
                treatCameBackFromWater();                
            }
            Vector3 updatedPosition = transform.position + movementList.First.Value.MovementDirection * LevelGenerator.UnitCube.x * playerSpeed * Time.deltaTime;
            updatePositionDependingOnMovementType(movementList.First.Value.MovementType, updatedPosition);            
        }
    }

    private void treatDrowningState()
    {
        currentState = playerState.Dead;
        levelManager.GetComponent<LevelManager>().treatPlayerDrowned();
    }
}
