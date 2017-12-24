﻿using System.Collections.Generic;
using UnityEngine;
public enum rowType { Grass, Road, Water}

public class CollisionInfo
{
    public float collisionPoint, maxCollisionPossible;
}

public class Row : MonoBehaviour
{
    public GameObject carPrefab, treePrefab, grassPrefab,roadPrefab,boulderPrefab;
    public GameObject truckPrefab;
    public GameObject waterPrefab, trunkPrefab;
    public Mesh redCarMesh, blueCarMesh, greenCarMesh;
    public Mesh redTruckMesh, blueTruckMesh, greenTruckMesh;
    public Mesh darkGrassMesh, clearGrassMesh;
    public Mesh smallTrunkMesh, mediumTrunkMesh, largeTrunkMesh;
    public Mesh darkWaterMesh, clearWaterMesh;
    private Mesh stripedRoadMesh;
    public static float leftmostBorder;
    public static uint rowWidthInUnitCubes;
    public static float rightmostBorder;
    public static uint rowMarginInUnitCubes;
    private rowType currentType;
    private static Vector3 unitCube;
    private static float halfCube;
    private bool incomingFromLeft;
    private float truckProportion;
    private static float vehicleMaxSpeed,vehicleMinSpeed;
    private float treeProportion;
    private float trunkTimer, trunkSlowSpeed;
    private List<bool> occupableRow;
    private LinkedList<GameObject> trunksInWater;

    public static float VehicleMaxSpeed
    {
        get
        {
            return vehicleMaxSpeed;
        }

        set
        {
            vehicleMaxSpeed = value;
        }
    }

