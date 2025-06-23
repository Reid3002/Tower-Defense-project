using Unity.Services.Core;
using UnityEngine;

public class WaveInformation : Unity.Services.Analytics.Event
{
    public WaveInformation(): base("WaveInformation") 
    {

    }

    public int WaveNumber { set { SetParameter("WaveNumber", value); } } 

    public float TimeTakenToFinishWave { set { SetParameter("TimeTakenToFinishWave", value); } }

    public int Deaths { set { SetParameter("Deaths", value); } }    

    public bool Completed { set { SetParameter("Completed", value); } }

}
