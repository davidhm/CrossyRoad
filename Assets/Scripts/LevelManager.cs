using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class LevelManager : MonoBehaviour {

    public GameObject mainMenu, player;
    public GameObject generator;
    public Vector3 unitCube;

    // Use this for initialization
    void Awake()
    {
        generator.GetComponent<LevelGenerator>().setLevelManager(this);
        generator.GetComponent<LevelGenerator>().setUnitCube(unitCube);
        generator.GetComponent<LevelGenerator>().generateInitialArea();
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
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
       /* if (player.GetComponent<PlayerController>().getNewPosition().z >= 1)
        {
            player.GetComponent<PlayerController>().setPlayerState(PlayerController.playerState.Dead);
            mainMenu.transform.GetChild(1).GetComponent<Text>().text = "You win!";
            mainMenu.SetActive(true);
        }*/
    }
}
