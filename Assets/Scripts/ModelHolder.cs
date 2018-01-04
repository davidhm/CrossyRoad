using UnityEngine;
public class ModelHolder : MonoBehaviour
{

    public class TrunkReturn
    {
        public Mesh returnedMesh;
        public bool isLarge;
    }

    public enum SupportType { normal, winter, desert}

    public class SupportReturn<ReturnType>
    {
        public SupportType supportType;
        public ReturnType support;
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

    public Mesh Car(SupportType support)
    { 
        if (support == SupportType.normal)
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
        else if (support == SupportType.winter)
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

    public Mesh Truck(SupportType support)
    {        
        if (support == SupportType.normal)
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
        else if (support == SupportType.winter)
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

    public SupportReturn<Mesh> RoadClear
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = plainRoadNormal;
                support.supportType = SupportType.normal;
                return support;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = plainRoadWinter;
                support.supportType = SupportType.winter;
                return support;
            }
            else
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = plainRoadDesert;
                support.supportType = SupportType.desert;
                return support;
            }
        }
    }

    public SupportReturn<Mesh> RoadForwardStrip
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = forwardStripNormal;
                support.supportType = SupportType.normal;
                return support;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = forwardStripWinter;
                support.supportType = SupportType.winter;
                return support;
            }
            else
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = forwardStripDesert;
                support.supportType = SupportType.desert;
                return support;
            }
        }
    }

    public SupportReturn<Mesh> RoadBackwardStrip
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = backStripNormal;
                support.supportType = SupportType.normal;
                return support;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = backStripWinter;
                support.supportType = SupportType.winter;
                return support;
            }
            else
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = backStripDesert;
                support.supportType = SupportType.desert;
                return support;
            }
        }
    }

    public SupportReturn<Mesh> RoadBothStrip
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = bothStripNormal;
                support.supportType = SupportType.normal;
                return support;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = bothStripWinter;
                support.supportType = SupportType.winter;
                return support;
            }
            else
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = bothStripDesert;
                support.supportType = SupportType.desert;
                return support;
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

    public SupportReturn<Mesh> WaterClear
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = clearWaterNormal;
                support.supportType = SupportType.normal;
                return support;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = clearWaterWinter;
                support.supportType = SupportType.winter;
                return support;
            }
            else
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = clearWaterDesert;
                support.supportType = SupportType.desert;
                return support;
            }
        }
    }

    public SupportReturn<Mesh> WaterDark
    {
        get
        {
            if (firstRows || PlayerController.NumberOfRowsPassed < winterCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = darkWaterNormal;
                support.supportType = SupportType.normal;
                return support;
            }
            else if (PlayerController.NumberOfRowsPassed >= winterCutoff &&
                PlayerController.NumberOfRowsPassed < desertCutoff)
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = darkWaterWinter;
                support.supportType = SupportType.winter;
                return support;
            }
            else
            {
                SupportReturn<Mesh> support = new SupportReturn<Mesh>();
                support.support = darkWaterDesert;
                support.supportType = SupportType.desert;
                return support;
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

    public TrunkReturn Trunk(SupportType support)
    {       
        TrunkReturn returned = new TrunkReturn();
        if (support == SupportType.normal)
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
        else if (support == SupportType.winter)
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

