using UnityEngine;

public class VehicleController : MonoBehaviour {

    private Vector3 speed;
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Time.deltaTime);
	}

    public void setSpeed(Vector3 speed)
    {
        this.speed = speed;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
