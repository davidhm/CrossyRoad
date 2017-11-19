using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LevelManager : MonoBehaviour {

    public GameObject mainMenu;
    // Use this for initialization
    void Start () {
	}

    public void treatPlayerCollision()
    {
        mainMenu.SetActive(true);
    }

    public void onReplayButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
