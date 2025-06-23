using Unity.Services.Core;
using UnityEngine;

public class NemesisAnalitics : Unity.Services.Analytics.Event
{
    public NemesisAnalitics() : base("NemesisAnalitics")
    {

    }

    public string Name { set { SetParameter("Name", value); } }

    public int Kills { set { SetParameter("Kills", value); } }
}
