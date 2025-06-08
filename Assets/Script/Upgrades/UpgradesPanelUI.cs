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
    public TMP_Text normalEssenceText;
    public TMP_Text otherWorldEssenceText;

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        int normalEssence = PlayerExperienceManager.Instance.GetTotalEssence(WorldState.Normal);
        int otherWorldEssence = PlayerExperienceManager.Instance.GetTotalEssence(WorldState.OtherWorld);

        normalEssenceText.text = $"{normalEssence}";
        otherWorldEssenceText.text = $"{otherWorldEssence}";

        // Limpiá contenedores
        foreach (Transform child in generalUpgradesContainer) Destroy(child.gameObject);
        foreach (Transform child in normalWorldUpgradesContainer) Destroy(child.gameObject);
        foreach (Transform child in otherWorldUpgradesContainer) Destroy(child.gameObject);

        List<UpgradeData> upgrades = new List<UpgradeData>(UpgradeManager.Instance.GetUpgrades());
        foreach (var upgrade in upgrades)
        {
            bool unlocked = UpgradeManager.Instance.IsUnlocked(upgrade.upgradeId);
            UpgradeUIItem uiItem = Instantiate(upgradeUIPrefab, GetContainer(upgrade.category));

            // Decidí cuál essence mostrarle según la categoría
            int playerEssence = upgrade.category switch
            {
                UpgradeCategory.General => Mathf.Min(normalEssence, otherWorldEssence), // O ambas, o lo que prefieras
                UpgradeCategory.NormalWorld => normalEssence,
                UpgradeCategory.OtherWorld => otherWorldEssence,
                _ => 0,
            };
            uiItem.Setup(upgrade, unlocked, OnUnlockUpgrade);
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

        bool unlocked = false;

        switch (upgrade.category)
        {
            case UpgradeCategory.NormalWorld:
                if (PlayerExperienceManager.Instance.GetTotalEssence(WorldState.Normal) >= upgrade.xpCost)
                {
                    unlocked = UpgradeManager.Instance.TryUnlockUpgrade(upgradeId, PlayerExperienceManager.Instance.GetTotalEssence(WorldState.Normal));
                    if (unlocked)
                        PlayerExperienceManager.Instance.RemoveEssence(WorldState.Normal, upgrade.xpCost);
                }
                break;

            case UpgradeCategory.OtherWorld:
                if (PlayerExperienceManager.Instance.GetTotalEssence(WorldState.OtherWorld) >= upgrade.xpCost)
                {
                    unlocked = UpgradeManager.Instance.TryUnlockUpgrade(upgradeId, PlayerExperienceManager.Instance.GetTotalEssence(WorldState.OtherWorld));
                    if (unlocked)
                        PlayerExperienceManager.Instance.RemoveEssence(WorldState.OtherWorld, upgrade.xpCost);
                }
                break;

            case UpgradeCategory.General:
                int half = Mathf.CeilToInt(upgrade.xpCost / 2f);
                if (PlayerExperienceManager.Instance.GetTotalEssence(WorldState.Normal) >= half &&
                    PlayerExperienceManager.Instance.GetTotalEssence(WorldState.OtherWorld) >= half)
                {
                    unlocked = UpgradeManager.Instance.TryUnlockUpgrade(upgradeId, half); // El valor no importa, solo para lógica interna
                    if (unlocked)
                        PlayerExperienceManager.Instance.RemoveEssenceBoth(upgrade.xpCost);
                }
                break;
        }

        if (unlocked)
            RefreshUI();
    }


}
