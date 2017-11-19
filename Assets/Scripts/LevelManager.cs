using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class LevelManager : MonoBehaviour {

    public GameObject mainMenu, player;
    // Use this for initialization
    void Start () {
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
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        if (player.GetComponent<PlayerController>().getNewPosition().z >= 1)
        {
            player.GetComponent<PlayerController>().setPlayerState(PlayerController.playerState.Dead);
            mainMenu.transform.GetChild(1).GetComponent<Text>().text = "You win!";
            mainMenu.SetActive(true);
        }
    }
}
