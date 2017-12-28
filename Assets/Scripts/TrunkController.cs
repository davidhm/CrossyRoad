using System;
using UnityEngine;
public class TrunkController : MonoBehaviour
{
    public enum TrunkSize {Small, Medium, Large}
    private TrunkSize trunkSize;
    private bool overflowedRow;
    private float slowSpeed, currentSpeed;
    private static float fastSpeed;
    private bool justSpawned;
    private bool incomingFromLeft;
    public static float FastSpeed
    {
        get
        {
            return fastSpeed;
        }

        set
        {
            fastSpeed = value;
        }
    }

    public bool JustSpawned
    {
        get
        {
            return justSpawned;
        }

        set
        {
            justSpawned = value;
        }
    }

    public bool IncomingFromLeft
    {
        get
        {
            return incomingFromLeft;
        }

        set
        {
            incomingFromLeft = value;
        }
    }

    public float SlowSpeed
    {
        get
        {
            return slowSpeed;
        }

        set
        {
            slowSpeed = value;
        }
    }

    public float CurrentSpeed
    {
        get
        {
            return currentSpeed;
        }
    }

    public TrunkSize TrunkSizeProperty
    {
        get
        {
            return trunkSize;
        }

        set
        {
            trunkSize = value;
        }
    }

    void Update()
    {
        currentSpeed = getCorrectSpeed();
        Vector3 currentSpeedVector = new Vector3(currentSpeed, 0.0f, 0.0f);
        if (!incomingFromLeft)
            currentSpeedVector = -currentSpeedVector;
        transform.Translate(currentSpeedVector * Time.deltaTime);
        checkRowOverflow();
    }

    private float getCorrectSpeed()
    {
        float lateralWidth = gameObject.GetComponent<Renderer>().bounds.extents.x;
        if (transform.position.x + lateralWidth < Row.rightmostBorder &&
            transform.position.x - lateralWidth > Row.leftmostBorder)
            return slowSpeed;
        return fastSpeed;
    }

    private void checkRowOverflow()
    {
        if (justSpawned)
        {
            if (incomingFromLeft && transform.position.x > Row.leftmostBorder)
                justSpawned = false;
            else if (!incomingFromLeft && transform.position.x < Row.rightmostBorder)
                justSpawned = false;
        }
        else
        {
            float offset = gameObject.GetComponent<Renderer>().bounds.extents.x;
            if ((incomingFromLeft && transform.position.x >
                Row.rightmostBorder + Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x + offset) ||
                (!incomingFromLeft && transform.position.x <
                Row.leftmostBorder - Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x - offset))
            {
                transform.parent.gameObject.GetComponent<Row>().notifyTrunkDestroyed(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public bool willFallOutOfLimits(float timeToCollision)
    {
        float offset = gameObject.GetComponent<Renderer>().bounds.extents.x;
        return incomingFromLeft ? (transform.position.x + offset + slowSpeed * timeToCollision)
            > Row.rightmostBorder : (transform.position.x - offset - slowSpeed * timeToCollision)
            < Row.leftmostBorder;
    }
}
