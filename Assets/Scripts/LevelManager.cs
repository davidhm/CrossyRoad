using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class LevelManager : MonoBehaviour {

    public GameObject mainMenu, player;
    public GameObject generator;
    public Vector3 unitCube;
    public float vehicleMaxSpeed, vehicleMinSpeed;
    // Use this for initialization
    void Start()
    {
        generator.GetComponent<LevelGenerator>().setLevelManager(this);
        LevelGenerator.UnitCube = unitCube;
        generator.GetComponent<LevelGenerator>().generateInitialArea();
        Row.VehicleMaxSpeed = vehicleMaxSpeed;
        Row.VehicleMinSpeed = vehicleMinSpeed;
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
}
