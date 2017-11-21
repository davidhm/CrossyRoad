using UnityEngine;

public class VehicleController : MonoBehaviour {

    public Vector3 initialPosition;
    public Vector3 speed;
		
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Time.deltaTime);
	}
}
