using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour {

    private bool overflowedRow;
    private Vector3 speed;
    private bool justSpawned;
    private bool incomingFromLeft;
    public AudioClip carHonk, carRumble;
    public float soundProbability;
    private float positionToEmit;
    private bool soundEmitted;

    public Vector3 Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public bool JustSpawned
    {
        get
        {
            return justSpawned;
        }

        set
        {
            justSpawned = value;
        }
    }

    public bool IncomingFromLeft
    {
        get
        {
            return incomingFromLeft;
        }

        set
        {
            incomingFromLeft = value;
        }
    }  

    void Start()
    {
        float lowerLimit = (Row.rightmostBorder - Row.leftmostBorder) / 2.0f;
        lowerLimit -= 1.5f * LevelGenerator.UnitCube.x + Random.Range(0, 1.5f * LevelGenerator.UnitCube.x);
        float upperLimit = (Row.rightmostBorder - Row.leftmostBorder) / 2.0f;
        upperLimit += 1.5f * LevelGenerator.UnitCube.x + Random.Range(0, 1.5f * LevelGenerator.UnitCube.x);
        positionToEmit = Random.Range(lowerLimit, upperLimit);
        soundEmitted = Random.value > soundProbability;
    }
    void Update () {
        transform.Translate(speed * Time.deltaTime);
        checkRowOverflow();
        produceSound();
	}

    private void produceSound()
    {
        if (!soundEmitted &&
            (incomingFromLeft && transform.position.x > positionToEmit ||
            !incomingFromLeft && transform.position.x < positionToEmit))
        {
            soundEmitted = true;
            if (Random.value > 0.5)
            {
                gameObject.GetComponent<AudioSource>().clip = carHonk;
                gameObject.GetComponent<AudioSource>().Play();
            }
            else
            {
                gameObject.GetComponent<AudioSource>().clip = carRumble;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    private void checkRowOverflow()
    {
        if (justSpawned)
        {
            if (incomingFromLeft && transform.position.x > Row.leftmostBorder)
                justSpawned = false;
            else if (!incomingFromLeft && transform.position.x < Row.rightmostBorder)
                justSpawned = false;
        }
        else
        {
            float offset = gameObject.GetComponent<Renderer>().bounds.extents.x;
            if ((incomingFromLeft && transform.position.x >
                Row.rightmostBorder + Row.rowMarginInUnitCubes*LevelGenerator.UnitCube.x + offset) ||
                (!incomingFromLeft && transform.position.x <
                Row.leftmostBorder - Row.rowMarginInUnitCubes * LevelGenerator.UnitCube.x - offset))
            {
                Destroy(gameObject);
            }
        }
    }

    /*void OnTriggerEnter()
    {
        Time.timeScale = 0;
    }*/
}
