using UnityEngine;

public class TimeDateInfo : Unity.Services.Analytics.Event
{
    public TimeDateInfo() : base("TimeDateInfo")
    {

    }

    public int NumberOfSesions { set { SetParameter("NumberOfSesions", value); } }

    public float TimePlayed { set { SetParameter("TimePlayed", value); } }

}
