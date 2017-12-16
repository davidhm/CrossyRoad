﻿using UnityEngine;
using System.Collections.Generic;
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
        
    }

    private void createGroupWithPreviousTypeRoad()
    {
        for (int k = 0; k < numberOfRows; ++k)
        {
            GameObject nextRow = Object.Instantiate(generator.getRowPrefab(), rowGroup.transform);
            nextRow.GetComponent<Row>().CurrentType = rowType.Grass;
            nextRow.transform.position = new Vector3(0, 0, nextRowZ);
            nextRowZ += LevelGenerator.UnitCube.z;
            setRandomGrassParameters(nextRow.GetComponent<Row>());
            nextRow.GetComponent<Row>().generateInitialElements();
            occupableMatrix[k] = nextRow.GetComponent<Row>().getOccupableRow();
        }
        type = rowType.Grass;
    }

    private void createGroupWithPreviousTypeGrass()
    {
        for (int k = 0; k < numberOfRows; ++k)
        {
            GameObject nextRow = Object.Instantiate(generator.getRowPrefab(), rowGroup.transform);
            nextRow.GetComponent<Row>().CurrentType = rowType.Road;
            nextRow.transform.position = new Vector3(0, 0, nextRowZ);
            nextRowZ += LevelGenerator.UnitCube.z;
            setRandomRoadParameters(nextRow.GetComponent<Row>());
            nextRow.GetComponent<Row>().generateInitialElements();
            occupableMatrix[k] = nextRow.GetComponent<Row>().getOccupableRow();
        }
        type = rowType.Road;
    }

    private void setRandomRoadParameters(Row roadRow)
    {
        roadRow.IncomingFromLeft = Random.value > 0.5;
        roadRow.TruckProportion = Random.value;
    }

    private void setRandomGrassParameters(Row grassRow)
    {
        grassRow.TreeProportion = Random.Range(0,0.3f);
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
        Object.Destroy(rowGroup);
    }
}
