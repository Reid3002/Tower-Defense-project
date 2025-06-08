using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.Collections.Generic;
using Unity.Services.Analytics;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float timePlayed { get; private set; } // público para que lo use la ResultScene
    private void Update()
    {
        timePlayed += Time.deltaTime;
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GameOver()
    {
        int wave = WaveManager.Instance != null ? WaveManager.Instance.GetCurrentWave() : 0;

        // Guardar ambas esencias de la sesión para la ResultScene
        int normalEssenceGain = PlayerExperienceManager.Instance.GetSessionEssence(WorldState.Normal);
        int otherEssenceGain = PlayerExperienceManager.Instance.GetSessionEssence(WorldState.OtherWorld);
        ResultData.SetData(timePlayed, wave, normalEssenceGain, otherEssenceGain);

        // Sumar ambas esencias de la sesión al total acumulado SOLO en GameOver
        PlayerExperienceManager.Instance.AddEssenceSessionToTotal();

        AnalyticsManager.Instance.CurrentWave(wave);
        AnalyticsManager.Instance.RecordModifierUsage(GameModifiersManager.Instance.GetAllModifierUsageCounts());

        SceneManager.LoadScene("ResultScene");
    }

}
