using Unity.Services.Core;
using UnityEngine;

public class WorldAnalitics : Unity.Services.Analytics.Event
{
    public WorldAnalitics() : base("WorldAnalitics")
    {

    }

    public string World { set { SetParameter("World", value); } }
    public float Time { set { SetParameter("Time", value); } }

    
}
