using UnityEngine;

public class WaveEnemyGeneratorManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveEnemyGeneratorNormal normalGenerator;
    [SerializeField] private WaveEnemyGeneratorOther otherWorldGenerator;


    private void Awake()
    {
        if (WaveManager.Instance != null)
            WaveManager.Instance.OnWaveStarted -= OnWaveStarted;
    }
    private void OnEnable()
    {
        if (WaveManager.Instance != null)
            WaveManager.Instance.OnWaveStarted += OnWaveStarted;
    }
    private void OnDisable()
    {
        if (WaveManager.Instance != null)
            WaveManager.Instance.OnWaveStarted -= OnWaveStarted;
    }

    private void OnDestroy()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveStarted -= OnWaveStarted;
        }
    }

    private void OnWaveStarted(int waveNumber, int totalEnemiesThisWave)
    {
        // ¿En qué mundo comenzó la oleada?
        bool inOtherWorld = WorldManager.Instance.CurrentWorld == WorldState.OtherWorld;

        Debug.Log($"[WaveEnemyGeneratorManager] OnWaveStarted llamada - Mundo: {(inOtherWorld ? "OtherWorld" : "NormalWorld")}, Wave: {waveNumber}, Total: {totalEnemiesThisWave}");

        // Si tenés penalizaciones, podés sumar enemigos acá (opcional)
        int finalEnemiesToSpawn = totalEnemiesThisWave;

        // Ejemplo de penalidad (si querés modificar el total):
        if (waveNumber % 5 == 0) finalEnemiesToSpawn++;
        if (waveNumber % 15 == 0) finalEnemiesToSpawn++;

        // Penalidad por corrupción (opcional)
        var corruptionLevel = CorruptionManager.Instance.CurrentLevel;
        if (corruptionLevel == CorruptionManager.CorruptionLevel.Level2 ||
            corruptionLevel == CorruptionManager.CorruptionLevel.Level3)
        {
            int extraEnemies = Mathf.FloorToInt(totalEnemiesThisWave * 0.25f); // +25% enemigos
            finalEnemiesToSpawn += extraEnemies;
            Debug.Log($"[WaveEnemyGeneratorManager] Penalidad por corrupción activa. Enemigos extra: {extraEnemies}");
        }

        // Genera los enemigos en el generador correspondiente
        if (inOtherWorld)
        {
            if (otherWorldGenerator != null)
                otherWorldGenerator.Spawn(waveNumber, finalEnemiesToSpawn);
            else
                Debug.LogError("[WaveEnemyGeneratorManager] Referencia a OtherWorldGenerator faltante.");
        }
        else
        {
            if (normalGenerator != null)
                normalGenerator.Spawn(waveNumber, finalEnemiesToSpawn);
            else
                Debug.LogError("[WaveEnemyGeneratorManager] Referencia a NormalGenerator faltante.");
        }
    }

}
