using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }


    private bool playerIsAlive=true;
    private int points;

    [Header("EnemySpawn")]
    private float timeToSpawn = 5f;
    private float currentTimeToSpawn = 4;
    private int enemiesToSpawn = 5;
    private int currentEnemies;

    #region Get&Set

    public int GetCurrentEnemies()
    {
        return this.currentEnemies;
    }

    public int GetPoints()
    {
        return this.points;
    }
    public bool GetPlayerIsAlive()
    {
        return this.playerIsAlive;
    }

    #endregion
    private void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }
    }

    void Start()
    {
        EventManager.Instance.AddListener<EndGameEvent>(this.OnEndGame);
        EventManager.Instance.AddListener<EnemyWasDestroyedEvent>(this.OnEnemyWasDestroyed);
        this.CheckSpawnEnemy(true);
    }

    void Update()
    {
        if (this.playerIsAlive) {
            this.CheckSpawnEnemy();
        }
    }

    #region EnemySpawn

    private void CheckSpawnEnemy(bool forceToSpawn=false)
    {
        if (this.currentTimeToSpawn > this.timeToSpawn || forceToSpawn) {
            Debug.LogError("Spawn");
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
        this.playerIsAlive = false;
    }
    private void OnEnemyWasDestroyed(EnemyWasDestroyedEvent e)
    {
        this.points++;
        EventManager.Instance.TriggerEvent(new UpdatePointsEvent());
        if (points % 10 == 0) {
            EventManager.Instance.TriggerEvent(new DiffcultLevelUpEvent());
            if (this.enemiesToSpawn < 20) {
                this.enemiesToSpawn += 2;
            }
            if (this.timeToSpawn > 2) {
                this.timeToSpawn -= 0.5f;
            }
        }
        this.currentEnemies--;
        if (this.currentEnemies <= 0) {
            this.CheckSpawnEnemy(true);
        }
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
