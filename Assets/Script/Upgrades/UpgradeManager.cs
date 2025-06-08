using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("All available upgrades")]
    [SerializeField] private List<UpgradeData> allUpgrades;

    public event Action<UpgradeData> OnUpgradeUnlocked;

    private HashSet<string> unlockedUpgrades = new HashSet<string>();
    private const string PlayerPrefsKey = "UnlockedUpgrades";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadUnlockedUpgrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IReadOnlyList<UpgradeData> GetUpgrades() => allUpgrades;

    public bool IsUnlocked(string upgradeId) => unlockedUpgrades.Contains(upgradeId);

    public bool TryUnlockUpgrade(string upgradeId, int playerXP)
    {
        var upgrade = allUpgrades.Find(u => u.upgradeId == upgradeId);
        if (upgrade == null || IsUnlocked(upgradeId)) return false;
        if (playerXP < upgrade.xpCost) return false;

        unlockedUpgrades.Add(upgradeId);
        SaveUnlockedUpgrades();

        OnUpgradeUnlocked?.Invoke(upgrade);
        return true;
    }

    /// Guarda upgrades desbloqueadas en PlayerPrefs como string separado por comas.
    private void SaveUnlockedUpgrades()
    {
        string unlocked = string.Join(",", unlockedUpgrades);
        PlayerPrefs.SetString(PlayerPrefsKey, unlocked);
        PlayerPrefs.Save();
    }
    // Carga upgrades desbloqueadas desde PlayerPrefs.

    private void LoadUnlockedUpgrades()
    {
        unlockedUpgrades.Clear();
        string unlocked = PlayerPrefs.GetString(PlayerPrefsKey, "");
        if (!string.IsNullOrEmpty(unlocked))
        {
            string[] upgrades = unlocked.Split(',');
            foreach (string upg in upgrades)
                if (!string.IsNullOrWhiteSpace(upg))
                    unlockedUpgrades.Add(upg);
        }
    }

    // Devuelve true si alguna vez se desbloqueó al menos una mejora.
    public bool HasAnyUpgradeUnlocked() => unlockedUpgrades.Count > 0;

#if UNITY_EDITOR
    [ContextMenu("Reset Upgrades")]
    public void ResetUpgrades()
    {
        unlockedUpgrades.Clear();
        Debug.Log("[UpgradeManager] Todas las mejoras han sido reseteadas.");
        // Si querés también actualizar la UI automáticamente:
        UpgradesPanelUI panel = FindFirstObjectByType<UpgradesPanelUI>();
        if (panel != null)
            panel.RefreshUI();
    }
#endif

}
