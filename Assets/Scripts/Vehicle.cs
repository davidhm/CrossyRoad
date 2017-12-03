using UnityEngine;

class Vehicle
{
    private GameObject vehiclePrefab;
    private VehicleController controller;
    public Vehicle(GameObject vehiclePrefab,VehicleController controller)
    {
        this.vehiclePrefab = vehiclePrefab;
        this.controller = controller;
    }

}

