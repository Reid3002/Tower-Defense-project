using UnityEngine;

public class SesionInformation : Unity.Services.Analytics.Event
{
    public SesionInformation() : base("SesionInformation")
    {

    }
    
    public int WorldSwitchAmmount { set { SetParameter("WorldSwitchAmmount", value); } }

    public int Deaths { set { SetParameter("Deaths", value); } }

    public int TimesPlayed { set { SetParameter("TimesPlayed", value); } }

    public float SesionTime { set { SetParameter("SesionTime", value); } }

}
