using UnityEngine;
public class ModelHolder : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    //Cars
    public Mesh redCarNormal, redCarWinter;
    public Mesh blueCarNormal, blueCarWinter;
    public Mesh greenCarNormal, greenCarWinter;
    //Trucks
    public Mesh redTruckNormal, redTruckWinter;
    public Mesh blueTruckNormal, blueTruckWinter;
    public Mesh greenTruckNormal, greenTruckWinter;
    //Road slabs
    public Mesh plainRoadNormal, plainRoadWinter;
    public Mesh backStripNormal, backStripWinter;
    public Mesh forwardStripNormal, forwardStripWinter;
    public Mesh bothStripNormal, bothStripWinter;
    //Grass
    public Mesh clearGrassNormal, clearGrassWinter;
    public Mesh darkGrassNormal, darkGrassWinter;
    //Water
    public Mesh clearWaterNormal, clearWaterWinter;
    public Mesh darkWaterNormal, darkWaterWinter;
    //Boulder
    public Mesh boulderNormal, boulderWinter;
    //Tree
    public Mesh treeNormal, treeWinter;
    //Train
    public Mesh trainLocomotiveNormal, trainLocomotiveWinter;
    public Mesh trainWagonNormal, trainWagonWinter;
    //Trunk
    public Mesh trunkSmallNormal, trunkSmallWinter;
    public Mesh trunkMediumNormal, trunkMediumWinter;
    public Mesh trunkLargeNormal, trunkLargeWinter;

    public Mesh Car
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh Truck
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh RoadClear
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh RoadForwardStrip
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh RoadBackwardStrip
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh RoadBothStrip
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh GrassClear
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh GrassDark
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh WaterClear
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh WaterDark
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh Boulder
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh Tree
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh TrainLocomotive
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh TrainWagon
    {
        get
        {
            return redCarNormal;
        }
    }

    public Mesh Trunk
    {
        get
        {
            return redCarNormal;
        }
    }
}

