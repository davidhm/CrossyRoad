using UnityEngine;
public class TrainRowManager : MonoBehaviour
{
    public GameObject railPrefab, trainPrefab, railSignalPrefab;
    public GameObject roadPrefab;
    private GameObject locomotive;
    public AudioClip trainPassingByLeft, trainPassingByRight;
    public Mesh trainWagon, trainLocomotive;
    public Mesh railSignalOn, railSignalOff;
    public float maxSecondsForTrain, minSecondsForTrain;
    public float trainSpeed;
    private float trainTimer;
    private bool incomingFromLeft;
    private float roadHeight;
    private GameObject railSignal;
    private bool signalingTrain;
    private bool spawningTrain;
    private uint numberOfWagonsLeft;
    private bool justDespawned;
    public float RoadHeight
    {
        get
        {
            return roadHeight;
        }

        set
        {
            roadHeight = value;
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

    private void generateRandomTimer()
    {
        trainTimer = UnityEngine.Random.Range(minSecondsForTrain, maxSecondsForTrain);
    }

    public bool isRailRoadVisible()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).GetComponent<Renderer>().isVisible)
                return true;
        }
        return false;
    }

    public void generateInitialElements()
    {
        generateRandomTimer();
        generateRoadSlabs();
        generateRails();
        generateRailwaySignal();
    }

    private void generateRoadSlabs()
    {
        float halfCube = LevelGenerator.UnitCube.x / 2.0f;
        for (float j = Row.leftmostBorder - Row.rowMarginInUnitCubes * LevelGenerator. UnitCube.x + halfCube;
            j <= Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x + Row.rightmostBorder - halfCube; j += LevelGenerator.UnitCube.x)
        {
            GameObject roadSlab = (GameObject)Instantiate(roadPrefab, transform);            
            roadSlab.transform.position = new Vector3(j, 0.0f,
                transform.position.z);
        }
    }

    private void generateRails()
    {
        float halfCube = LevelGenerator.UnitCube.x / 2.0f;
        float wholeCube = LevelGenerator.UnitCube.x;
        for (float i = Row.leftmostBorder - Row.rowMarginInUnitCubes*wholeCube + halfCube;
            i <= Row.rightmostBorder + Row.rowMarginInUnitCubes*wholeCube - halfCube;
            i += wholeCube)
        {
            GameObject railInstance = Instantiate(railPrefab, transform);
            railInstance.transform.position = new Vector3(i, roadHeight, transform.position.z);
        }
    }

    private void generateRailwaySignal()
    {
        railSignal = Instantiate(railSignalPrefab, transform);
        float halfCube = LevelGenerator.UnitCube.x / 2.0f;
        float wholeCube = LevelGenerator.UnitCube.x;
        float lateralPosition = incomingFromLeft ? 
            Row.leftmostBorder + wholeCube + halfCube : 
            Row.rightmostBorder - wholeCube - halfCube;
        float depthPosition = transform.position.z - halfCube;
        railSignal.transform.position = new Vector3(lateralPosition,
            roadHeight, depthPosition);
    }

    void Start()
    {
        signalingTrain = false;
        spawningTrain = false;
        numberOfWagonsLeft = 0;
        justDespawned = false;
    }

    void Update()
    {       
        trainTimer -= Time.deltaTime;
        generateEvents();
        catchEvents();
        playTrainPassingBy();                
    }

    private void playTrainPassingBy()
    {
        if (locomotive != null && 
            (incomingFromLeft && locomotive.transform.position.x >
            (Row.rightmostBorder - Row.leftmostBorder)/2.0f ||
            !incomingFromLeft && locomotive.transform.position.x <
            (Row.rightmostBorder - Row.leftmostBorder) / 2.0f))
        {
            if (incomingFromLeft)
            {
                gameObject.GetComponent<AudioSource>().clip = trainPassingByLeft;
                gameObject.GetComponent<AudioSource>().Play();
            }
            else
            {
                gameObject.GetComponent<AudioSource>().clip = trainPassingByRight;
                gameObject.GetComponent<AudioSource>().Play();
            }
            locomotive = null;
        }
    }

    private void generateEvents()
    {
        if (!signalingTrain && trainTimer <= 2.0f)
        {
            signalingTrain = true;
            signalTrain();
        }
        if (!spawningTrain && trainTimer <= 0)
        {
            spawningTrain = true;
            spawnTrain();
        }
    }

    private void catchEvents()
    {
        if (justDespawned)
        {
            justDespawned = false;
            if (signalingTrain)
            {
                signalingTrain = false;
                designalTrain();
            }
            if (spawningTrain)
            {
                spawningTrain = false;
            }
            generateRandomTimer();
        }
    }

    private bool trainEnded()
    {
        return numberOfWagonsLeft == 0;
    }

    public void onTrainWagonDestroyed()
    {
        --numberOfWagonsLeft;
        if (numberOfWagonsLeft == 0)
            justDespawned = true;
    }

    private void signalTrain()
    {
        railSignal.GetComponent<MeshFilter>().mesh = railSignalOn;
        reproduceWarningSound();
    }

    private void reproduceWarningSound()
    {
        railSignal.GetComponent<AudioSource>().Play();
    }

    private void designalTrain()
    {
        railSignal.GetComponent<MeshFilter>().mesh = railSignalOff;
    }

    private void spawnTrain()
    {
        GameObject wagonInstance = Instantiate(trainPrefab, transform);
        wagonInstance.GetComponent<MeshFilter>().mesh = trainLocomotive;
        float locomotiveWidth = wagonInstance.GetComponent<Renderer>().bounds.extents.x;
        float spawningPoint = incomingFromLeft ?
            Row.leftmostBorder - Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x - locomotiveWidth :
            Row.rightmostBorder + Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x + locomotiveWidth;
        wagonInstance.GetComponent<MeshFilter>().mesh = trainWagon;
        float wagonWidth = wagonInstance.GetComponent<Renderer>().bounds.extents.x;
        int numberOfWagons = 10;
        float i = spawningPoint;
        float railHeight = railPrefab.GetComponent<Renderer>().bounds.size.y;
        Destroy(wagonInstance);
        while (incomingFromLeft && i > spawningPoint - numberOfWagons * 2 * wagonWidth ||
               !incomingFromLeft && i < spawningPoint + numberOfWagons*2*wagonWidth)
        {
            wagonInstance = Instantiate(trainPrefab, transform);
            wagonInstance.transform.position = new Vector3(i, roadHeight + railHeight,
                transform.position.z);
            if (i == spawningPoint)
            {
                wagonInstance.GetComponent<MeshFilter>().mesh = trainLocomotive;
                locomotive = wagonInstance;
                i = incomingFromLeft ? i - locomotiveWidth - wagonWidth :
                    i + locomotiveWidth + wagonWidth;                
            }
            else
            {
                i = incomingFromLeft ? i - 2 * wagonWidth : i + 2 * wagonWidth;
            }
            if (!incomingFromLeft)
            {
                wagonInstance.transform.Rotate(new Vector3(0, 180, 0));
            }
            wagonInstance.GetComponent<TrainController>().Manager = this;
            wagonInstance.GetComponent<TrainController>().TrainSpeed = new Vector3(trainSpeed, 0, 0);
            wagonInstance.GetComponent<TrainController>().IncomingFromLeft = incomingFromLeft;
        }
        numberOfWagonsLeft = (uint)numberOfWagons;
    }

}

