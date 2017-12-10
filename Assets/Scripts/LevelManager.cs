using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class LevelManager : MonoBehaviour {

    public GameObject mainMenu, player;
    public GameObject generatorPrefab;
    public Vector3 unitCube;
    public float vehicleMaxSpeed, vehicleMinSpeed;
    private GameObject generatorRuntime;
    private Vector3 initialPlayerPosition;

    public Vector3 InitialPlayerPosition
    {
        get
        {
            return initialPlayerPosition;
        }
    }

    // Use this for initialization
    void Awake()
    {
        generatorRuntime = (GameObject)Instantiate(generatorPrefab);
        generatorRuntime.GetComponent<LevelGenerator>().setLevelManager(this);
        LevelGenerator.UnitCube = unitCube;
        generatorRuntime.GetComponent<LevelGenerator>().generateInitialArea();
        initialPlayerPosition = player.transform.position;
    }

    public void treatPlayerCollision()
    {
        mainMenu.transform.GetChild(1).GetComponent<Text>().text = "You lose!";
        mainMenu.SetActive(true);
    }

    public void onReplayButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public Vector3 getPlayerPosition()
    {
        return player.transform.position;
    }

    public bool checkPositionIsOccupable(Vector3 movementDestination)
    {
        return generatorRuntime.GetComponent<LevelGenerator>().checkPositionIsFree(movementDestination);
    }

    public int getColumnInCubeUnits(Vector3 position)
    {
        int res = Mathf.RoundToInt((position.x - InitialPlayerPosition.x + unitCube.x * 4) / unitCube.x);
        return res;
    }
}
