using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIItem : MonoBehaviour
{
    public TMP_Text upgradeNameText;
    public TMP_Text upgradeDescriptionText;
    public TMP_Text xpCostText;
    public Button unlockButton;

    private string upgradeId;

    public void Setup(UpgradeData upgrade, bool unlocked, int currentXP, System.Action<string> onUnlockPressed)
    {
        upgradeId = upgrade.upgradeId;
        upgradeNameText.text = upgrade.upgradeName;
        upgradeDescriptionText.text = upgrade.description;
        xpCostText.text = $"XP: {upgrade.xpCost}";

        unlockButton.interactable = !unlocked && currentXP >= upgrade.xpCost;
        unlockButton.GetComponentInChildren<TMP_Text>().text = unlocked ? "Unlocked" : "Unlock";
        unlockButton.onClick.RemoveAllListeners();

        if (!unlocked && currentXP >= upgrade.xpCost)
        {
            unlockButton.onClick.AddListener(() => onUnlockPressed?.Invoke(upgradeId));
        }
    }

    // Si querés animar el unlock o mostrar feedback visual, podés hacerlo acá.
}
