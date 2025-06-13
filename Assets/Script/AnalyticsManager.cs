using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Rendering;


public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool isInitialized = false;


    //---------Wave Variables----------------------------------------------------------

    private int _waveNumber;
    private int _enemiesSpawned;
    private string _modifierForThisWave;
    private int _enemiesDefeated;
    private int _goldEarned;
    private float _timeTakenToFinishWave;
    private string _mostSpawnedEnemy;

    //--------------------------------------------------------------------------------




    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            isInitialized = true; // Ahora se inicializa correctamente
            Debug.Log("Analytics inicializado correctamente");
        }
    }

    public void CurrentWave(int maxWave)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Analytics no inicializado aún.");
            return;
        }
        // Crear un evento personalizado y agregar parámetros
        CustomEvent customEvent = new CustomEvent("Max_Wave_Reached");
        customEvent["Max_Wave"] = maxWave;

        // Registrar el evento
        AnalyticsService.Instance.RecordEvent(customEvent);
        AnalyticsService.Instance.Flush();
        Debug.Log($"Evento enviado: Max_Wave_Reached, Max_Wave: {maxWave}");
    }
    public void RecordPlayTime(float playTimeInSeconds)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Analytics no inicializado aún.");
            return;
        }

        CustomEvent playTimeEvent = new CustomEvent("Play_Time");
        playTimeEvent["Seconds"] = playTimeInSeconds;

        AnalyticsService.Instance.RecordEvent(playTimeEvent);
        AnalyticsService.Instance.Flush();
        Debug.Log($"Evento enviado: Play_Time, Seconds: {playTimeInSeconds}");
    }

    public void RecordModifierUsage(Dictionary<string, int> modifierUsages)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Analytics no inicializado aún.");
            return;
        }

        foreach (var modifier in modifierUsages)
        {
            CustomEvent modifierUsageEvent = new CustomEvent("Modifier_Usage");
            modifierUsageEvent["modifier_name"] = modifier.Key;
            modifierUsageEvent["usage_count"] = modifier.Value;

            AnalyticsService.Instance.RecordEvent(modifierUsageEvent);
        }

        AnalyticsService.Instance.Flush();

        Debug.Log($"Eventos enviados: Modifier_Usage, Modificadores: {modifierUsages.Count}");
    }


    public void RecordTilesChosen(List<string> tilesChosen)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Analytics no inicializado aún.");
            return;
        }

        for (int i = 0; i < tilesChosen.Count; i++)
        {
            CustomEvent tileEvent = new CustomEvent("Tile_Chosen");
            tileEvent["tile_name"] = tilesChosen[i];
            tileEvent["selection_order"] = i + 1;  // Orden de selección (1 es el primero)

            AnalyticsService.Instance.RecordEvent(tileEvent);
        }

        AnalyticsService.Instance.Flush();
        Debug.Log($"Eventos enviados: Tile_Chosen, Cantidad: {tilesChosen.Count}");
    }
    
    public void RecordWaveInfoStart(int waveNumber, int enemiesSpawned, string modifierForThisWave = "none")
    {
        _waveNumber = waveNumber;
        _enemiesSpawned = enemiesSpawned;
        _modifierForThisWave = modifierForThisWave;
    }

    public void RecordWaveInfoEnd(int goldEarned, float timeTakenToFinishWave, int enemiesDefeated = 0, string mostSpawnedEnemy = "none", string enemyType1 = "none", int enemiesType1Spawned = 0, string enemyType2 = "none", int enemiesType2Spawned = 0, string enemyType3 = "none", int enemiesType3Spawned = 0)
    {
        _goldEarned = goldEarned;
        _timeTakenToFinishWave = timeTakenToFinishWave;
        _enemiesDefeated = enemiesDefeated;
        _mostSpawnedEnemy = mostSpawnedEnemy;

        WaveInformation waveInformation = new WaveInformation()
        {
            WaveNumber = _waveNumber,
            EnemiesSpawned = _enemiesSpawned,
            EnemiesDefeated = _enemiesDefeated,
            MostSpawnedEnemy = _mostSpawnedEnemy,
            ModifierForThisWave = _modifierForThisWave,
            GoldEarned = _goldEarned,
            TimeTakenToFinishWave = _timeTakenToFinishWave,
        };
        AnalyticsService.Instance.RecordEvent(waveInformation);

        AnalyticsService.Instance.Flush();
        Debug.Log("Wave info sent");

    }  


}
