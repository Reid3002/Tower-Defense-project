using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoreUI : MonoBehaviour
{
    public static CoreUI Instance;

    [Header("UI")]
    public TMP_Text healthText;
    public TMP_Text coreLevelText;
    public TMP_Text upgradeCostText;
    public Button upgradeButton;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        upgradeButton.onClick.AddListener(() => Core.Instance.UpgradeCore());
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (Core.Instance == null) return;

        int current = Core.Instance.GetCurrentHealth();
        int max = Core.Instance.GetMaxHealth();
        int level = Core.Instance.GetCoreLevel();

        healthText.text = $" Health: {current} / {max}";
        coreLevelText.text = $"Level: {level}";

        if (Core.Instance.IsMaxLevel())
        {
            upgradeCostText.text = "Máximo nivel";
            upgradeButton.interactable = false;
        }
        else
        {
            int cost = Core.Instance.GetUpgradeCost();
            upgradeCostText.text = $"({cost} oro)";
            upgradeButton.interactable = GoldManager.Instance.HasEnoughGold(cost);
        }
    }
}
