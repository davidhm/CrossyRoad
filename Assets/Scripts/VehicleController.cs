using UnityEngine;

public class VehicleController : MonoBehaviour {

    private bool overflowedRow;
    private Vector3 speed;
    private bool justSpawned;
    private bool incomingFromLeft;

    public Vector3 Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
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

    // Update is called once per frame
    void Update () {
        transform.Translate(speed * Time.deltaTime);
        checkRowOverflow();
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
                Row.rightmostBorder + Row.rowMarginInUnitCubes*LevelGenerator.UnitCube.x + offset) ||
                (!incomingFromLeft && transform.position.x <
                Row.leftmostBorder - Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x - offset))
            {
                Destroy(gameObject);
            }
        }
    }
}
