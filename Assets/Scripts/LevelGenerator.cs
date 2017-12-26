using UnityEngine;
using System.Collections.Generic;
using System;

class LevelGenerator : MonoBehaviour {
    public GameObject carPrefab, truckPrefab,treePrefab,grassPrefab,rowPrefab;
    public Mesh darkGrassMesh, clearRoadMesh;
    public Mesh clearGrassMesh, forwardStripeRoadMesh, backwardStripeRoadMesh;
    public Mesh bothStripeRoadMesh;
    private LevelManager levelManager;
    private Vector3 leftBoundary, rightBoundary;
    private static float halfCube;
    private static Vector3 unitCube;
    private float nextRowZ;
    private LinkedList<RowGroup> rows;
    private GameObject initialArea;
    private float timer;

    public static Vector3 UnitCube
    {
        get
        {
            return unitCube;
        }

        set
        {
            unitCube = value;
            halfCube = value.z / 2.0f;
        }
    }

    void Awake()
    {
        timer = 0.0f;
        
    }
    public GameObject getRowPrefab()
    {
        return rowPrefab;
    }
    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            if (!rows.First.Value.isGroupVisible())
            {
                rows.First.Value.destroyGroup();
                rows.RemoveFirst();
            }
            if (rows.Last.Value.isGroupVisible())
            {
                RowGroup nextRow = RowGroup.generateRowGroup(nextRowZ, rows.Last.Value.Type, generateRandomNumberOfRows());
                rows.AddLast(nextRow);
                nextRowZ = nextRow.NextRowZ;
            }
        }
    }

    public rowType getRowTypeFromPosition(Vector3 position)
    {
        if (position.z >= levelManager.InitialPlayerPosition.z + 4 * unitCube.z &&
            position.z >= rows.First.Value.FirstRowZ)
        {
            LinkedListNode<RowGroup> currentNode = rows.First;
            while (currentNode != null && (position.z < currentNode.Value.FirstRowZ ||
                position.z > currentNode.Value.LastRowZ))
            {
                currentNode = currentNode.Next;
            }
            if (currentNode != null)
                return currentNode.Value.Type;
        }
        return rowType.Grass;
    }

    public bool checkIfTrunkInPosition(Vector3 position)
    {
        int column = levelManager.GetComponent<LevelManager>().getColumnInCubeUnits(position);
        if (column < 0 || column >= 9 || position.z < levelManager.InitialPlayerPosition.z - 3 * unitCube.z)
            return false;
        if (position.z >= levelManager.InitialPlayerPosition.z + 4 * unitCube.z &&
            position.z >= rows.First.Value.FirstRowZ)
        {
            LinkedListNode<RowGroup> currentNode = rows.First;
            while (currentNode != null && (position.z < currentNode.Value.FirstRowZ ||
                position.z > currentNode.Value.LastRowZ || currentNode.Value.Type != rowType.Water))
            {
                currentNode = currentNode.Next;
            }
            if (currentNode != null)              
                return currentNode.Value.checkIfTrunkInPosition(position);
        }
        return false;
    }

    public Vector3 getFutureTrunkPosition(Vector3 movementDestination, float timeToCollision)
    {
        LinkedListNode<RowGroup> currentNode = rows.First;
        while (currentNode != null && (movementDestination.z < currentNode.Value.FirstRowZ ||
            movementDestination.z > currentNode.Value.LastRowZ || currentNode.Value.Type != rowType.Water))
        {
            currentNode = currentNode.Next;
        }
        if (currentNode != null)
            return currentNode.Value.getFutureTrunkPosition(movementDestination, timeToCollision);
        throw new InvalidOperationException("There should always be a candidate row group for future trunk position.");
    }

    public float getTargetHeight(Vector3 position)
    {
        if (position.z >= levelManager.InitialPlayerPosition.z + 4 * unitCube.z &&
            position.z >= rows.First.Value.FirstRowZ)
        {
            LinkedListNode<RowGroup> currentNode = rows.First;
            while (currentNode != null && (position.z < currentNode.Value.FirstRowZ ||
                position.z > currentNode.Value.LastRowZ))
            {
                currentNode = currentNode.Next;
            }
            if (currentNode != null && currentNode.Value.Type == rowType.Water)
                return currentNode.Value.getTargetHeight(position);
            if (currentNode != null && currentNode.Value.Type == rowType.Grass)
                return Row.grassHeight;
            if (currentNode != null && currentNode.Value.Type == rowType.Road)
                return Row.roadHeight;
        }
        return Row.grassHeight;
    }

    public bool checkPositionIsFree(Vector3 position)
    {
        int column = levelManager.GetComponent<LevelManager>().getColumnInCubeUnits(position);
        if (column < 0 || column >= 9 || position.z < levelManager.InitialPlayerPosition.z - 3*unitCube.z)
            return false;
        if (position.z >= levelManager.InitialPlayerPosition.z + 4 * unitCube.z &&
            position.z >= rows.First.Value.FirstRowZ)
        {
            LinkedListNode<RowGroup> currentNode = rows.First;
            while (currentNode != null && (position.z < currentNode.Value.FirstRowZ ||
                position.z > currentNode.Value.LastRowZ))
            {
                currentNode = currentNode.Next;
            }
            if (currentNode == null)
                throw new InvalidOperationException("The queried position does not exist in the generator's list.");
            return currentNode.Value.checkIfPositionIsFree(position, levelManager);
        }
        if (position.z < levelManager.InitialPlayerPosition.z + 4 * unitCube.z)
            return true;
        return false;
    }

    public void setLevelManager(LevelManager manager)
    {
        levelManager = manager;
    }

    public void generateInitialArea()
    {
        leftBoundary = levelManager.GetComponent<LevelManager>().getPlayerPosition();
        leftBoundary.x -= 9*halfCube;
        rightBoundary = levelManager.GetComponent<LevelManager>().getPlayerPosition();
        rightBoundary.x += 9*halfCube;
        setUpRowVariables();
        initialArea = new GameObject("initialArea");
        rows = new LinkedList<RowGroup>();
        generateInitialObjects();
        generateInitialRows();
    }

    private void setUpRowVariables()
    {
        Row.setUnitCube(UnitCube);
        Row.leftmostBorder = leftBoundary.x;
        Row.rightmostBorder = leftBoundary.x + 9 * UnitCube.x;
        Row.rowWidthInUnitCubes = 9;
        Row.rowMarginInUnitCubes = 15;
        RowGroup.generator = this;
        Row.VehicleMaxSpeed = levelManager.vehicleMaxSpeed;
        Row.VehicleMinSpeed = levelManager.vehicleMinSpeed;
    }
    private void generateInitialObjects()
    {
        Vector3 aux = new Vector3(halfCube, 0, 0);
        for (float i = -10*unitCube.z; i <= 3*unitCube.z; i += unitCube.z)
        {
            Vector3 offset = new Vector3(0, 0, i);
            for (Vector3 j = (leftBoundary - 30*aux) + aux; 
                j.x <= ((rightBoundary + 30*aux) - aux).x; j += 2* aux)
            {
                Vector3 grassCoordinates = j + offset;
                grassCoordinates.y = grassPrefab.GetComponent<Renderer>().bounds.extents.y;
                GameObject grass = (GameObject) Instantiate(grassPrefab, initialArea.transform);
                grass.transform.position = grassCoordinates;
                if (j.x < leftBoundary.x || j.x > rightBoundary.x || i < -3*unitCube.z)
                {
                    Vector3 treeCoordinates = j + offset;
                    treeCoordinates.y = 1.5f*grassPrefab.GetComponent<Renderer>().bounds.size.y;
                    GameObject tree = (GameObject) Instantiate(treePrefab, initialArea.transform);
                    tree.transform.position = treeCoordinates;
                }
            }
            
        }
    }

    private void generateInitialRows()
    {
        float rowOffset = levelManager.GetComponent<LevelManager>().getPlayerPosition().z;
        rowOffset += 4 * UnitCube.z;
        nextRowZ = rowOffset;
        RowGroup current = RowGroup.generateRowGroup(nextRowZ, rowType.Grass,generateRandomNumberOfRows());
        rows.AddLast(current);
        nextRowZ = current.NextRowZ;
        for (uint i = 0; i < 10; ++i)
        {
            current = RowGroup.generateRowGroup(nextRowZ, current.Type, generateRandomNumberOfRows());
            rows.AddLast(current);
            nextRowZ = current.NextRowZ;
        }
        current = RowGroup.generateRowGroup(nextRowZ, current.Type,generateRandomNumberOfRows());
        rows.AddLast(current);
        nextRowZ = current.NextRowZ;
    }

    private uint generateRandomNumberOfRows()
    {
        float randValue = UnityEngine.Random.value;
        if (randValue < 0.3)
        {
            return 1;
        }
        else if (randValue >= 0.3 && randValue < 0.6)
        {
            return 3;
        }
        else
        {
            return 5;
        }
    }
}
