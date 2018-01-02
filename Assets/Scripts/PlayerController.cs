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
    private bool godMode, playerMoved, willDrown, mustCheckTrunk, justDeletedMovement;
    private float soundTimer, whenToPlay;
    private bool playPigSound, playTrunkSound;
    public AudioClip pigSound, trunkAttachment;
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
        justDeletedMovement = false;
        soundTimer = 4.0f + UnityEngine.Random.Range(0, 2);
        whenToPlay = UnityEngine.Random.value;
        playPigSound = false;
        playTrunkSound = false;
	}
	
	// Update is called once per frame
	void Update () {
        soundTimer -= Time.deltaTime;
        if (soundTimer <= 0)
        {
            playPigSound = true;
        }
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
            bool addedTarget = false;            
            if (targetType == rowType.Water)
            {               
                nextObjective.TargetType = MovementObjective.targType.Water;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);
                addedTarget = true;
            }
            else
            {
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                    addedTarget = true;
                }
            }
            if (addedTarget && currentType == rowType.Water)
            {
                transform.parent = null;
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
            bool addedTarget = false;
            if (targetType == rowType.Water)
            {
                nextObjective.TargetType = MovementObjective.targType.Water;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);
                addedTarget = true;
            }
            else
            {
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                    addedTarget = true;
                }
            }
            if (addedTarget && currentType == rowType.Water)
            {
                transform.parent = null;
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
            bool addedTarget = false;
            if (targetType == rowType.Water)
            {
                nextObjective.TargetType = MovementObjective.targType.Water;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);
                addedTarget = true;
            }
            else
            {
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                    addedTarget = true;
                }
            }
            if (addedTarget && currentType == rowType.Water)
            {
                transform.parent = null;
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
            bool addedTarget = false;
            if (targetType == rowType.Water)
            {
                nextObjective.TargetType = MovementObjective.targType.Water;
                mustCheckTrunk = true;
                movementList.AddLast(nextObjective);
                addedTarget = true;
            }
            else
            {
                if (levelManager.GetComponent<LevelManager>().checkPositionIsOccupable(nextObjective.MovementDestination))
                {
                    movementList.AddLast(nextObjective);
                    addedTarget = true;
                }
            }
            if (addedTarget && currentType == rowType.Water)
            {
                transform.parent = null;
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

    private Vector3 getDiscretePosition(Vector3 continousPosition)
    {
        float discreteX = Mathf.Round((continousPosition.x - levelManager.GetComponent<LevelManager>().InitialPlayerPosition.x) /
                LevelGenerator.UnitCube.x);
        discreteX = discreteX* LevelGenerator.UnitCube.x + levelManager.GetComponent<LevelManager>().InitialPlayerPosition.x; 
            float discreteZ = Mathf.Round((continousPosition.z - levelManager.GetComponent<LevelManager>().InitialPlayerPosition.z) /
                LevelGenerator.UnitCube.z);
        discreteZ = discreteZ* LevelGenerator.UnitCube.z + levelManager.GetComponent<LevelManager>().InitialPlayerPosition.z; 
            return new Vector3(discreteX, continousPosition.y, discreteZ);
    }

    private void treatSound(float arcCompleted)
    {
        if (playTrunkSound || playPigSound && arcCompleted >= whenToPlay)
        {
            soundTimer = 4.0f + UnityEngine.Random.Range(0, 2);
            whenToPlay = UnityEngine.Random.value;
            playPigSound = false;
            if (!playTrunkSound)
            {
                gameObject.GetComponent<AudioSource>().clip = pigSound;
                gameObject.GetComponent<AudioSource>().volume = 0.1f;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    private void playTrunkAttachment()
    {
        gameObject.GetComponent<AudioSource>().clip = trunkAttachment;
        gameObject.GetComponent<AudioSource>().volume = 1;
        gameObject.GetComponent <AudioSource>().Play();
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
                playTrunkSound = true;
            }
            else if (mustCheckTrunk && nextObjective.TargetType == MovementObjective.targType.Water)
            {
                willDrown = true;
                mustCheckTrunk = false;
            }
            if (justDeletedMovement && movementList.First.Value.TargetType == MovementObjective.targType.Rest)
            {
                justDeletedMovement = false;
                movementList.First.Value.MovementDestination = getDiscretePosition(movementList.First.Value.MovementDestination);
                movementList.First.Value.MovementDirection = (movementList.First.Value.MovementDestination - movementList.First.Value.MovementOrigin).normalized;
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
                        playTrunkAttachment();
                    }                   
                    else if (willDrown)
                    {
                        currentState = playerState.Dead;
                        treatDrowningState();
                    }
                    movementList.RemoveFirst();                    
                    justDeletedMovement = true;
                }
                else
                {
                    float distance = (transform.position.z - movementList.First.Value.MovementOrigin.z)/LevelGenerator.UnitCube.z;
                    treatSound(distance);
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
                        playTrunkAttachment();
                    }
                    else if (willDrown)
                    {
                        currentState = playerState.Dead;
                        treatDrowningState();
                    }           
                    movementList.RemoveFirst();
                    justDeletedMovement = true;
                }
                else
                {
                    float distance = (movementList.First.Value.MovementOrigin.z - transform.position.z)/LevelGenerator.UnitCube.z;
                    treatSound(distance);
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
                        playTrunkAttachment();
                    }
                    else if (willDrown)
                    {
                        currentState = playerState.Dead;
                        treatDrowningState();
                    }
                    movementList.RemoveFirst();
                    justDeletedMovement = true;
                }
                else
                {
                    float distance = (transform.position.x - movementList.First.Value.MovementOrigin.x)/LevelGenerator.UnitCube.z;
                    treatSound(distance);
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
                        playTrunkAttachment();
                    }
                    else if (willDrown)
                    {
                        currentState = playerState.Dead;
                        treatDrowningState();
                    }
                    movementList.RemoveFirst();
                    justDeletedMovement = true;
                }
                else
                {
                    float distance = (movementList.First.Value.MovementOrigin.x - transform.position.x)/LevelGenerator.UnitCube.z;
                    treatSound(distance);
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
