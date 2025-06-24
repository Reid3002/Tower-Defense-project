using Unity.Services.Core;
using UnityEngine;

public class PlaythroughInfo : Unity.Services.Analytics.Event
{
    public PlaythroughInfo() : base("PlaythroughInfo")
    {

    }    

    public int NormalEscence { set { SetParameter("NormalEscence", value); } }

    public int OtherEscence { set { SetParameter("OtherEscence", value); } }

    public int Gold { set { SetParameter("Gold", value); } }

    public float Duration { set { SetParameter("Duration", value); } }
}
