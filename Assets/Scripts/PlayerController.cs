using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private bool moving;
    private Vector3 newPosition;
    private Vector3 movingDirection;
    public float unitsPerSecond;
	// Use this for initialization
	void Start () {
        moving = false;
        newPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        processInput();
        updatePosition();
	}

    public bool isMoving()
    {
        return moving;
    }

    public Vector3 getNewPosition()
    {
        return newPosition;
    }

    private void processInput() {
        if (!moving && Input.GetKeyDown(KeyCode.UpArrow))
        {
            ++newPosition.z;
            moving = true;
            movingDirection = new Vector3(0, 0, 1);  
        }
        else if (!moving && Input.GetKeyDown(KeyCode.DownArrow))
        {
            --newPosition.z;
            moving = true;
            movingDirection = new Vector3(0, 0, -1);
        }
    }

    private void updatePosition()
    {
        if (moving)
        {
            Vector3 updatedPosition = transform.position + movingDirection * unitsPerSecond * Time.deltaTime;
            if (movingDirection == new Vector3(0,0,1))
            {
                if (updatedPosition.z >= newPosition.z)
                {
                    transform.position = newPosition;
                    moving = false;
                }
                else
                    transform.position = updatedPosition;
            }  
            else if (movingDirection == new Vector3(0,0,-1))
            {
                if (updatedPosition.z <= newPosition.z)
                {
                    transform.position = newPosition;
                    moving = false;
                }
                else
                    transform.position = updatedPosition;
            }          
        }
    }
}
