using UnityEngine;

public class TurretUpgrade : MonoBehaviour, IUpgradeable
{
    [SerializeField] private int maxUpgradeLevel = 15;
    [SerializeField] private float upgradeMultiplier = 1.25f;

    private TurretStats stats;

    void Awake()
    {
        stats = GetComponent<TurretStats>();
    }

    public bool CanUpgrade()
    {
        return stats.UpgradeLevel < maxUpgradeLevel;
    }

    public void Upgrade()
    {
        if (!CanUpgrade()) return;

        stats.ApplyMultiplier(upgradeMultiplier);
    }

    public int GetUpgradeLevel() => stats.UpgradeLevel;
    public int GetMaxUpgradeLevel() => maxUpgradeLevel;
}
