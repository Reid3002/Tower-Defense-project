using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;


public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool isInitialized = false;

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


}
