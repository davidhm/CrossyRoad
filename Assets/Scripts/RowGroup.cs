using UnityEngine;
class RowGroup
{
    private uint numberOfRows;
    private float nextRowZ;
    private rowType type;
    private GameObject rowGroup;
    public static LevelGenerator generator;
    private RowGroup(float firstRowZ,rowType previousGroupType,uint numberOfRows)
    {
        rowGroup = new GameObject("RowGroup");
        this.numberOfRows = numberOfRows;
        nextRowZ = firstRowZ;
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
        
    }

    private void createGroupWithPreviousTypeRoad()
    {
        for (uint k = 0; k < numberOfRows; ++k)
        {
            GameObject nextRow = Object.Instantiate(generator.getRowPrefab(), rowGroup.transform);
            nextRow.GetComponent<Row>().CurrentType = rowType.Grass;
            nextRow.transform.position = new Vector3(0, 0, nextRowZ);
            nextRowZ += LevelGenerator.UnitCube.z;
            setRandomGrassParameters(nextRow.GetComponent<Row>());
            nextRow.GetComponent<Row>().generateInitialElements();
        }
        type = rowType.Grass;
    }

    private void createGroupWithPreviousTypeGrass()
    {
        for (uint k = 0; k < numberOfRows; ++k)
        {
            GameObject nextRow = Object.Instantiate(generator.getRowPrefab(), rowGroup.transform);
            nextRow.GetComponent<Row>().CurrentType = rowType.Road;
            nextRow.transform.position = new Vector3(0, 0, nextRowZ);
            nextRowZ += LevelGenerator.UnitCube.z;
            setRandomRoadParameters(nextRow.GetComponent<Row>());
            nextRow.GetComponent<Row>().generateInitialElements();
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
        grassRow.TreeProportion = Random.Range(0,0.5f);
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

    public static RowGroup generateRowGroup(float firstRowZ,rowType previousGroupType,uint numberOfRows)
    {
        return new RowGroup(firstRowZ,previousGroupType,numberOfRows);
    }

    public bool isGroupVisible()
    {
        return false;
    }

    public void destroyGroup()
    {

    }
}

