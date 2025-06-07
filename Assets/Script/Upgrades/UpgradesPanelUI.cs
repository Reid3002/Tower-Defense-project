using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UpgradesPanelUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform generalUpgradesContainer;
    public Transform normalWorldUpgradesContainer;
    public Transform otherWorldUpgradesContainer;
    public UpgradeUIItem upgradeUIPrefab;
    public TMP_Text playerXPText;

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        int playerXP = PlayerExperienceManager.Instance.GetTotalExperience();
        playerXPText.text = $"XP: {playerXP}";

        // Limpiá contenedores
        foreach (Transform child in generalUpgradesContainer) Destroy(child.gameObject);
        foreach (Transform child in normalWorldUpgradesContainer) Destroy(child.gameObject);
        foreach (Transform child in otherWorldUpgradesContainer) Destroy(child.gameObject);

        List<UpgradeData> upgrades = new List<UpgradeData>(UpgradeManager.Instance.GetUpgrades());
        foreach (var upgrade in upgrades)
        {
            bool unlocked = UpgradeManager.Instance.IsUnlocked(upgrade.upgradeId);
            UpgradeUIItem uiItem = Instantiate(upgradeUIPrefab, GetContainer(upgrade.category));
            uiItem.Setup(upgrade, unlocked, playerXP, OnUnlockUpgrade);
        }
    }

    private Transform GetContainer(UpgradeCategory category)
    {
        return category switch
        {
            UpgradeCategory.General => generalUpgradesContainer,
            UpgradeCategory.NormalWorld => normalWorldUpgradesContainer,
            UpgradeCategory.OtherWorld => otherWorldUpgradesContainer,
            _ => generalUpgradesContainer,
        };
    }

    private void OnUnlockUpgrade(string upgradeId)
    {
        var upgrade = UpgradeManager.Instance.GetUpgrades().FirstOrDefault(u => u.upgradeId == upgradeId);
        if (upgrade == null) return;

        // Intentá desbloquear y restá XP solo si fue exitoso
        if (UpgradeManager.Instance.TryUnlockUpgrade(upgradeId, PlayerExperienceManager.Instance.GetTotalExperience()))
        {
            PlayerExperienceManager.Instance.RemoveTotalExperience(upgrade.xpCost);
            RefreshUI();
        }
    }
}
