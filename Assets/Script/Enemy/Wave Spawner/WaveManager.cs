using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private int maxWaves = 45;
    [SerializeField] private int baseEnemiesPerWave = 3;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    //private int otherWorldEnemyCount = 0;
    private int enemiesThisWave = 0;
    private int consecutiveOtherWorldWaves = 0;
    private WorldState previousWorldState = WorldState.Normal;

    public bool WaveInProgress => enemiesAlive > 0;

    public event Action<int, int> OnWaveStarted;
    public event Action OnWaveEnded;

    //public event Action<GoldTurretIncome> OnGoldTurretRegistered;

    private bool isFirstWave = true;

    public bool IsFirstWave => isFirstWave;
    private bool waveStarted = false;
    private WorldState waveStartWorld;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
        // Si necesitás registrar lógica especial, hacelo acá
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
    public void TryStartFirstWave()
    {
        if (!waveStarted && currentWave == 0)
        {
            StartNextWave();
        }
    }

    public void StartNextWave()
    {
        if (waveStarted)
            return;

        waveStarted = true;
        currentWave++;

        waveStartWorld = WorldManager.Instance.CurrentWorld;

        CalculateWorldStreak();

        int totalEnemies = CalculateEnemiesThisWave();

        // Incluir jefes en el conteo manual
        if (currentWave % 5 == 0)
            totalEnemies++; // MiniBoss

        if (currentWave % 15 == 0)
            totalEnemies++; // Boss

        enemiesThisWave = 0;
        enemiesAlive = 0;

        Debug.Log($"[WaveManager] Oleada {currentWave} iniciada con {totalEnemies} enemigos.");
        OnWaveStarted?.Invoke(currentWave, totalEnemies);
    }

    public void NotifyEnemyKilled()
    {
        enemiesAlive--;

        // Actualizá la UI de enemigos restantes
        WaveUIController.Instance?.UpdateEnemiesRemaining(enemiesAlive);

        if (enemiesAlive <= 0)
        {
            Debug.Log($"[WaveManager] Oleada {currentWave} finalizada.");
            waveStarted = false;

            // Penalidad de corrupción nivel 3
            if (CorruptionManager.Instance != null && CorruptionManager.Instance.CurrentLevel == CorruptionManager.CorruptionLevel.Level3)
            {
                Debug.Log("[WaveManager] Penalidad de corrupción nivel 3 activa: daño automático al núcleo.");
                Core.Instance?.TakeDamage(1);
            }

            // Upgrades
            var upgradeMgr = UpgradeManager.Instance;

            // Oro pasivo (siempre al terminar una oleada)
            if (upgradeMgr != null && upgradeMgr.IsUnlocked("gold_pasive"))
                GoldManager.Instance?.AddGold(5);

            // Corrupción menos en NormalWorld
            if (upgradeMgr != null &&
                upgradeMgr.IsUnlocked("corruption_normalWorld") &&
                WorldManager.Instance.CurrentWorld == WorldState.Normal)
            {
                CorruptionManager.Instance?.ReduceCorruptionPercent(0.02f); // 2%
            }

            // Oro extra si la oleada terminó en OtherWorld
            if (upgradeMgr != null &&
                upgradeMgr.IsUnlocked("gold_otherWorld") &&
                WorldManager.Instance.CurrentWorld == WorldState.OtherWorld)
            {
                GoldManager.Instance?.AddGold(15);
            }

            GoldManager.Instance?.UpdateGoldUI();

            // Evento de fin de oleada
            OnWaveEnded?.Invoke();
        }
    }


    public int GetCurrentWave() => currentWave;
    public int GetEnemiesAlive() => enemiesAlive;
    public int GetEnemiesThisWave() => enemiesThisWave;
    public bool IsLastWave() => currentWave >= maxWaves;

    private void CalculateWorldStreak()
    {
        var currentWorld = WorldManager.Instance.CurrentWorld;
        if (currentWorld == WorldState.OtherWorld)
        {
            if (previousWorldState == WorldState.OtherWorld)
                consecutiveOtherWorldWaves++;
            else
                consecutiveOtherWorldWaves = 1;
        }
        else
        {
            consecutiveOtherWorldWaves = 0;
        }

        previousWorldState = currentWorld;
    }

    private int CalculateEnemiesThisWave()
    {
        int extraEnemies = 0;

        if (currentWave % 5 == 0)
            extraEnemies++; // MiniBoss

        if (currentWave % 15 == 0)
            extraEnemies++; // Boss

        int scaling = (currentWave - 1) * 2;
        int bonus = Mathf.RoundToInt(GameModifiersManager.Instance.enemyCountMultiplier);

        int total = baseEnemiesPerWave + scaling + bonus + extraEnemies;

        Debug.Log($"[WaveManager] Total enemigos esta oleada: {total} (Base: {baseEnemiesPerWave}, Escalado: {scaling}, Bonus: {bonus}, Jefes: {extraEnemies})");
        return total;
    }

    public void RegisterEnemyManually()
    {
        enemiesAlive++;
        enemiesThisWave++;
        WaveUIController.Instance?.UpdateEnemiesRemaining(enemiesAlive);

        Debug.Log($"[WaveManager] Enemigo adicional registrado manualmente. Enemigos vivos: {enemiesAlive}");
    }


}
