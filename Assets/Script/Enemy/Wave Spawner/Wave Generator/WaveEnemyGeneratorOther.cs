using System.Collections;
using UnityEngine;

public class WaveEnemyGeneratorOther : WaveEnemyGeneratorBase
{
    [SerializeField] private int startingWave = 6;
    public int StartingWave => startingWave;

    protected override WorldState TargetWorld => WorldState.OtherWorld;

    protected override EnemyType GetEnemyType()
    {
        return Random.value < 0.4f ? EnemyType.Fast : EnemyType.Heavy;
    }

    protected override int GetEnemyCount(int baseCount)
    {
        float multiplier = GameModifiersManager.Instance != null ? GameModifiersManager.Instance.enemyCountMultiplier : 1f;

        return Mathf.Max(1, Mathf.RoundToInt(baseCount * multiplier));
    }
}
