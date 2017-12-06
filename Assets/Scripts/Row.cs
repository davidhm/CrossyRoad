using UnityEngine;
public enum rowType { Grass, Road, Water}
public class Row : MonoBehaviour
{
    public GameObject carPrefab, treePrefab, grassPrefab,roadPrefab;
    public static float leftmostBorder;
    public static uint rowWidthInUnitCubes;
    private static float rightmostBorder;
    private rowType currentType;
    public static Vector3 unitCube;
    private static float halfCube;
    private bool incomingFromLeft;
    private float truckProportion;
    private float vehicleMaxSpeed,vehicleMinSpeed;
    void Start()
    {
        rightmostBorder = ((float)rowWidthInUnitCubes) * 2.0f * halfCube + leftmostBorder;
        halfCube = unitCube.z / 2.0f;
    }

    void LateUpdate() {
        if (transform.Find("Vehicle") == null)
            generateOneVehicle();
    }
    public void setCurrentType(rowType type)
    {
        currentType = type;
    }

    public void setCurrentSense(bool incomingFromLeft)
    {
        this.incomingFromLeft = incomingFromLeft;
    }

    public void setTruckProportion(float truckProportion)
    {
        this.truckProportion = truckProportion;
    }

    public void setVehicleSpeedLimits(float minSpeed,float maxSpeed)
    {
        vehicleMinSpeed = minSpeed;
        vehicleMaxSpeed = maxSpeed;
    }

    public void generateInitialElements()
    {
        if (currentType == rowType.Road)
        {
            generateInitialRow();
        }
    }
    
    private void generateInitialRow()
    {
        for (float j = leftmostBorder - 7*halfCube;
            j <= 7*halfCube + rightmostBorder; j += 2*halfCube)
        {
            GameObject roadSlab = (GameObject) Instantiate(roadPrefab, transform);
            float slabY = roadSlab.GetComponent<Renderer>().bounds.extents.y;
            roadSlab.transform.position = new Vector3(j,slabY,
                roadSlab.transform.position.z);
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
            float carHeightOffset = carPrefab.GetComponent<Renderer>().bounds.extents.y;
            float carHeight = roadHeightOffset + carHeightOffset;
            float carLateralPosition, carWidthOffset;
            carWidthOffset = carPrefab.GetComponent<Renderer>().bounds.extents.x;
            float carSpeed = Random.Range(vehicleMinSpeed, vehicleMaxSpeed);
            if (incomingFromLeft)
            {
                carLateralPosition = leftmostBorder + carWidthOffset;
                carInstance.GetComponent<VehicleController>().setSpeed(new Vector3(
                    carSpeed,0,0));
            }
            else
            {
                carLateralPosition = rightmostBorder - carWidthOffset;
                carInstance.GetComponent<VehicleController>().setSpeed(new Vector3(
                    -carSpeed, 0, 0));
            }
            carInstance.transform.position = new Vector3(carLateralPosition, carHeight,
                carInstance.transform.position.z);
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

