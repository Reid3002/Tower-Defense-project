using Unity.Services.Core;
using UnityEngine;


public class ModifierAnalytics : Unity.Services.Analytics.Event
{
    public ModifierAnalytics() : base("ModifierAnalytics")
    {

    }

    public string Name { set { SetParameter("Name", value); } }

    public int TimesChosen { set { SetParameter("TimesChosen", value); } }
}
