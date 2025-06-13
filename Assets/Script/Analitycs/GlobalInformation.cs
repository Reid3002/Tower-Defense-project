using UnityEngine;

public class GlobalInformation : Unity.Services.Analytics.Event
{
    public GlobalInformation() : base("GlobalInformation")
    {

    }

    public string MostPlayedWorld { set { SetParameter("MostPlayedWorld", value); } }

    public int WorldSitchAmmount { set { SetParameter("WorldSitchAmmount", value); } }

    public int Deaths { set { SetParameter("Deaths", value); } }

    public int WavesCompleted { set { SetParameter("WavesCompleted", value); } }

    public string MostUsedMod { set { SetParameter("MostUsedMod", value); } }

    public string LeastUsedMod { set { SetParameter("MostUsedMod", value); } }


    public string MostUsedBuilding { set { SetParameter("MostUsedBuilding", value); } }

    public string LeastUsedBuilding { set { SetParameter("LeastUsedBuilding", value); } }


    public float SesionTime { set { SetParameter("SesionTime", value); } }


}
