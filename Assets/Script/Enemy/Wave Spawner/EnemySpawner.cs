using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs del mundo normal")]
    [SerializeField] private GameObject heavyEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;
    [SerializeField] private GameObject miniBossPrefab;
    [SerializeField] private GameObject bossPrefab;


    private Dictionary<EnemyType, GameObject> enemyPrefabs;

    private void Awake()
    {
        enemyPrefabs = new Dictionary<EnemyType, GameObject>
        {
            { EnemyType.Heavy, heavyEnemyPrefab },
            { EnemyType.Fast, fastEnemyPrefab },
            { EnemyType.MiniBoss, miniBossPrefab },
            { EnemyType.Boss, bossPrefab }
        };
    }

    public Enemy SpawnEnemy(Vector3 spawnPosition, Vector3[] path, EnemyType type)
    {
        if (!enemyPrefabs.ContainsKey(type))
        {
            Debug.LogError($"[EnemySpawner] No hay prefab registrado para el tipo: {type}");
            return null;
        }

        GameObject enemyGO = EnemyPool.Instance.GetEnemy(type); // <- Usá el enum
        enemyGO.transform.position = spawnPosition;

        Enemy enemy = enemyGO.GetComponent<Enemy>();

        // Inicialización completa
        enemy.InitializePath(path, Core.Instance.gameObject, GridManager.Instance);
        enemy.SetOriginWorld(WorldManager.Instance.CurrentWorld);
        enemy.WorldLogic.UpdateVisibility();

        return enemy;
    }
}
