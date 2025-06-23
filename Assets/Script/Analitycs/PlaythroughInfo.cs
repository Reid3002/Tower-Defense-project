using Unity.Services.Core;
using UnityEngine;

public class PlaythroughInfo : Unity.Services.Analytics.Event
{
    public PlaythroughInfo() : base("NemesisAnalitics")
    {

    }    

    public int NormalEscence { set { SetParameter("Escence", value); } }

    public int OtherEscence { set { SetParameter("Escence", value); } }

    public int Gold { set { SetParameter("Gold", value); } }

    public float Duration { set { SetParameter("Duration", value); } }
}
