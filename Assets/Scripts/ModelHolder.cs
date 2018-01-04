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

    public uint winterCutoff, desertCutoff;

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
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
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
            else
            {
                float value = Random.value;
                if (value < 0.33f)
                {
                    return redCarDesert;
                }
                else if (value >= 0.33f && value < 0.66f)
                {
                    return blueCarDesert;
                }
                else
                {
                    return greenCarDesert;
                }
            }            
        }
    }

    public Mesh Truck
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
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
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
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
            else
            {
                float value = Random.value;
                if (value < 0.33f)
                {
                    return redTruckDesert;
                }
                else if (value >= 0.33f && value < 0.66f)
                {
                    return blueTruckDesert;
                }
                else
                {
                    return greenTruckDesert;
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
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                return plainRoadWinter;
            }
            else
            {
                return plainRoadDesert;
            }
        }
    }

    public Mesh RoadForwardStrip
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return forwardStripNormal;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                return forwardStripWinter;
            }
            else
            {
                return forwardStripDesert;
            }
        }
    }

    public Mesh RoadBackwardStrip
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return backStripNormal;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                return backStripWinter;
            }
            else
            {
                return backStripDesert;
            }
        }
    }

    public Mesh RoadBothStrip
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return bothStripNormal;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                return bothStripWinter;
            }
            else
            {
                return bothStripDesert;
            }
        }
    }

    public Mesh GrassClear
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return clearGrassNormal;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                return clearGrassWinter;
            }
            else
            {
                return clearGrassDesert;
            }
        }
    }

    public Mesh GrassDark
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return darkGrassNormal;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                return darkGrassWinter;
            }
            else
            {
                return darkGrassDesert;
            }
        }
    }

    public Mesh WaterClear
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return clearWaterNormal;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                return clearWaterWinter;
            }
            else
            {
                return clearWaterDesert;
            }
        }
    }

    public Mesh WaterDark
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return darkWaterNormal;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                return darkWaterWinter;
            }
            else
            {
                return darkWaterDesert;
            }
        }
    }

    public Mesh Boulder
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return boulderNormal;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                return boulderWinter;
            }
            else
            {
                return boulderDesert;
            }
        }
    }

    public Mesh Tree
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                return treeNormal;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                return treeWinter;
            }
            else
            {
                return treeDesert;
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
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
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
            else
            {
                float value = Random.value;
                if (value < 0.33f)
                {
                    returned.returnedMesh = trunkSmallDesert;
                    returned.isLarge = false;
                    return returned;
                }
                else if (value >= 0.33f && value < 0.66f)
                {
                    returned.returnedMesh = trunkMediumDesert;
                    returned.isLarge = false;
                    return returned;
                }
                else
                {
                    returned.returnedMesh = trunkLargeDesert;
                    returned.isLarge = true;
                    return returned;
                }
            }
        }
    }
}

