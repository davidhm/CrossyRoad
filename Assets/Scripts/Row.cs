using UnityEngine;
public enum rowType { Grass, Road, Water}
public class Row : MonoBehaviour
{
    public GameObject carPrefab, treePrefab, grassPrefab,roadPrefab;
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

    public static void setUnitCube(Vector3 unitCube)
    {
        Row.unitCube = unitCube;
        halfCube = unitCube.z / 2.0f;
    }

    void LateUpdate() {
        if (transform.Find("Vehicle") == null)
            generateOneVehicle();
    }
    public void generateInitialElements()
    {
        if (CurrentType == rowType.Road)
        {
            generateInitialRow();
        }
    }
    
    private void generateInitialRow()
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
        //if (Random.value > truckProportion)
        //{
            GameObject carInstance = (GameObject)Instantiate(carPrefab, transform);
            carInstance.name = "Vehicle";
            float roadHeightOffset = roadPrefab.GetComponent<Renderer>().bounds.size.y;
            float carHeight = roadHeightOffset;
            float carLateralPosition, carWidthOffset;
            carWidthOffset = carPrefab.GetComponent<Renderer>().bounds.extents.x;
            float carSpeed = Random.Range(VehicleMinSpeed, VehicleMaxSpeed);
            if (IncomingFromLeft)
            {
                carLateralPosition = leftmostBorder - 5*unitCube.x + carWidthOffset;
                carInstance.GetComponent<VehicleController>().setSpeed(new Vector3(
                    -carSpeed,0,0));
                carInstance.transform.Rotate(new Vector3(0, 180, 0));
            }
            else
            {
                carLateralPosition = rightmostBorder + 5*unitCube.x - carWidthOffset;
                carInstance.GetComponent<VehicleController>().setSpeed(new Vector3(
                    -carSpeed, 0, 0));
            }
            carInstance.transform.position = new Vector3(carLateralPosition, carHeight,
                transform.position.z);            
        /*}
        else
        {
            GameObject truckInstance = (GameObject)Instantiate(truckPrefab, transform);
            float roadHeightOffset = roadPrefab.GetComponent<Renderer>().bounds.size.y;
            float truckHeightOffset = truckPrefab.GetComponent<Renderer>().bounds.extents.y;
            float truckHeight = roadHeightOffset + truckHeightOffset;
            float truckLateralPosition, truckWidthOffset;
            truckWidthOffset = truckPrefab.GetComponent<Renderer>().bounds.extents.x;
            float truckSpeed = Random.Range(vehicleMinSpeed, vehicleMaxSpeed);
            if (incomingFromLeft)
            {
                truckLateralPosition = leftmostBorder + truckWidthOffset;
                truckInstance.GetComponent<VehicleController>().setSpeed(new Vector3(
                    truckSpeed, 0, 0));
            }
            else
            {
                truckLateralPosition = rightmostBorder - truckWidthOffset;
                truckInstance.GetComponent<VehicleController>().setSpeed(new Vector3(
                    -truckSpeed, 0, 0));
            }
            truckInstance.transform.position = new Vector3(truckLateralPosition, truckHeight,
                truckInstance.transform.position.z);
        }*/
    }
}

