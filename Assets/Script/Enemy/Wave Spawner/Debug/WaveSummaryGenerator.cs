using UnityEngine;
using System.Collections.Generic;

public class WaveSummaryGenerator : MonoBehaviour
{
    [SerializeField] private int totalWaves = 45;
    [SerializeField] private int baseEnemies = 3;
    [SerializeField] private int scalingPerWave = 4;
    [SerializeField] private int extraEnemiesPer5 = 1;
    [SerializeField] private int extraEnemiesPer15 = 1;
    [SerializeField] private int bonusOtherWorldPerWave = 2;

    private enum World { Normal, Other }

    private void Start()
    {
        GenerateSummary();
    }

    private void GenerateSummary()
    {
        int consecutiveOtherWorld = 0;
        World currentWorld = World.Normal;


        for (int wave = 1; wave <= totalWaves; wave++)
        {
            // Simular cambio de mundo
            currentWorld = wave >= 5 && wave % 2 == 1 ? World.Other : World.Normal;
            if (currentWorld == World.Other)
                consecutiveOtherWorld++;
            else
                consecutiveOtherWorld = 0;

            int extra = 0;
            if (wave % 15 == 0) extra += extraEnemiesPer15;
            else if (wave % 5 == 0) extra += extraEnemiesPer5;

            int totalEnemies = baseEnemies + (wave - 1) * scalingPerWave + extra + (consecutiveOtherWorld * bonusOtherWorldPerWave);

            int heavyCount = Mathf.CeilToInt(totalEnemies * 0.5f);
            int fastCount = totalEnemies - heavyCount;
            int miniBoss = wave % 5 == 0 && wave % 15 != 0 ? 1 : 0;
            int boss = wave % 15 == 0 ? 1 : 0;

            int totalNormal = currentWorld == World.Normal ? totalEnemies + miniBoss + boss : 0;
            int totalOther = currentWorld == World.Other ? totalEnemies + miniBoss + boss : 0;

            //Debug.Log($"  Oleada {wave,2}       H:{heavyCount}  F:{fastCount} /MB:{miniBoss}  B:{boss}        N:{totalNormal}  OW:{totalOther}                  ");
        }

        //Debug.Log("");
    }
}
