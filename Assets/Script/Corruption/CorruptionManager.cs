using UnityEngine;
using System;

public class CorruptionManager : MonoBehaviour
{
    public static CorruptionManager Instance;

    [Header("Configuración")]
    [SerializeField] private float maxCorruption = 100f;
    [SerializeField] private float corruptionRate = 5f; // por segundo
    [SerializeField][Range(0f, 1f)] private float reductionOnWorldChange = 0.4f; // 40%

    [Header("Valores")]
    [SerializeField] private float currentCorruption = 0f;

    public enum CorruptionLevel { None, Level1, Level2, Level3 }
    public CorruptionLevel CurrentLevel { get; private set; } = CorruptionLevel.None;

    public Action<CorruptionLevel> OnCorruptionLevelChanged;
    public Action<float> OnCorruptionChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (WaveManager.Instance == null || !WaveManager.Instance.WaveInProgress)
            return;

        float actualRate = corruptionRate;
        // Aplicar el modificador del upgrade si está desbloqueado
        if (UpgradeManager.Instance != null && UpgradeManager.Instance.IsUnlocked("less_corruption"))
            actualRate *= 0.97f; // 3% más lento

        currentCorruption += actualRate * Time.deltaTime;
        currentCorruption = Mathf.Min(currentCorruption, maxCorruption);

        OnCorruptionChanged?.Invoke(currentCorruption / maxCorruption);
        UpdateCorruptionLevel();
    }
    private void OnEnable()
    {
        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.OnUpgradeUnlocked += HandleUpgradeUnlocked;
        ApplyUnlockedUpgradesAtStart();
    }

    private void OnDisable()
    {
        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.OnUpgradeUnlocked -= HandleUpgradeUnlocked;
    }

    private void HandleUpgradeUnlocked(UpgradeData upgrade)
    {
        if (upgrade.upgradeId == "less_corruption")
        {
            // Si querés ajustar algún valor global, hacelo acá.
            // Sino, con sólo chequear UpgradeManager.Instance.IsUnlocked en tu lógica alcanza.
        }
        // ... otros upgrades si hace falta
    }

    private void ApplyUnlockedUpgradesAtStart()
    {
        if (UpgradeManager.Instance == null) return;
        foreach (var upgrade in UpgradeManager.Instance.GetUpgrades())
        {
            if (UpgradeManager.Instance.IsUnlocked(upgrade.upgradeId))
                HandleUpgradeUnlocked(upgrade);
        }
    }

    private void UpdateCorruptionLevel()
    {
        float percent = currentCorruption / maxCorruption;

        CorruptionLevel newLevel;

        if (percent < 1f / 3f)
            newLevel = CorruptionLevel.None;
        else if (percent < 2f / 3f)
            newLevel = CorruptionLevel.Level1;
        else if (percent < 1f)
            newLevel = CorruptionLevel.Level2;
        else
            newLevel = CorruptionLevel.Level3;

        if (newLevel != CurrentLevel)
        {
            CurrentLevel = newLevel;
            OnCorruptionLevelChanged?.Invoke(CurrentLevel);
        }
    }

    public void ReduceCorruption()
    {
        float reduction = maxCorruption * reductionOnWorldChange;
        currentCorruption = Mathf.Max(0, currentCorruption - reduction);

        OnCorruptionChanged?.Invoke(currentCorruption / maxCorruption);
        UpdateCorruptionLevel(); 
    }

    public void ReduceCorruptionPercent(float percent)
    {
        currentCorruption -= maxCorruption * percent;
        currentCorruption = Mathf.Max(0, currentCorruption);
    }


    public float GetCorruptionPercent()
    {
        return currentCorruption / maxCorruption;
    }
}
