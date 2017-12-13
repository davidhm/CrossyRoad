using System;
using System.Collections.Generic;
using UnityEngine;
public enum rowType { Grass, Road, Water}
public class Row : MonoBehaviour
{
    public GameObject carPrefab, treePrefab, grassPrefab,roadPrefab;
    public GameObject truckFrontPrefab, truckBackPrefab;
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
    private List<bool> occupableRow;

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

    public static void setUnitCube(Vector3 unitCube)
    {
        Row.unitCube = unitCube;
        halfCube = unitCube.z / 2.0f;
    }

    void LateUpdate() {
        if (currentType == rowType.Road && transform.Find("Vehicle") == null)
            generateOneVehicle();
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
            if (i < leftmostBorder || i > rightmostBorder || UnityEngine.Random.value < treeProportion)
            {
                GameObject tree = (GameObject)Instantiate(treePrefab, transform);
                float treeHeight = grassPrefab.GetComponent<Renderer>().bounds.size.y;
                tree.transform.position = new Vector3(i, treeHeight, transform.position.z);
                if (k >= 0 && k <= 9 && i > leftmostBorder && i < rightmostBorder)
                    occupableRow[k] = false;
            }
            if (i >= leftmostBorder + halfCube && i < rightmostBorder - halfCube)
                ++k;
        }
    }

    private void generateRoadRow()
    {
        for (float j = leftmostBorder - 5*halfCube;
            j <= 5*halfCube + rightmostBorder; j += 2*halfCube)
        {
            GameObject roadSlab = (GameObject) Instantiate(roadPrefab, transform);
            float slabY = roadSlab.GetComponent<Renderer>().bounds.extents.y;
            roadSlab.transform.position = new Vector3(j,slabY,
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
            if (IncomingFromLeft)
            {
                carLateralPosition = leftmostBorder - 5*unitCube.x + carWidthOffset;
                carInstance.GetComponent<VehicleController>().Speed = new Vector3(
                    -carSpeed,0,0);
                carInstance.transform.Rotate(new Vector3(0, 180, 0));
            }
            else
            {
                carLateralPosition = rightmostBorder + 5*unitCube.x - carWidthOffset;
                carInstance.GetComponent<VehicleController>().Speed = new Vector3(
                    -carSpeed, 0, 0);
            }
            carInstance.transform.position = new Vector3(carLateralPosition, carHeight,
                transform.position.z);            
        }
        else
        {
            GameObject truckFrontInstance = (GameObject)Instantiate(truckFrontPrefab, transform);
            GameObject truckBackInstance = (GameObject)Instantiate(truckBackPrefab, transform);
            float roadHeightOffset = roadPrefab.GetComponent<Renderer>().bounds.size.y;
            float truckHeightOffset = truckFrontPrefab.GetComponent<Renderer>().bounds.extents.y;
            float truckHeight = roadHeightOffset + truckHeightOffset;
            float truckLateralPosition, truckWidthOffset;
            truckWidthOffset = truckFrontPrefab.GetComponent<Renderer>().bounds.extents.x;
            truckWidthOffset += truckBackPrefab.GetComponent<Renderer>().bounds.extents.x;
            float truckSpeed = UnityEngine.Random.Range(vehicleMinSpeed, vehicleMaxSpeed);
            if (incomingFromLeft)
            {
                truckLateralPosition = leftmostBorder + truckWidthOffset;
                truckFrontInstance.GetComponent<VehicleController>().Speed = new Vector3(
                    truckSpeed, 0, 0);
                truckBackInstance.GetComponent<VehicleController>().Speed = new Vector3(
                    truckSpeed, 0, 0);
            }
            else
            {
                truckLateralPosition = rightmostBorder - truckWidthOffset;
                truckFrontInstance.GetComponent<VehicleController>().Speed = new Vector3(
                    truckSpeed, 0, 0);
                truckBackInstance.GetComponent<VehicleController>().Speed = new Vector3(
                    truckSpeed, 0, 0);
            }
            truckInstance.transform.position = new Vector3(truckLateralPosition, truckHeight,
                truckInstance.transform.position.z);
        }
    }
}

