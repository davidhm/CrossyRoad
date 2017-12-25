using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class LevelManager : MonoBehaviour {

    public GameObject mainMenu, player, cameraObject;
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

    private void generalLoss()
    {
        cameraObject.GetComponent<CameraController>().CurrentState = cameraStates.PlayerDead;
        mainMenu.transform.GetChild(1).GetComponent<Text>().text = "You lose!";
        mainMenu.SetActive(true);
    }

    public void treatPlayerCollision()
    {
        generalLoss();
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

    public rowType getRowTypeFromPosition(Vector3 position)
    {
        return generatorRuntime.GetComponent<LevelGenerator>().getRowTypeFromPosition(position);
    }

    public bool checkIfTrunkInPosition(Vector3 position)
    {
        return generatorRuntime.GetComponent<LevelGenerator>().checkIfTrunkInPosition(position);
    }

    public int getColumnInCubeUnits(Vector3 position)
    {
        int res = Mathf.RoundToInt((position.x - InitialPlayerPosition.x + unitCube.x * 4) / unitCube.x);
        return res;
    }

    public void treatPlayerInvisible()
    {
        generalLoss();
    }

    public void treatPlayerDrowned()
    {
        generalLoss();
    }

    public float getTargetPositionHeight(Vector3 newDestination)
    {
        return generatorRuntime.GetComponent<LevelGenerator>().getTargetHeight(newDestination);
    }
}
