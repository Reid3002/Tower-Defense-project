using Unity.Services.Core;
using UnityEngine;

public class WaveInformation : Unity.Services.Analytics.Event
{
    public WaveInformation(): base("WaveInformation") 
    {

    }

    public int WaveNumber { set { SetParameter("WaveNumber", value); } }

    public string ModifierForThisWave { set { SetParameter("ModifierForThisWave", value); } }

    public int EnemiesSpawned { set { SetParameter("EnemiesSpawned", value); } }
    public int EnemiesDefeated { set { SetParameter("EnemiesDefeated", value); } }

    public string EnemyType1 { set { SetParameter("EnemyType1", value); } }

    public int EnemiesType1Spawned { set { SetParameter("EnemiesType1Spawned", value); } }

    public string EnemyType2 { set { SetParameter("EnemyType1", value); } }

    public int EnemiesType2Spawned { set { SetParameter("EnemiesType1Spawned", value); } }

    public string EnemyType3 { set { SetParameter("EnemyType1", value); } }

    public int EnemiesType3Spawned { set { SetParameter("EnemiesType1Spawned", value); } }


    public string MostSpawnedEnemy { set { SetParameter("MostSpawnedEnemy", value); } }


    public int GoldEarned { set { SetParameter("GoldEarned", value); } }

    public float TimeTakenToFinishWave { set { SetParameter("TimeTakenToFinishWave", value); } }





}
