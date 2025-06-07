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

        // 1. Guardar datos para la ResultScene:
        int xpGain = PlayerExperienceManager.Instance.GetExperienceThisSession(); // Ver abajo
        ResultData.SetData(timePlayed, wave, xpGain);

        // 2. Sumar XP de esta sesión a la total acumulada SOLO en GameOver (esto evita sumar 2 veces si reiniciás la escena)
        PlayerExperienceManager.Instance.AddExperienceToTotal();


        AnalyticsManager.Instance.CurrentWave(wave);
        AnalyticsManager.Instance.RecordModifierUsage(GameModifiersManager.Instance.GetAllModifierUsageCounts());

        // Reiniciar escena actual
        SceneManager.LoadScene("ResultScene");
    }
}
