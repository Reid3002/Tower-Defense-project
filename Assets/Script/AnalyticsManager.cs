using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool isInitialized = false;
    [SerializeField] GameObject warning;

    //---------Wave Variables----------------------------------------------------------

    private int _waveNumber;

    private float _timeTakenToFinishWave =0;

    private int waveDeaths = 0;

    private bool startTimer = false;

    //--------------------------------------------------------------------------------
    //-----------Playthrough Variables------------------------------------------------------------------------

    private int normalEscence = 0;
    private int OtherEscence = 0;
    private int gold = 0;
    private float playTime = 0;
    private bool startPlaythroughTimer = false;
    private bool enableSubscriptions = false;

    //-------------------------------------------------------------------------------------------------------
    //-------------Sesion Variables-------------------------------------------------------------------
    private float timeSpentOnNormalWorld = 0;
    private float timeSpentOnOtherWorld = 0;
    private int numberOfTimesSwitched = 0;
    private float sesionTime = 0;

    private int timesPLayed = 0;

    private int numberOfDeaths;

    private Dictionary<string, int> modsChosen = new Dictionary<string, int>();

    private Dictionary<string, int> nemeses = new Dictionary<string, int>();

    private bool upgradeUnlocked = false;

    //--------------------------------------------------------------------------------
    private float waitTimer = 0;

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
            //AnalyticsService.Instance.StartDataCollection();
            //isInitialized = true; // Ahora se inicializa correctamente
            //Debug.Log("Analytics inicializado correctamente");
        }
        
    }

    private void Start()
    {
        warning.gameObject.SetActive(true);

        WorldManager.OnWorldChanged += GetWorldSwitch;
        Core.OnCoreDestroyed += GetDeaths; 

       

        ResultSceneController.OnButtonPressed += ReportPlaythrough;
        ResultSceneController.OnButtonPressed += RemoveGameplaySubscriptions;
        ResultSceneController.OnMainMenuPressed += RecordWaveAbandonned;
        ResultSceneController.OnMainMenuPressed += ReportPlaythrough;
        ResultSceneController.OnMainMenuPressed += RemoveGameplaySubscriptions;

        MainMenuController.OnPlayPressed += CountTimesPLayed;
        MainMenuController.OnPlayPressed += TogglePlaythroughTimer;



        UpgradeManager.Instance.OnUpgradeUnlocked += FirstUpgrade;

        

        PlayerExperienceManager.Instance.OnEssenceGained += RecordEssence;
        
    }

    private void Update()
    {
        sesionTime += Time.deltaTime;

        if (startPlaythroughTimer)
        {
            playTime += Time.deltaTime;
        }

        if (startTimer)
        {
            _timeTakenToFinishWave += Time.deltaTime;

            if (WorldManager.Instance.CurrentWorld == WorldState.Normal)
            {
                timeSpentOnNormalWorld += Time.deltaTime;
            }
            else if (WorldManager.Instance.CurrentWorld == WorldState.OtherWorld)
            {
                timeSpentOnOtherWorld += Time.deltaTime;
            }
        }

        if(playTime >= 5 && enableSubscriptions == false)
        {
            ModifierPanelSelection.Instance.onModifierChosenAnalitics += CountModifiers;
            WaveManager.Instance.OnWaveEnded += RecordWaveInfo;
            WaveManager.Instance.OnWaveStarted += StartTimer;
            WaveManager.Instance.OnWaveEnded += StopTimer;

            GameManager.Instance.OnMainMenuPressed += RecordWaveAbandonned;
            GameManager.Instance.OnMainMenuPressed += ReportPlaythrough;
            GameManager.Instance.OnMainMenuPressed += RemoveGameplaySubscriptions;
            GoldManager.Instance.OnGoldEarned += RecordGold;
            enableSubscriptions = true;
            
        }
    }

    public void StartAnalitics()
    {
        AnalyticsService.Instance.StartDataCollection();
        isInitialized = true; // Ahora se inicializa correctamente
        Debug.Log("Analytics inicializado correctamente");
        DontShowWarning();
    }

    private void OnApplicationQuit() // Sesion info sender
    {
        ReportModifiers();
        ReportNemeses();
        SesionReport();
        ReportWorldInfo();
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


    public void RecordWaveInfo()
    {
        if (Core.Instance.CurrentHealth > 0)
        {
            WaveInformation waveInformation = new WaveInformation()
            {
                WaveNumber = WaveManager.Instance.CurrentWave,
                TimeTakenToFinishWave = _timeTakenToFinishWave,
                Deaths = waveDeaths,
                Completed = true,

            };
            AnalyticsService.Instance.RecordEvent(waveInformation);

            AnalyticsService.Instance.Flush();
            waveDeaths = 0;
            Debug.Log("Wave info sent");
            print("Wave info sent");
        }
        
        _timeTakenToFinishWave = 0;
    }

    public void RecordWaveAbandonned()
    {
        WaveInformation waveInformation = new WaveInformation()
        {
            WaveNumber = _waveNumber,
            TimeTakenToFinishWave = _timeTakenToFinishWave,
            Deaths = waveDeaths,
            Completed = false,

        };
        AnalyticsService.Instance.RecordEvent(waveInformation);
        
        waveDeaths = 0;
        _timeTakenToFinishWave = 0;
    }

    private void GetWorldSwitch(WorldState world)
    {
        numberOfTimesSwitched++;
    }

    private void GetDeaths()
    {
        numberOfDeaths++;
        waveDeaths++;
    }

    private void CountModifiers(IGameModifier modifier)
    {
        if (modsChosen.ContainsKey(modifier.Name))
        {
            modsChosen[modifier.Name]++;
        }
        else
        {
            modsChosen.Add(modifier.Name, 1);
        }
    }
    public void RecordNemeses(EnemyType enemy)
    {
        string type = enemy.ToString();
        if (nemeses.ContainsKey(type))
        {
            nemeses[type]++;
        }
        else
        {
            nemeses.Add(type, 1);
        }
    }

    private void ReportModifiers()
    {
        foreach (var modifier in modsChosen)
        {
            ModifierAnalytics modifierAnalytics = new ModifierAnalytics();
            modifierAnalytics.Name = modifier.Key;
            modifierAnalytics.TimesChosen = modifier.Value;

            AnalyticsService.Instance.RecordEvent(modifierAnalytics);

            AnalyticsService.Instance.Flush();
        }
    }

    private void ReportNemeses()
    {
        foreach (var enemy in nemeses)
        {
            NemesisAnalitics nemesisAnalitics = new NemesisAnalitics();
            nemesisAnalitics.Name = enemy.Key;
            nemesisAnalitics.Kills = enemy.Value;

            AnalyticsService.Instance.RecordEvent(nemesisAnalitics);

            AnalyticsService.Instance.Flush();
        }
    }

    private void StartTimer(int a, int b)
    {
        startTimer = true;
    }

    private void StopTimer()
    {
        startTimer = false;
    }

    private void CountTimesPLayed()
    {
        timesPLayed++;
    }

    private void SesionReport()
    {
        SesionInformation sesionInformation = new SesionInformation();       
        sesionInformation.WorldSwitchAmmount = numberOfTimesSwitched;
        sesionInformation.Deaths = numberOfDeaths;
        sesionInformation.TimesPlayed = timesPLayed;
        sesionInformation.SesionTime = sesionTime;

        AnalyticsService.Instance.RecordEvent(sesionInformation);

        AnalyticsService.Instance.Flush();
    }

    private void FirstUpgrade(UpgradeData upgrade)
    {
        if(upgradeUnlocked == false)
        {
            FirstUpgradeAnalitycs upgradeEvent = new FirstUpgradeAnalitycs();
            upgradeEvent.Deaths = numberOfDeaths;
            upgradeEvent.UpgradeName = upgrade.upgradeName;

            AnalyticsService.Instance.RecordEvent(upgradeEvent);

            AnalyticsService.Instance.Flush();
        }
        upgradeUnlocked = true;
    }

    private void TogglePlaythroughTimer()
    {
        startPlaythroughTimer = !startPlaythroughTimer;
        AnalyticsService.Instance.Flush();
    }

    private void ReportPlaythrough()
    {
        PlaythroughInfo playthroughInfo = new PlaythroughInfo();
        playthroughInfo.NormalEscence = normalEscence;
        playthroughInfo.OtherEscence = OtherEscence;
        playthroughInfo.Gold = gold;
        playthroughInfo.Duration = playTime;

        AnalyticsService.Instance.RecordEvent(playthroughInfo);
        AnalyticsService.Instance.Flush();

        normalEscence = 0;
        OtherEscence = 0;
        gold = 0;
        playTime = 0;
    }

    private void RecordEssence(WorldState world, int amount)
    {
        if(world == WorldState.Normal)
        {
            normalEscence += amount;
        }
        else
        {
            OtherEscence += amount;
        }
    }

    private void RecordGold(int amount)
    {
        gold += amount;
    }

    private void ReportWorldInfo()
    {
        WorldAnalitics worldAnalitics = new WorldAnalitics();
        worldAnalitics.World = "NormalWorld";
        worldAnalitics.Time = timeSpentOnNormalWorld;

        AnalyticsService.Instance.RecordEvent(worldAnalitics);

        AnalyticsService.Instance.Flush();

        WorldAnalitics worldAnalitics2 = new WorldAnalitics();
        worldAnalitics2.World = "OtherWorld";
        worldAnalitics2.Time = timeSpentOnOtherWorld;

        AnalyticsService.Instance.RecordEvent(worldAnalitics2);

        AnalyticsService.Instance.Flush();
    }

    public void DontShowWarning()
    {
        warning.gameObject.SetActive(false);
    }

    private void RemoveGameplaySubscriptions()
    {
        //ModifierPanelSelection.Instance.onModifierChosenAnalitics -= CountModifiers;
        //WaveManager.Instance.OnWaveEnded -= RecordWaveInfo;
        //WaveManager.Instance.OnWaveStarted -= StartTimer;
        //WaveManager.Instance.OnWaveEnded -= StopTimer;

        //GameManager.Instance.OnMainMenuPressed -= RecordWaveAbandonned;        
        enableSubscriptions = false;        
        //GameManager.Instance.OnMainMenuPressed -= ReportPlaythrough;
        //GoldManager.Instance.OnGoldEarned -= RecordGold;
        //GameManager.Instance.OnMainMenuPressed -= RemoveGameplaySubscriptions;
    }

}
