using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{

    public Vector2 size;
    private Vector2 spawnPos;

    private void Start()
    {
        EventManager.Instance.AddListener<SpawnEnemiesEvent>(this.OnSpawnEnemies);
    }

    public void OnSpawnEnemies(SpawnEnemiesEvent e)
    {
        for (int i = 0; i < e.enemiesQuantity; i++) {
            this.SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        this.spawnPos = new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(this.transform.position.y -size.y / 2, this.transform.position.y + size.y / 2));
        GameObject enemySpawned = PoolManager.Instance.GetObject(Env.ENEMY_PATH);
        enemySpawned.transform.position = this.spawnPos;
        GameManager.Instance.PlusCurrentEnemies();
    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<SpawnEnemiesEvent>(this.OnSpawnEnemies);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.localPosition, size);
    }

}
