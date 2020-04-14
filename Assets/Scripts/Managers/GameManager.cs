using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int points;

    [Header("EnemySpawn")]
    private float timeToSpawn = 5f;
    private float currentTimeToSpawn =0;
    private int enemiesToSpawn = 5;
    private int currentEnemies;

    #region Get&Set

    public int GetCurrentEnemies()
    {
        return this.currentEnemies;
    }

    public void SetCurrentEnemies(int value)
    {
        this.currentEnemies = value;
    }

    #endregion


    void Start()
    {
        EventManager.Instance.AddListener<EndGameEvent>(this.OnEndGame);
        EventManager.Instance.AddListener<EnemyWasDestroyedEvent>(this.OnEnemyWasDestroyed);
    }

    void Update()
    {
        this.CheckSpawnEnemy();
    }

    #region EnemySpawn

    private void CheckSpawnEnemy()
    {
        if (this.currentTimeToSpawn > this.timeToSpawn) {
            this.currentTimeToSpawn = 0;
            this.currentEnemies += enemiesToSpawn;
            EventManager.Instance.TriggerEvent(new SpawnEnemiesEvent
            {
                enemiesQuantity = enemiesToSpawn
            });
        }
        this.currentTimeToSpawn += Time.deltaTime;
    }


    #endregion


    #region Events
    private void OnEndGame(EndGameEvent e)
    {
        Debug.LogError("Finish");
    }
    private void OnEnemyWasDestroyed(EnemyWasDestroyedEvent e)
    {
        this.points++;
    }


    #endregion

    private void OnDestroy()
    {
        if (EventManager.Instance != null) {
            EventManager.Instance.RemoveListener<EndGameEvent>(this.OnEndGame);
            EventManager.Instance.RemoveListener<EnemyWasDestroyedEvent>(this.OnEnemyWasDestroyed);
        }
    }
}
