using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class LevelManager : MonoBehaviour
{

    public GameObject mainMenu, player, cameraObject;
    public GameObject generatorPrefab;
    public Vector3 unitCube;
    public GameObject scoreCanvas;
    public GameObject scoreHolder;
    public float vehicleMaxSpeed, vehicleMinSpeed;
    private GameObject generatorRuntime;
    private Vector3 initialPlayerPosition;
    private bool firstPlayerMove;
    public GameObject firstMenu;

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
        firstPlayerMove = false;
    }



    void LateUpdate()
    {
        if (!firstPlayerMove && player.GetComponent<PlayerController>().PlayerMoved)
        {
            firstPlayerMove = true;
            scoreCanvas.transform.Find("PlayerScore").GetComponent<Text>().text = "0";
            scoreCanvas.SetActive(true);
            firstMenu.SetActive(false);
        }
        if (player.GetComponent<PlayerController>().JustIncreasedRow)
        {
            scoreHolder.GetComponent<ScoreHolder>().CurrentPlayerScore = 
                player.GetComponent<PlayerController>().NumberOfRowsPassed;
            drawPlayerScore();
        }
    }

    private void drawPlayerScore()
    {
        scoreCanvas.transform.Find("PlayerScore").GetComponent<Text>().text =
            scoreHolder.GetComponent<ScoreHolder>().CurrentPlayerScore.ToString();   
    }

    private void generalLoss()
    {
        cameraObject.GetComponent<CameraController>().CurrentState = cameraStates.PlayerDead;
        if (player.GetComponent<PlayerController>().NumberOfRowsPassed >
            scoreHolder.GetComponent<ScoreHolder>().PlayerMaxScore)
        {
            scoreHolder.GetComponent<ScoreHolder>().PlayerMaxScore = 
                player.GetComponent<PlayerController>().NumberOfRowsPassed;
        }
        scoreCanvas.transform.Find("TopScore").GetComponent<Text>().text =
            "TOP " + scoreHolder.GetComponent<ScoreHolder>().PlayerMaxScore.ToString();
        scoreCanvas.transform.Find("TopScore").gameObject.SetActive(true);
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

    public Vector3 getFutureTrunkPosition(Vector3 movementDestination, float timeToCollision)
    {
        return generatorRuntime.GetComponent<LevelGenerator>().getFutureTrunkPosition(movementDestination, timeToCollision);
    }

    public void attachPlayerToTrunk(GameObject gameObject)
    {
        generatorRuntime.GetComponent<LevelGenerator>().attachPlayerToTrunk(gameObject);
    }
}
