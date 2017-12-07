using System;
using UnityEngine;
class RowGroup
{
    private uint numberOfRows;
    private rowType type;
    private float nextRowZ;
    private GameObject rowGroup;
    public static LevelGenerator generator;
    private RowGroup(float firstRowZ,rowType previousGroupType)
    {
        rowGroup = new GameObject("RowGroup");
        rowGroup.transform.position = new Vector3(0, 0, firstRowZ);
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
    }

    private void createGroupWithPreviousTypeWater()
    {
        throw new NotImplementedException();
    }

    private void createGroupWithPreviousTypeRoad()
    {
        for (uint k = 0; k < numberOfRows; ++k)
        {
            GameObject nextRow = UnityEngine.Object.Instantiate(generator.getRowPrefab(), rowGroup.transform);
            nextRow.GetComponent<Row>().CurrentType = rowType.Grass;
            nextRow.transform.position = new Vector3(0, 0, nextRowZ);
            nextRowZ += LevelGenerator.UnitCube.z;
            nextRow.GetComponent<Row>().generateInitialElements();
        }
    }

    private void createGroupWithPreviousTypeGrass()
    {
        for (uint k = 0; k < numberOfRows; ++k)
        {
            GameObject nextRow = UnityEngine.Object.Instantiate(generator.getRowPrefab(), rowGroup.transform);
            nextRow.GetComponent<Row>().CurrentType = rowType.Road;
            nextRow.transform.position = new Vector3(0, 0, nextRowZ);
            nextRowZ += LevelGenerator.UnitCube.z;
            setRandomRoadParameters(nextRow.GetComponent<Row>());
            nextRow.GetComponent<Row>().generateInitialElements();
        }
    }

    private void setRandomRoadParameters(Row roadRow)
    {
        roadRow.IncomingFromLeft = UnityEngine.Random.value > 0.5;
        roadRow.TruckProportion = UnityEngine.Random.value;
    }

    private void setRandomGrassParameters(Row grassRow)
    {

    }
    public rowType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
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

        set
        {
            numberOfRows = value;
        }
    }

    public static RowGroup generateRowGroup(float firstRowZ,rowType previousGroupType)
    {
        return new RowGroup(firstRowZ,previousGroupType);
    }

    public bool isGroupVisible()
    {
        return false;
    }

    public void destroyGroup()
    {

    }
}

