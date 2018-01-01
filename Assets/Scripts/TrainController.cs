using UnityEngine;

public class TrainController : MonoBehaviour
{
    private Vector3 trainSpeed;
    private TrainRowManager manager;
    private bool incomingFromLeft;

    public Vector3 TrainSpeed
    {
        get
        {
            return trainSpeed;
        }

        set
        {
            trainSpeed = value;
        }
    }

    public TrainRowManager Manager
    {
        get
        {
            return manager;
        }

        set
        {
            manager = value;
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

    void Update()
    {
        transform.Translate(trainSpeed * Time.deltaTime);
        checkOutOfBounds();
    }

    private void checkOutOfBounds()
    {
        float offset = gameObject.GetComponent<Renderer>().bounds.extents.x;
        if (incomingFromLeft && transform.position.x - offset >
            Row.rightmostBorder + Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x ||
            !incomingFromLeft && transform.position.x + offset <
            Row.leftmostBorder - Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x) 
        {
            manager.GetComponent<TrainRowManager>().onTrainWagonDestroyed();
            Destroy(gameObject);
        }
    }
}



