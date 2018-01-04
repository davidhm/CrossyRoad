using UnityEngine;
using System.Collections.Generic;
using System;

class RowGroup
{
    private uint numberOfRows;
    private float nextRowZ;
    private rowType type;
    private GameObject rowGroup;
    public static LevelGenerator generator;
    private float firstRowZ, lastRowZ;
    private List<List<bool>> occupableMatrix;
    private RowGroup(float firstRowZ,rowType previousGroupType,uint numberOfRows)
    {
        rowGroup = new GameObject("RowGroup");
        this.numberOfRows = numberOfRows;
        nextRowZ = firstRowZ;
        this.firstRowZ = firstRowZ;
        rowGroup.transform.position = new Vector3(0, 0, firstRowZ);
        occupableMatrix = new List<List<bool>>((int)numberOfRows);
        for (int i = 0; i < numberOfRows; ++i)
        {
            occupableMatrix.Add(new List<bool>());
            for (int j = 0; j < 9; ++j)
            {
                occupableMatrix[i].Add(true);
            }
        }
        if (previousGroupType == rowType.Grass)
        {
            createGroupWithPreviousTypeGrass();
        }
        else if (previousGroupType == rowType.Road)
        {
            createGroupWithPreviousTypeRoad();
        }
        else if (previousGroupType == rowType.Water)
        {
            createGroupWithPreviousTypeWater();
        }
        lastRowZ = nextRowZ - LevelGenerator.UnitCube.z;
    }

    private void createGroupWithPreviousTypeWater()
    {
        for (int k = 0; k < numberOfRows; ++k)
        {
            GameObject nextRow = UnityEngine.Object.Instantiate(generator.getRowPrefab(), rowGroup.transform);
            nextRow.GetComponent<Row>().CurrentType = rowType.Grass;
            nextRow.GetComponent<Row>().AssetHolder = generator.ModelHolder;
            nextRow.transform.position = new Vector3(0, 0, nextRowZ);
            nextRowZ += LevelGenerator.UnitCube.z;
            setRandomGrassParameters(nextRow.GetComponent<Row>());
            nextRow.GetComponent<Row>().generateInitialElements();
            occupableMatrix[k] = nextRow.GetComponent<Row>().getOccupableRow();
        }
        type = rowType.Grass;
    }

    private void createGroupWithPreviousTypeRoad()
    {
        if (UnityEngine.Random.value > 0.5)
        {
            
            for (int k = 0; k < numberOfRows; ++k)
            {
                GameObject nextRow = UnityEngine.Object.Instantiate(generator.getRowPrefab(), rowGroup.transform);
                nextRow.GetComponent<Row>().CurrentType = rowType.Grass;
                nextRow.GetComponent<Row>().AssetHolder = generator.ModelHolder;
                nextRow.transform.position = new Vector3(0, 0, nextRowZ);
                nextRowZ += LevelGenerator.UnitCube.z;
                setRandomGrassParameters(nextRow.GetComponent<Row>());
                nextRow.GetComponent<Row>().generateInitialElements();
                occupableMatrix[k] = nextRow.GetComponent<Row>().getOccupableRow();
            }
            type = rowType.Grass;
        }
        else
        {
            bool incomingFromLeft = UnityEngine.Random.value > 0.5;
            for (int k = 0; k < numberOfRows; ++k)
            {
                GameObject nextRow = UnityEngine.Object.Instantiate(generator.getRowPrefab(), rowGroup.transform);
                nextRow.GetComponent<Row>().CurrentType = rowType.Water;
                nextRow.GetComponent<Row>().AssetHolder = generator.ModelHolder;
                nextRow.GetComponent<Row>().IncomingFromLeft = incomingFromLeft;
                incomingFromLeft = !incomingFromLeft;
                nextRow.transform.position = new Vector3(0, 0, nextRowZ);
                nextRowZ += LevelGenerator.UnitCube.z;
                nextRow.GetComponent<Row>().generateInitialElements();
                occupableMatrix[k] = nextRow.GetComponent<Row>().getOccupableRow();
            }
            type = rowType.Water;
        }
    }

    private void createGroupWithPreviousTypeGrass()
    {
        for (int k = 0; k < numberOfRows; ++k)
        {
            bool isRailRoad = UnityEngine.Random.value > 0.9f;
            GameObject nextRow = isRailRoad ? null : UnityEngine.Object.Instantiate(generator.getRowPrefab(), rowGroup.transform);
            if (!isRailRoad && numberOfRows > 1)
            {
                nextRow.GetComponent<Row>().AssetHolder = generator.ModelHolder;
                if (k == 0)
                {
                    nextRow.GetComponent<Row>().StripedRoadMesh = generator.ForwardStripeRoadMesh;
                }
                else if (k == numberOfRows - 1)
                {
                    nextRow.GetComponent<Row>().StripedRoadMesh = generator.BackwardStripeRoadMesh;
                }
                else
                {
                    nextRow.GetComponent<Row>().StripedRoadMesh = generator.BothStripeRoadMesh;
                }
            }
            else if (!isRailRoad)
            {
                nextRow.GetComponent<Row>().AssetHolder = generator.ModelHolder;
                nextRow.GetComponent<Row>().StripedRoadMesh = generator.ClearRoadMesh;
            }
            if (!isRailRoad)
            {
                nextRow.transform.position = new Vector3(0, 0, nextRowZ);
                nextRowZ += LevelGenerator.UnitCube.z;
                setRandomRoadParameters(nextRow.GetComponent<Row>());
                nextRow.GetComponent<Row>().CurrentType = rowType.Road;
                nextRow.GetComponent<Row>().generateInitialElements();
                occupableMatrix[k] = nextRow.GetComponent<Row>().getOccupableRow();
            }
            if (isRailRoad)
            {
                GameObject trainRow = UnityEngine.Object.Instantiate(generator.trainRowPrefab, rowGroup.transform);
                trainRow.GetComponent<TrainRowManager>().AssetHolder = generator.ModelHolder;
                trainRow.name = "RailRoad";
                trainRow.transform.position = new Vector3(0, 0, nextRowZ);
                nextRowZ += LevelGenerator.UnitCube.z;
                trainRow.GetComponent<TrainRowManager>().IncomingFromLeft = UnityEngine.Random.value > 0.5;
                trainRow.GetComponent<TrainRowManager>().RoadHeight = 
                    generator.rowPrefab.GetComponent<Row>().roadPrefab.GetComponent<Renderer>().bounds.extents.y;
                trainRow.GetComponent<TrainRowManager>().generateInitialElements();
            }
        }        
        type = rowType.Road;
    }

