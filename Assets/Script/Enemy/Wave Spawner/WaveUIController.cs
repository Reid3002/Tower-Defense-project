using TMPro;
using UnityEngine;

public class WaveUIController : MonoBehaviour
{
    public static WaveUIController Instance { get; private set; }

    [Header("Referencias UI")]
    [SerializeField] private TMP_Text waveCounterText;
    [SerializeField] private TMP_Text enemiesRemainingText;
    //[SerializeField] private TMP_Text spawnMultiplierText;
    [SerializeField] private TMP_Text experienceText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveStarted += UpdateWaveUI;
            WaveManager.Instance.OnWaveEnded += TryTriggerNextWave;
        }
    }

    public void UpdateWaveUI(int waveNumber, int totalEnemies)
    {
        if (waveCounterText != null)
            waveCounterText.text = $"Oleada: {waveNumber}";

        UpdateEnemiesRemaining(totalEnemies);
        UpdateExperienceUI();

        //UpdateSpawnMultiplierUI();
    }
    public void UpdateExperienceUI()
    {
        if (experienceText == null)
        {
            Debug.LogWarning("[WaveUIController] El campo experienceText no está asignado en el inspector.");
            return;
        }
        int xp = PlayerExperienceManager.Instance.GetExperienceThisSession ();
        experienceText.text = $"Puntos: {xp}";
    }


    /*public void UpdateSpawnMultiplierUI()
    {
        if (spawnMultiplierText == null) return;

        float goldMultiplier = OtherWorldBonusController.Instance != null
            ? OtherWorldBonusController.Instance.GetGoldBonusMultiplier()
            : 1f;

        float enemyMultiplier = OtherWorldBonusController.Instance != null
            ? OtherWorldBonusController.Instance.GetEnemyBonusMultiplier()
            : 1f;

        spawnMultiplierText.text = $"Oro x{goldMultiplier:F2} | Enemigos x{enemyMultiplier:F2}";
    }
    */

    public void UpdateEnemiesRemaining(int remaining)
    {
        if (enemiesRemainingText == null) return;

        int total = WaveManager.Instance.GetEnemiesThisWave();

        int fast = EnemyTracker.CountEnemiesOfType(EnemyType.Fast);
        int heavy = EnemyTracker.CountEnemiesOfType(EnemyType.Heavy);
        int normal = fast + heavy;

        int other = EnemyTracker.CountEnemies(WorldState.OtherWorld);
        int bosses = EnemyTracker.CountEnemiesOfType(EnemyType.Boss);
        int miniBosses = EnemyTracker.CountEnemiesOfType(EnemyType.MiniBoss);


        string output = $"Total: {remaining} / {total}\n";

        if (normal > 0)
            output += $"Normal: {normal}\n";

        if (other > 0)
            output += $"OtherWorld: {other}\n";

        if (miniBosses > 0)
            output += $"MiniBosses: {miniBosses}\n";

        if (bosses > 0)
            output += $"Bosses: {bosses}\n";


        enemiesRemainingText.text = output.TrimEnd(); // Elimina el salto final extra
    }


    private void TryTriggerNextWave()
    {
        // Verificamos si hay un nuevo tile seleccionado y lo aplicamos antes de la nueva oleada
        if (WaveManager.Instance.GetCurrentWave() > 0)
        {
            UIManager.Instance.ShowTileSelection(GridManager.Instance.GetTileOptions());
            //UpdateSpawnMultiplierUI();

        }
    }

    private void OnDestroy()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveStarted -= UpdateWaveUI;
            WaveManager.Instance.OnWaveEnded -= TryTriggerNextWave;
        }
    }
}
