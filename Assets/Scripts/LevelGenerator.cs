using UnityEngine;


class LevelGenerator : MonoBehaviour {
    public GameObject carPrefab, truckPrefab,treePrefab,grassPrefab;
    private LevelManager levelManager;
    private Vector3 leftBoundary, rightBoundary;
    private static Vector3 lateralHalfSide = new Vector3(0.5f, 0, 0);
    void Start()
    {

    }

    public void setLevelManager(LevelManager manager)
    {
        levelManager = manager;
    }

    public void generateInitialArea()
    {
        leftBoundary = levelManager.GetComponent<LevelManager>().getPlayerPosition();
        leftBoundary.y = 0.1f;
        leftBoundary -= new Vector3(4.5f, 0, 0);
        rightBoundary = levelManager.GetComponent<LevelManager>().getPlayerPosition();
        rightBoundary.y = 0.1f;
        rightBoundary += new Vector3(4.5f, 0, 0);
        generateInitialGrass();
    }
    private void generateInitialGrass()
    {
        for (int i = -2; i <= 3; ++i)
        {
            Vector3 offset = new Vector3(0, 0, i);
            for (Vector3 j = leftBoundary + lateralHalfSide; 
                j.x <= (rightBoundary - lateralHalfSide).x; j += 2* lateralHalfSide)
            {
                Instantiate(grassPrefab, j + offset,Quaternion.identity);
            }
        }
    }
    private void generateInitialTrees()
    {

    }
}