    public static float VehicleMinSpeed
    {
        get
        {
            return vehicleMinSpeed;
        }

        set
        {
            vehicleMinSpeed = value;
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

    public float TruckProportion
    {
        get
        {
            return truckProportion;
        }

        set
        {
            truckProportion = value;
        }
    }

    public rowType CurrentType
    {
        get
        {
            return currentType;
        }

        set
        {
            currentType = value;
        }
    }

    public float TreeProportion
    {
        get
        {
            return treeProportion;
        }

        set
        {
            treeProportion = value;
        }
    }

    public Mesh StripedRoadMesh
    {
        get
        {
            return stripedRoadMesh;
        }

        set
        {
            stripedRoadMesh = value;
        }
    }

    public static void setUnitCube(Vector3 unitCube)
    {
        Row.unitCube = unitCube;
        halfCube = unitCube.z / 2.0f;
    }

    void Start()
    {
        TrunkController.FastSpeed = 320.0f;
        trunksInWater = new LinkedList<GameObject>();
    }
    void LateUpdate() {
        if (currentType == rowType.Road)
        {            
            LinkedList<CollisionInfo> collisions = new LinkedList<CollisionInfo>();
            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform currentVehicle = transform.GetChild(i);
                if (transform.GetChild(i).name == "Vehicle")
                {
                    float initialPosition;
                    float offset = currentVehicle.gameObject.GetComponent<Renderer>().bounds.extents.x;
                    if (incomingFromLeft)
                    {
                        initialPosition = currentVehicle.position.x - (leftmostBorder - rowMarginInUnitCubes * unitCube.x);
                        initialPosition -= offset;
                    }
                    else
                    {
                        initialPosition = currentVehicle.position.x - (rightmostBorder + rowMarginInUnitCubes * unitCube.x);
                        initialPosition += offset;
                    }
                    if (initialPosition > 0)
                    {
                        float maxAllowedCollision = (rightmostBorder - leftmostBorder) + 2 * rowMarginInUnitCubes * unitCube.x;
                        float vehicleSpeed = Mathf.Abs(currentVehicle.gameObject.GetComponent<VehicleController>().Speed.x);
                        float timeForCollision = initialPosition / (vehicleMaxSpeed - vehicleSpeed);
                        if (vehicleSpeed*timeForCollision < maxAllowedCollision)
                            return;                            
                        else
                        {
                            CollisionInfo current = new CollisionInfo();
                            current.collisionPoint = vehicleSpeed * timeForCollision;
                            current.maxCollisionPossible = rightmostBorder + rowMarginInUnitCubes * unitCube.x;
                            current.maxCollisionPossible -= leftmostBorder - rowMarginInUnitCubes * unitCube.x;
                            collisions.AddLast(current);
                        }
                    }
                    else
                        return;                    
                }
            }
            generateOneVehicle(collisions);
            
        }
        else if (currentType == rowType.Water)
        {
            trunkTimer -= Time.deltaTime;
            if (trunkTimer <= 0) {
                trunkTimer = 2.5f + Random.Range(0.0f, 0.5f);
                generateOneTrunk();
            }
        }
    }

    public List<bool> getOccupableRow()
    {
        return occupableRow;
    }

    public void generateInitialElements()
    {
        occupableRow = new List<bool>(9);
        for (int i = 0; i < 9; ++i)
            occupableRow.Add(true);
        if (currentType == rowType.Road)
        {
            generateRoadRow();
        }
        else if (currentType == rowType.Grass)
        {
            generateGrassRow();
        }
        else if (currentType == rowType.Water)
        {
            generateWaterRow();
        }
    }

    private void generateWaterRow()
    {
        trunkTimer = 2.5f + Random.Range(0, 0.5f);
        incomingFromLeft = Random.value > 0.5;
        trunkSlowSpeed = 40.0f + Random.Range(0.0f, 80.0f);        
        for (float i = leftmostBorder - rowMarginInUnitCubes*unitCube.x + halfCube;
            i <= rightmostBorder + rowMarginInUnitCubes * unitCube.x - halfCube;
            i += unitCube.x)
        {
            GameObject waterInstance = (GameObject)Instantiate(waterPrefab, transform);
            if (i < leftmostBorder || i > rightmostBorder)
            {
                waterInstance.GetComponent<MeshFilter>().mesh = darkWaterMesh;
            }
            float waterHeight = -waterPrefab.GetComponent<Renderer>().bounds.extents.y;
            waterInstance.transform.position = new Vector3(i, waterHeight, transform.position.z);
        }
        generateOneTrunk();
    }

    private void generateOneTrunk()
    {
        GameObject trunkInstance = (GameObject)Instantiate(trunkPrefab, transform);
        trunkInstance.GetComponent<TrunkController>().SlowSpeed = trunkSlowSpeed;
        trunkInstance.GetComponent<TrunkController>().IncomingFromLeft = incomingFromLeft;
        float lateralPosition;
        if (incomingFromLeft)
        {
            lateralPosition = leftmostBorder - rowMarginInUnitCubes * unitCube.x;
            lateralPosition -= trunkInstance.GetComponent<Renderer>().bounds.extents.x;
        }
        else
        {
            lateralPosition = rightmostBorder + rowMarginInUnitCubes * unitCube.x;
            lateralPosition += trunkInstance.GetComponent<Renderer>().bounds.extents.x;
        }
        trunkInstance.transform.position = new Vector3(lateralPosition, 0.0f, transform.position.z);
        trunkInstance.GetComponent<TrunkController>().JustSpawned = true;
    }

    private void generateGrassRow()
    {
        int k = 0;
        for (float i = leftmostBorder - rowMarginInUnitCubes*unitCube.x + halfCube;
            i <= rightmostBorder + rowMarginInUnitCubes*unitCube.x - halfCube;
            i += unitCube.x)
        {          
            GameObject grassSlab = (GameObject)Instantiate(grassPrefab, transform);
            float grassHeight = grassPrefab.GetComponent<Renderer>().bounds.extents.y;
            grassSlab.transform.position = new Vector3(i, grassHeight, transform.position.z);
            if (i < leftmostBorder || i > rightmostBorder)
            {
                GameObject tree = (GameObject)Instantiate(treePrefab, transform);
                float treeHeight = grassPrefab.GetComponent<Renderer>().bounds.size.y;
                tree.transform.position = new Vector3(i, treeHeight, transform.position.z);
            }
            else if (UnityEngine.Random.value < treeProportion) 
            {
                if (UnityEngine.Random.value > 0.5)
                {
                    GameObject tree = (GameObject)Instantiate(treePrefab, transform);
                    float treeHeight = grassPrefab.GetComponent<Renderer>().bounds.size.y;
                    tree.transform.position = new Vector3(i, treeHeight, transform.position.z);                    
                }
                else
                {
                    GameObject boulder = (GameObject)Instantiate(boulderPrefab, transform);
                    float boulderHeight = grassPrefab.GetComponent<Renderer>().bounds.size.y;
                    boulder.transform.position = new Vector3(i, boulderHeight, transform.position.z);
                }
                if (k >= 0 && k <= 9)
                    occupableRow[k] = false;
            }
            if (i >= leftmostBorder + halfCube && i < rightmostBorder - halfCube)
                ++k;
        }
    }

    private void generateRoadRow()
    {
        for (float j = leftmostBorder - rowMarginInUnitCubes*unitCube.x + halfCube;
            j <= rowMarginInUnitCubes*unitCube.x + rightmostBorder - halfCube; j += unitCube.x)
        {
            GameObject roadSlab = (GameObject) Instantiate(roadPrefab, transform);
            if (Mathf.RoundToInt((j - (leftmostBorder - rowMarginInUnitCubes * unitCube.x + halfCube)) / unitCube.x) % 2 == 0)
                roadSlab.GetComponent<MeshFilter>().mesh = stripedRoadMesh;
            roadSlab.transform.position = new Vector3(j,0.0f,
                transform.position.z);
        }
        generateOneVehicle();
    }

    private void generateOneVehicle()
    {
        if (UnityEngine.Random.value > truckProportion)
        {
            GameObject carInstance = (GameObject)Instantiate(carPrefab, transform);
            carInstance.name = "Vehicle";
            float roadHeightOffset = roadPrefab.GetComponent<Renderer>().bounds.size.y;
            float carHeight = roadHeightOffset;
            float carLateralPosition, carWidthOffset;
            carWidthOffset = carPrefab.GetComponent<Renderer>().bounds.extents.x;
            float carSpeed = UnityEngine.Random.Range(vehicleMinSpeed, vehicleMaxSpeed);
            assignCarModel(carInstance);
            if (IncomingFromLeft)
            {
                carLateralPosition = leftmostBorder - rowMarginInUnitCubes * unitCube.x - carWidthOffset;
                carInstance.transform.Rotate(new Vector3(0, 180, 0));
                carInstance.GetComponent<VehicleController>().IncomingFromLeft = true;
            }
            else
            {
                carLateralPosition = rightmostBorder + rowMarginInUnitCubes * unitCube.x + carWidthOffset;
                carInstance.GetComponent<VehicleController>().IncomingFromLeft = false;
            }
            carInstance.GetComponent<VehicleController>().Speed = new Vector3(
                    -carSpeed, 0, 0);
            carInstance.transform.position = new Vector3(carLateralPosition, carHeight,
                    transform.position.z);
            carInstance.GetComponent<VehicleController>().JustSpawned = true;
        }
        else
        {
            GameObject truckInstance = (GameObject)Instantiate(truckPrefab, transform);
            truckInstance.name = "Vehicle";
            float roadHeightOffset = roadPrefab.GetComponent<Renderer>().bounds.size.y;
            float truckHeight = roadHeightOffset;
            float truckLateralPosition, truckWidthOffset;
            truckWidthOffset = truckPrefab.GetComponent<Renderer>().bounds.extents.x;
            float truckSpeed = UnityEngine.Random.Range(vehicleMinSpeed, vehicleMaxSpeed);
            assigntruckModel(truckInstance);
            if (IncomingFromLeft)
            {
                truckLateralPosition = leftmostBorder - rowMarginInUnitCubes * unitCube.x - truckWidthOffset;
                truckInstance.transform.Rotate(new Vector3(0, 180, 0));
                truckInstance.GetComponent<VehicleController>().IncomingFromLeft = true;
            }
            else
            {
                truckLateralPosition = rightmostBorder + rowMarginInUnitCubes * unitCube.x + truckWidthOffset;
                truckInstance.GetComponent<VehicleController>().IncomingFromLeft = false;
            }
            truckInstance.GetComponent<VehicleController>().Speed = new Vector3(
                    -truckSpeed, 0, 0);
            truckInstance.transform.position = new Vector3(truckLateralPosition, truckHeight,
                    transform.position.z);
            truckInstance.GetComponent<VehicleController>().JustSpawned = true;
        }
    }

    private void generateOneVehicle(LinkedList<CollisionInfo> collisions)
    {
        if (UnityEngine.Random.value > truckProportion)
        {
            GameObject carInstance = (GameObject)Instantiate(carPrefab, transform);
            carInstance.name = "Vehicle";
            float roadHeightOffset = roadPrefab.GetComponent<Renderer>().bounds.size.y;
            float carHeight = roadHeightOffset;
            float carLateralPosition, carWidthOffset;
            carWidthOffset = carPrefab.GetComponent<Renderer>().bounds.extents.x;
            float carSpeed = UnityEngine.Random.Range(vehicleMinSpeed, vehicleMaxSpeed);
            assignCarModel(carInstance);
            if (IncomingFromLeft)
            {
                carLateralPosition = leftmostBorder - rowMarginInUnitCubes*unitCube.x - carWidthOffset;
                carInstance.transform.Rotate(new Vector3(0, 180, 0));
                carInstance.GetComponent<VehicleController>().IncomingFromLeft = true;
            }
            else
            {
                carLateralPosition = rightmostBorder + rowMarginInUnitCubes*unitCube.x + carWidthOffset;
                carInstance.GetComponent<VehicleController>().IncomingFromLeft = false;
            }
            carInstance.GetComponent<VehicleController>().Speed = new Vector3(
                    -carSpeed, 0, 0);
            carInstance.transform.position = new Vector3(carLateralPosition, carHeight,
                    transform.position.z);
            carInstance.GetComponent<VehicleController>().JustSpawned = true;
            carInstance.GetComponent<VehicleController>().CollisionsDebug = collisions;   
        }
        else
        {
            GameObject truckInstance = (GameObject)Instantiate(truckPrefab, transform);
            truckInstance.name = "Vehicle";
            float roadHeightOffset = roadPrefab.GetComponent<Renderer>().bounds.size.y;
            float truckHeight = roadHeightOffset;
            float truckLateralPosition, truckWidthOffset;
            truckWidthOffset = truckPrefab.GetComponent<Renderer>().bounds.extents.x;
            float truckSpeed = UnityEngine.Random.Range(vehicleMinSpeed, vehicleMaxSpeed);
            assigntruckModel(truckInstance);
            if (IncomingFromLeft)
            {
                truckLateralPosition = leftmostBorder - rowMarginInUnitCubes * unitCube.x - truckWidthOffset;
                truckInstance.transform.Rotate(new Vector3(0, 180, 0));
                truckInstance.GetComponent<VehicleController>().IncomingFromLeft = true;
            }
            else
            {
                truckLateralPosition = rightmostBorder + rowMarginInUnitCubes * unitCube.x + truckWidthOffset;
                truckInstance.GetComponent<VehicleController>().IncomingFromLeft = false;
            }
            truckInstance.GetComponent<VehicleController>().Speed = new Vector3(
                    -truckSpeed, 0, 0);
            truckInstance.transform.position = new Vector3(truckLateralPosition, truckHeight,
                    transform.position.z);
            truckInstance.GetComponent<VehicleController>().JustSpawned = true;
            truckInstance.GetComponent<VehicleController>().CollisionsDebug = collisions;
        }
    }

    private void assigntruckModel(GameObject truckInstance)
    {
        float randValue = UnityEngine.Random.value;
        if (randValue < 0.33)
        {
            truckInstance.GetComponent<MeshFilter>().mesh = redTruckMesh;
        }
        else if (randValue >= 0.33 && randValue < 0.66)
        {
            truckInstance.GetComponent<MeshFilter>().mesh = redTruckMesh;
        }
        else
        {
            truckInstance.GetComponent<MeshFilter>().mesh = blueTruckMesh;
        }
    }

    private void assignCarModel(GameObject carInstance)
    {
        float randValue = UnityEngine.Random.value;
        if (randValue < 0.33)
        {
            carInstance.GetComponent<MeshFilter>().mesh = redCarMesh;
        }
        else if (randValue >= 0.33 && randValue < 0.66)
        {
            carInstance.GetComponent<MeshFilter>().mesh = greenCarMesh;
        }
        else
        {
            carInstance.GetComponent<MeshFilter>().mesh = blueCarMesh;
        }
    }

    public bool isTrunkInPosition(Vector3 position)
    {

    }

    public void notifyTrunkDestroyed(GameObject trunk)
    {
        
    }
}

