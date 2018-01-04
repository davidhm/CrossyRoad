using UnityEngine;
public class ModelHolder : MonoBehaviour
{

    public class TrunkReturn
    {
        public Mesh returnedMesh;
        public bool isLarge;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool firstRows;

    //Cars
    public Mesh redCarNormal, redCarWinter, redCarDesert;
    public Mesh blueCarNormal, blueCarWinter, blueCarDesert;
    public Mesh greenCarNormal, greenCarWinter, greenCarDesert;
    //Trucks
    public Mesh redTruckNormal, redTruckWinter, redTruckDesert;
    public Mesh blueTruckNormal, blueTruckWinter, blueTruckDesert;
    public Mesh greenTruckNormal, greenTruckWinter, greenTruckDesert;
    //Road slabs
    public Mesh plainRoadNormal, plainRoadWinter, plainRoadDesert;
    public Mesh backStripNormal, backStripWinter, backStripDesert;
    public Mesh forwardStripNormal, forwardStripWinter, forwardStripDesert;
    public Mesh bothStripNormal, bothStripWinter, bothStripDesert;
    //Grass
    public Mesh clearGrassNormal, clearGrassWinter, clearGrassDesert;
    public Mesh darkGrassNormal, darkGrassWinter, darkGrassDesert;
    //Water
    public Mesh clearWaterNormal, clearWaterWinter, clearWaterDesert;
    public Mesh darkWaterNormal, darkWaterWinter, darkWaterDesert;
    //Boulder
    public Mesh boulderNormal, boulderWinter, boulderDesert;
    //Tree
    public Mesh treeNormal, treeWinter, treeDesert;
    //Train
    public Mesh trainLocomotiveNormal, trainLocomotiveWinter;
    public Mesh trainWagonNormal, trainWagonWinter;
    //Trunk
    public Mesh trunkSmallNormal, trunkSmallWinter, trunkSmallDesert;
    public Mesh trunkMediumNormal, trunkMediumWinter, trunkMediumDesert;
    public Mesh trunkLargeNormal, trunkLargeWinter, trunkLargeDesert;

    public uint winterCutoff;

    public Mesh Car
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                float value = Random.value;
                if (value < 0.33f)
                {
                    return redCarNormal;
                }
                else if (value >= 0.33f && value < 0.66f)
                {
                    return blueCarNormal;
                }
                else
                {
                    return greenCarNormal;
                }
            }
            else
            {
                float value = Random.value;
                if (value < 0.33f)
                {
                    return redCarWinter;
                }
                else if (value >= 0.33f && value < 0.66f)
                {
                    return blueCarWinter;
                }
                else
                {
                    return greenCarWinter;
                }
            }            
        }
    }

    public Mesh Truck
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                float value = Random.value;
                if (value < 0.33f)
                {
                    return redTruckNormal;
                }
                else if (value >= 0.33f && value < 0.66f)
                {
                    return blueTruckNormal;
                }
                else
                {
                    return greenTruckNormal;
                }
            }
            else
            {
                float value = Random.value;
                if (value < 0.33f)
                {
                    return redTruckWinter;
                }
                else if (value >= 0.33f && value < 0.66f)
                {
                    return blueTruckWinter;
                }
                else
                {
                    return greenTruckWinter;
                }
            }
        }
    }

    public Mesh RoadClear
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return plainRoadNormal;
            }
            else
            {
                return plainRoadWinter;
            }
        }
    }

    public Mesh RoadForwardStrip
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return forwardStripNormal;
            }
            else
            {
                return forwardStripWinter;
            }
        }
    }

    public Mesh RoadBackwardStrip
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return backStripNormal;
            }
            else
            {
                return backStripWinter;
            }
        }
    }

    public Mesh RoadBothStrip
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return bothStripNormal;
            }
            else
            {
                return bothStripWinter;
            }
        }
    }

    public Mesh GrassClear
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return clearGrassNormal;
            }
            else
            {
                return clearGrassWinter;
            }
        }
    }

    public Mesh GrassDark
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return darkGrassNormal;
            }
            else
            {
                return darkGrassWinter;
            }
        }
    }

    public Mesh WaterClear
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return clearWaterNormal;
            }
            else
            {
                return clearWaterWinter;
            }
        }
    }

    public Mesh WaterDark
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return darkWaterNormal;
            }
            else
            {
                return darkWaterWinter;
            }
        }
    }

    public Mesh Boulder
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return boulderNormal;
            }
            else
            {
                return boulderWinter;
            }
        }
    }

    public Mesh Tree
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return treeNormal;
            }
            else
            {
                return treeWinter;
            }
        }
    }

    public Mesh TrainLocomotive
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return trainLocomotiveNormal;
            }
            else
            {
                return trainLocomotiveWinter;
            }
        }
    }

    public Mesh TrainWagon
    {
        get
        {
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return trainWagonNormal;
            }
            else
            {
                return trainWagonWinter;
            }
        }
    }

    public TrunkReturn Trunk
    {
        get
        {
            TrunkReturn returned = new TrunkReturn();
            if ( firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                float value = Random.value;
                if (value < 0.33f)
                {
                    returned.returnedMesh = trunkSmallNormal;
                    returned.isLarge = false;
                    return returned;   
                }
                else if (value >= 0.33f && value < 0.66f)
                {
                    returned.returnedMesh = trunkMediumNormal;
                    returned.isLarge = false;
                    return returned;
                }
                else
                {
                    returned.returnedMesh = trunkLargeNormal;
                    returned.isLarge = true;
                    return returned;
                }
            }
            else
            {
                float value = Random.value;
                if (value < 0.33f)
                {
                    returned.returnedMesh = trunkSmallWinter;
                    returned.isLarge = false;
                    return returned;
                }
                else if (value >= 0.33f && value < 0.66f)
                {
                    returned.returnedMesh = trunkMediumWinter;
                    returned.isLarge = false;
                    return returned;
                }
                else
                {
                    returned.returnedMesh = trunkLargeWinter;
                    returned.isLarge = false;
                    return returned;
                }
            }
        }
    }
}

