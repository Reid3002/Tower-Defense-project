using UnityEngine;

public class SesionInformation : Unity.Services.Analytics.Event
{
    public SesionInformation() : base("SesionInformation")
    {

    }

    public string MostPlayedWorld { set { SetParameter("MostPlayedWorld", value); } }

    public int WorldSwitchAmmount { set { SetParameter("WorldSwitchAmmount", value); } }

    public int Deaths { set { SetParameter("Deaths", value); } }

    public int WavesCompleted { set { SetParameter("WavesCompleted", value); } }

    public string MostUsedMod { set { SetParameter("MostUsedMod", value); } }

    public string LeastUsedMod { set { SetParameter("MostUsedMod", value); } }


    public string MostUsedBuilding { set { SetParameter("MostUsedBuilding", value); } }

    public string LeastUsedBuilding { set { SetParameter("LeastUsedBuilding", value); } }


    public float SesionTime { set { SetParameter("SesionTime", value); } }


}
