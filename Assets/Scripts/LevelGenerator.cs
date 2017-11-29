using UnityEngine;


class LevelGenerator : MonoBehaviour {
    public GameObject carPrefab, truckPrefab,treePrefab,grassPrefab;
    private LevelManager levelManager;
    private Vector3 leftBoundary, rightBoundary;
    private static Vector3 lateralHalfSide;
    private static Vector3 unitCube;
    void Start()
    {

    }

    public void setLevelManager(LevelManager manager)
    {
        levelManager = manager;
    }

    public void setUnitCube(Vector3 unitCube)
    {
        LevelGenerator.unitCube = unitCube;
        LevelGenerator.lateralHalfSide = new Vector3(unitCube.x / 2.0f, 0, 0);
    }

    public void generateInitialArea()
    {
        leftBoundary = levelManager.GetComponent<LevelManager>().getPlayerPosition();
        leftBoundary -= 9*lateralHalfSide;
        rightBoundary = levelManager.GetComponent<LevelManager>().getPlayerPosition();
        rightBoundary += 9*lateralHalfSide;
        generateInitialGrass();
    }
    private void generateInitialGrass()
    {
        for (float i = -2*lateralHalfSide.z; i <= 3*lateralHalfSide.z; i += lateralHalfSide.z)
        {
            Vector3 offset = new Vector3(0, 0, i);
            for (Vector3 j = (leftBoundary - 6*lateralHalfSide) + lateralHalfSide; 
                j.x <= ((rightBoundary + 6*lateralHalfSide) - lateralHalfSide).x; j += 2* lateralHalfSide)
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
    private void generateInitialTrees()
    {

    }
}
