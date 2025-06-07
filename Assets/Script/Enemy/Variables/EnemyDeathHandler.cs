using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour, IEnemyDeathHandler
{
    public void OnEnemyDeath(Enemy enemy)
    {
        if (WaveManager.Instance != null)
            WaveManager.Instance.NotifyEnemyKilled();

        if (EnemyPool.Instance != null)
            EnemyPool.Instance.ReturnEnemy(enemy.Type, enemy.gameObject); // Usá enum

        // Nueva lógica: sumar experiencia si es Boss o MiniBoss
        if (enemy.Type == EnemyType.Boss)
        {
            PlayerExperienceManager.Instance.OnBossKilled();
        }
        else if (enemy.Type == EnemyType.MiniBoss)
        {
            PlayerExperienceManager.Instance.OnMiniBossKilled();
        }

        enemy.NotifyDeath();

    }
}
