using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Upgrades/Upgrade Data", order = 1)]
public class UpgradeData : ScriptableObject
{
    public string upgradeId;
    public string upgradeName;
    [TextArea]
    public string description;
    public UpgradeCategory category;
    public int xpCost;
}
