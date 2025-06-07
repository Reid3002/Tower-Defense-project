using System.Collections;
using UnityEngine;

public class WaveEnemyGeneratorNormal : WaveEnemyGeneratorBase
{
    protected override WorldState TargetWorld => WorldState.Normal;

    // Ya no usaremos GetEnemyType(), lo definimos dentro del bucle.

    protected override int GetEnemyCount(int baseCount)
    {
        // Sin modificador
        return baseCount;
    }

    public new void Spawn(int waveNumber, int totalEnemies)
    {
        if (WorldManager.Instance.CurrentWorld != TargetWorld) return;
        StartCoroutine(SpawnEnemies(waveNumber, totalEnemies));
    }

    protected override IEnumerator SpawnEnemies(int waveNumber, int totalEnemies)
    {
        Vector3[] path = gridManager.GetPathPositions();
        if (path == null || path.Length == 0)
        {
            Debug.LogWarning($"[{GetType().Name}] El path está vacío.");
            yield break;
        }

        bool miniBossSpawned = false;
        bool bossSpawned = false;

        for (int i = 0; i < totalEnemies; i++)
        {
            EnemyType typeToSpawn;

            // Boss tiene prioridad si ambas condiciones se cumplen
            if (waveNumber % 15 == 0 && !bossSpawned)
            {
                typeToSpawn = EnemyType.Boss;
                bossSpawned = true;
            }
            else if (waveNumber % 5 == 0 && !miniBossSpawned)
            {
                typeToSpawn = EnemyType.MiniBoss;
                miniBossSpawned = true;
            }
            else
            {
                typeToSpawn = (Random.value < 0.5f) ? EnemyType.Fast : EnemyType.Heavy;
            }

            Enemy enemy = enemySpawner.SpawnEnemy(path[^1], path, typeToSpawn);
            if (enemy != null)
            {
                enemy.SetOriginWorld(TargetWorld);
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        Debug.Log($"[{GetType().Name}] Oleada {WaveManager.Instance.GetCurrentWave()} - {TargetWorld} - Generados: {totalEnemies} enemigos.");
    }
}
