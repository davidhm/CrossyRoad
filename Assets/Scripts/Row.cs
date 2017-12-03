using UnityEngine;
public enum rowType { Grass, Road, Water}
public class Row : MonoBehaviour
{
    public GameObject carPrefab, treePrefab, grassPrefab,roadPrefab;
    public float leftLimit;
    private rowType currentType;
    private static Vector3 unitCube;
    private static float halfCube;    
    public void setUnitCube(Vector3 unitCube)
    {
        Row.unitCube = unitCube;
        halfCube = unitCube.z / 2.0f;
    }

    public void setCurrentType(rowType type)
    {
        currentType = type;
    }

    public void generateRowElements()
    {
        if (currentType == rowType.Road)
        {
            generateRoadRow();
        }
    }
    
    private void generateRoadRow()
    {
        for (float j = leftLimit - 7*halfCube;
            j <= 25*halfCube + leftLimit; j += 2*halfCube)
        {
            GameObject roadSlab = (GameObject) Instantiate(roadPrefab, transform);
            roadSlab.transform.position = new Vector3(j,transform.position.y,
                transform.position.z);
        }
    } 
}

