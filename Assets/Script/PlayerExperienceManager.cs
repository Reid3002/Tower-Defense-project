using System;
using UnityEngine;

public class PlayerExperienceManager : MonoBehaviour
{
    public static PlayerExperienceManager Instance { get; private set; }

    private int totalExperience = 0;   // Experiencia acumulada total
    private int sessionExperience = 0;
    private float timePlayed = 0f;

    [Header("Valores de experiencia")]
    [SerializeField] private int xpPorSegundo = 1;
    [SerializeField] private int xpPorBoss = 100;
    [SerializeField] private int xpPorMiniBoss = 50;
    [SerializeField] private int xpPorCambioMundo = 10;
    [SerializeField] private int xpPorOleadaSinDaño = 50;

    private bool coreDamagedThisWave = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Se mantiene entre escenas
            totalExperience = PlayerPrefs.GetInt("TotalExperience", 0); // Cargar acumulado de PlayerPrefs

        }
        else
        {
            Destroy(gameObject); // Si vuelve a escena 1 y ya existe, elimina el duplicado
        }
    }


    private void OnEnable()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveStarted += OnWaveStarted;
            WaveManager.Instance.OnWaveEnded += OnWaveEnded;
        }
        WorldManager.OnWorldChanged += OnWorldChanged;
        Core.OnCoreDamaged += OnCoreDamaged;
    }

    private void OnDisable()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveStarted -= OnWaveStarted;
            WaveManager.Instance.OnWaveEnded -= OnWaveEnded;
        }
        WorldManager.OnWorldChanged -= OnWorldChanged;
        Core.OnCoreDamaged -= OnCoreDamaged;
    }

    private void Update()
    {
        // Solo suma experiencia por tiempo si hay una oleada activa
        if (WaveManager.Instance != null && WaveManager.Instance.WaveInProgress)
        {
            timePlayed += Time.deltaTime;
            if (timePlayed >= 1f)
            {
                AddExperience(xpPorSegundo);
                timePlayed -= 1f;
            }
        }
    }


    private void OnWaveStarted(int wave, int totalEnemies)
    {
        coreDamagedThisWave = false;
    }

    private void OnWaveEnded()
    {
        if (!coreDamagedThisWave)
            AddExperience(xpPorOleadaSinDaño);
    }

    public void OnBossKilled()
    {
        AddExperience(xpPorBoss);
    }
    public void OnMiniBossKilled()
    {
        AddExperience(xpPorMiniBoss);
    }

    public void OnWorldChanged(WorldState newWorld)
    {
        AddExperience(xpPorCambioMundo);
    }

    private void OnCoreDamaged()
    {
        coreDamagedThisWave = true;
    }

    private void AddExperience(int amount)
    {
        sessionExperience += amount;
        WaveUIController.Instance?.UpdateExperienceUI();
        // Evento/feedback visual si querés.
    }
    public void AddExperienceToTotal()
    {
        totalExperience += sessionExperience;
        PlayerPrefs.SetInt("TotalExperience", totalExperience); // Guardá en disco
        sessionExperience = 0; // Reiniciá SOLO la experiencia de la sesión
    }
    public void RemoveTotalExperience(int amount)
    {
        totalExperience = Mathf.Max(0, totalExperience - amount);
        PlayerPrefs.SetInt("TotalExperience", totalExperience);
        PlayerPrefs.Save();
        WaveUIController.Instance?.UpdateExperienceUI();
    }

    public void ResetExperience()
    {
        sessionExperience = 0;
        WaveUIController.Instance?.UpdateExperienceUI();
    }
    public int GetTotalExperience()
    {
        return totalExperience;
    }
    public int GetExperienceThisSession()
    {
        return sessionExperience;
    }

#if UNITY_EDITOR
    [Header("Test XP")]
    public int testXPAmount = 500;

    [ContextMenu("Sumar XP de test")]
    public void AddXPFromInspector()
    {
        AddExperience(testXPAmount);
        Debug.Log($"TEST: +{testXPAmount} XP (Inspector)");
    }
    private int lastTestXPAmount = 0;

    private void OnValidate()
    {
        // Detecta cambio en el inspector y suma XP automáticamente (solo para test)
        if (testXPAmount != lastTestXPAmount)
        {
            int diff = testXPAmount - lastTestXPAmount;
            AddExperience(diff);
            lastTestXPAmount = testXPAmount;
            Debug.Log($"[TEST] Sumar XP desde Inspector: {diff} (XP total: {sessionExperience})");
        }
    }

    [ContextMenu("Reset Total Experience")]
    public void ResetTotalExperience()
    {
        totalExperience = 0;
        PlayerPrefs.DeleteKey("TotalExperience");
        PlayerPrefs.Save();
        Debug.Log("Total de experiencia reseteado.");
        WaveUIController.Instance?.UpdateExperienceUI();
    }


#endif

}
