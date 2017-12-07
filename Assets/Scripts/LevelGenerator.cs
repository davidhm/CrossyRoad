using UnityEngine;
using System.Collections.Generic;

class LevelGenerator : MonoBehaviour {
    public GameObject carPrefab, truckPrefab,treePrefab,grassPrefab,rowPrefab;
    private LevelManager levelManager;
    private Vector3 leftBoundary, rightBoundary;
    private static float halfCube;
    private static Vector3 unitCube;
    private float nextRowZ;
    private LinkedList<RowGroup> rows = new LinkedList<RowGroup>();
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

    public GameObject getRowPrefab()
    {
        return rowPrefab;
    }
    void LateUpdate()
    {
        while (!rows.First.Value.isGroupVisible())
        {
            rows.RemoveFirst();
        }
        if (rows.Last.Value.isGroupVisible())
        {
            RowGroup newGroup = RowGroup.generateRowGroup(nextRowZ, rows.Last.Value.Type);
            rows.AddLast(newGroup);
            nextRowZ = newGroup.NextRowZ;
        }
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
        generateInitialObjects();
        generateInitialRows();
    }

    private void setUpRowVariables()
    {
        Row.setUnitCube(UnitCube);
        Row.leftmostBorder = leftBoundary.x;
        Row.rightmostBorder = leftBoundary.x + 9 * UnitCube.x;
        Row.rowWidthInUnitCubes = 9;
        Row.rowMarginInUnitCubes = 3;
        RowGroup.generator = this;
    }
    private void generateInitialObjects()
    {
        Vector3 aux = new Vector3(halfCube, 0, 0);
        for (float i = -4*halfCube; i <= 6*halfCube; i += 2*halfCube)
        {
            Vector3 offset = new Vector3(0, 0, i);
            for (Vector3 j = (leftBoundary - 6*aux) + aux; 
                j.x <= ((rightBoundary + 6*aux) - aux).x; j += 2* aux)
            {
                Vector3 grassCoordinates = j + offset;
                grassCoordinates.y = grassPrefab.transform.localScale.y / 2.0f;
                Instantiate(grassPrefab, grassCoordinates, Quaternion.identity);
                if (j.x < leftBoundary.x || j.x > rightBoundary.x)
                {
                    Vector3 treeCoordinates = j + offset;
                    treeCoordinates.y = grassPrefab.transform.localScale.y;
                    treeCoordinates.y += treePrefab.transform.localScale.y / 2.0f;
                    Instantiate(treePrefab, treeCoordinates, Quaternion.identity);
                }
            }
            
        }
    }

    private void generateInitialRows()
    {
        float rowOffset = levelManager.GetComponent<LevelManager>().getPlayerPosition().z;
        rowOffset += 4 * UnitCube.z;
        nextRowZ = rowOffset;
        RowGroup current = RowGroup.generateRowGroup(nextRowZ, rowType.Grass);
        rows.AddLast(current);
        nextRowZ = current.NextRowZ;
        while (current.isGroupVisible())
        {
            current = RowGroup.generateRowGroup(nextRowZ, current.Type);
            rows.AddLast(current);
            nextRowZ = current.NextRowZ;
        }
        current = RowGroup.generateRowGroup(nextRowZ, rowType.Grass);
        rows.AddLast(current);
        nextRowZ = current.NextRowZ;
    }
}
