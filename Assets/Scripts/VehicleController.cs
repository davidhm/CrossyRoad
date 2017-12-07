using UnityEngine;

public class VehicleController : MonoBehaviour {

    private bool overflowedRow;
    private Vector3 speed;
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Time.deltaTime);
        checkRowOverflow();
	}

    private void checkRowOverflow()
    {
        Vector3 unitCube = LevelGenerator.UnitCube;
        float offset = gameObject.GetComponent<Renderer>().bounds.extents.x;
        if (transform.position.x - offset > Row.rightmostBorder + 
            Row.rowMarginInUnitCubes * unitCube.x ||
            transform.position.x + offset < Row.leftmostBorder -
            Row.rowMarginInUnitCubes * unitCube.x)
        {
            Destroy(gameObject);
        }
    }

    public void setSpeed(Vector3 speed)
    {
        this.speed = speed;
    }
}
