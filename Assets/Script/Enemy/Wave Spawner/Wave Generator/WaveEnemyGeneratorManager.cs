using UnityEngine;

public class WaveEnemyGeneratorManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private WaveEnemyGeneratorNormal normalGenerator;
    

    private void Start()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveStarted -= OnWaveStarted;
            WaveManager.Instance.OnWaveStarted += OnWaveStarted;
        }
    }

    private void OnDestroy()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveStarted -= OnWaveStarted;
        }
    }

    private void OnWaveStarted(int waveNumber, int baseEnemyCount)
    {
        bool inOtherWorld = WorldManager.Instance.CurrentWorld == WorldState.OtherWorld;

        int totalEnemies = baseEnemyCount;

        //  Penalidad: jefes y minibosses
        if (waveNumber % 5 == 0) totalEnemies++;
        if (waveNumber % 15 == 0) totalEnemies++;

        //  Penalidad: corrupción Level2 o Level3
        var corruptionLevel = CorruptionManager.Instance.CurrentLevel;
        if (corruptionLevel == CorruptionManager.CorruptionLevel.Level2 ||
            corruptionLevel == CorruptionManager.CorruptionLevel.Level3)
        {
            int extraEnemies = Mathf.FloorToInt(baseEnemyCount * 0.25f); // +25% enemigos
            totalEnemies += extraEnemies;
            Debug.Log($"[WaveEnemyGeneratorManager] Penalidad por corrupción activa. Enemigos extra: {extraEnemies}");
        }
    }
}
