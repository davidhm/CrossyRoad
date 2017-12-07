﻿using UnityEngine;


class LevelGenerator : MonoBehaviour {
    public GameObject carPrefab, truckPrefab,treePrefab,grassPrefab,rowPrefab;
    private LevelManager levelManager;
    private Vector3 leftBoundary, rightBoundary;
    private static float halfCube;
    private static Vector3 unitCube;
    public void setLevelManager(LevelManager manager)
    {
        levelManager = manager;
    }

    public void setUnitCube(Vector3 unitCube)
    {
        LevelGenerator.unitCube = unitCube;
        halfCube = unitCube.z / 2.0f;
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
        rowPrefab.GetComponent<Row>().setUnitCube(unitCube);
        Row.leftmostBorder = leftBoundary.x;
        Row.rightmostBorder = leftBoundary.x + 9 * unitCube.x;
        Row.rowWidthInUnitCubes = 9;
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
        rowOffset += 4 * unitCube.z;
        GameObject testRow = Instantiate(rowPrefab, new Vector3(0, 0, rowOffset),Quaternion.identity);
        testRow.GetComponent<Row>().setCurrentType(rowType.Road);
        testRow.GetComponent<Row>().setCurrentSense(true);
        testRow.GetComponent<Row>().setVehicleSpeedLimits(80, 240);
        testRow.GetComponent<Row>().generateInitialElements();
    }
}