    private void setRandomRoadParameters(Row roadRow)
    {
        roadRow.IncomingFromLeft = UnityEngine.Random.value > 0.5;
        roadRow.TruckProportion = UnityEngine.Random.value;
    }

    private void setRandomGrassParameters(Row grassRow)
    {
        grassRow.TreeProportion = UnityEngine.Random.Range(0,0.3f);
    }

    public float NextRowZ
    {
        get
        {
            return nextRowZ;
        }
    }

    public uint NumberOfRows
    {
        get
        {
            return numberOfRows;
        }
    }

    public rowType Type
    {
        get
        {
            return type;
        }

    }

    public float FirstRowZ
    {
        get
        {
            return firstRowZ;
        }
    }

    public float LastRowZ
    {
        get
        {
            return lastRowZ;
        }
    }

    public bool checkIfPositionIsFree(Vector3 position, LevelManager manager)
    {        
        if (type == rowType.Grass)
        {
            int column = manager.getColumnInCubeUnits(position);
            int row = Mathf.RoundToInt((position.z - firstRowZ) / LevelGenerator.UnitCube.z);
            return occupableMatrix[row][column];
        }
        return true;
    }

    public bool checkIfTrunkInPosition(Vector3 position)
    {
        if (type != rowType.Water || position.z < firstRowZ || position.z > lastRowZ)
            return false;
        float targetRowZ = Mathf.Round((position.z - firstRowZ) / LevelGenerator.UnitCube.z);
        for (int i = 0; i < rowGroup.transform.childCount; ++i)
        {
            if (rowGroup.transform.GetChild(i).transform.position.z == 
                targetRowZ*LevelGenerator.UnitCube.z + firstRowZ)
            {
                return rowGroup.transform.GetChild(i).gameObject.GetComponent<Row>().isTrunkInPosition(position);
            }
        }
        return false;
    }

    public Vector3 getFutureTrunkPosition(Vector3 movementDestination, float timeToCollision)
    {
        float targetRowZ = Mathf.Round((movementDestination.z - firstRowZ) / LevelGenerator.UnitCube.z);
        for (int i = 0; i < rowGroup.transform.childCount; ++i)
        {
            if (rowGroup.transform.GetChild(i).transform.position.z ==
                targetRowZ * LevelGenerator.UnitCube.z + firstRowZ)
            {
                return rowGroup.transform.GetChild(i).gameObject.GetComponent<Row>().getFutureTrunkPosition(movementDestination,timeToCollision);
            }
        }
        throw new InvalidOperationException("There should always be a candidate for future trunk position");
    }

    public void attachPlayerToTrunk(GameObject gameObject)
    {
        Vector3 position = gameObject.transform.position;
        float targetRowZ = Mathf.Round((position.z - firstRowZ) / LevelGenerator.UnitCube.z);
        int i;
        for (i = 0; i < rowGroup.transform.childCount; ++i)
        {
            if (rowGroup.transform.GetChild(i).transform.position.z ==
                targetRowZ * LevelGenerator.UnitCube.z + firstRowZ)
            {
                rowGroup.transform.GetChild(i).gameObject.GetComponent<Row>().attachPlayerToTrunk(gameObject);
                return;
            }
        }
        if (i == rowGroup.transform.childCount)
            throw new InvalidOperationException("There should always be a trunk to get attached to.");
    }

    public float getTargetHeight(Vector3 position)
    {
        if (type == rowType.Grass)
            return Row.grassHeight;
        if (type == rowType.Road)
            return Row.roadHeight;
        float targetRowZ = Mathf.Round((position.z - firstRowZ) / LevelGenerator.UnitCube.z);
        for(int i = 0; i < rowGroup.transform.childCount; ++i)
        {
            if (rowGroup.transform.GetChild(i).transform.position.z ==
                targetRowZ * LevelGenerator.UnitCube.z + firstRowZ)
            {
                return rowGroup.transform.GetChild(i).gameObject.GetComponent<Row>().getTargetHeight(position);
            }
        }
        return 0;
    }

    public static RowGroup generateRowGroup(float firstRowZ,rowType previousGroupType,uint numberOfRows)
    {
        return new RowGroup(firstRowZ,previousGroupType,numberOfRows);
    }

    public bool isGroupVisible()
    {
        for (int i = 0; i < rowGroup.transform.childCount; ++i)
        {             
            for (int j = 0; j < rowGroup.transform.GetChild(i).childCount; ++j)
            {
                if (rowGroup.transform.GetChild(i).transform.GetChild(j).gameObject.GetComponent<Renderer>().isVisible)
                    return true;
            }            
        }
        return false;
    }

    public void destroyGroup()
    {
        UnityEngine.Object.Destroy(rowGroup);
    }
}

