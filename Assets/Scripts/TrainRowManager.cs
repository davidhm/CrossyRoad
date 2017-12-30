using System;
using UnityEngine;
public class TrainRowManager : MonoBehaviour
{
    public GameObject railPrefab, trainPrefab, railSignalPrefab;
    public Mesh trainWagon, trainLocomotive;
    public Mesh railSignalOn, railSignalOff;
    public float maxSecondsForTrain, minSecondsForTrain;
    private float trainTimer;
    private bool incomingFromLeft;
    private float roadHeight;
    private GameObject railSignal;
    private bool signalingTrain;
    private bool spawningTrain;
    private uint numberOfWagonsLeft;
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
        generateRails();
        generateRailwaySignal();
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
    }

    void Update()
    {
        trainTimer -= Time.deltaTime;
        generateEvents();
        catchEvents();        
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
        if (trainEnded())
        {
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
    }

    private void signalTrain()
    {

    }

    private void designalTrain()
    {

    }

    private void spawnTrain()
    {

    }

}

