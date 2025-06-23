using Unity.Services.Core;
using UnityEngine;

public class FirstUpgradeAnalitycs : Unity.Services.Analytics.Event
{
    public FirstUpgradeAnalitycs() : base("FirstUpgradeAnalitycs")
    {

    }

    public int Deaths { set { SetParameter("Deaths", value); } } 
    
    public string UpgradeName { set { SetParameter("UpgradeName", value); } }

}
