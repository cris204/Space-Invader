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

    [Header("General")]
    private bool playerIsAlive=true;
    private bool isPaused=false;

    [Header("EnemySpawn")]
    private float timeToSpawnEnemies = 5f;
    private float currentTimeToSpawnEnemies = 4;
    private int enemiesToSpawn = 5;
    private int currentEnemies;

    [Header("Score")]
    private int highScore;
    private int points;

    #region Get&Set
    public bool GetIsPaused()
    {
        return this.isPaused;
    }
    public int GetCurrentEnemies()
    {
        return this.currentEnemies;
    }
    public void PlusCurrentEnemies()
    {
        this.currentEnemies++;
    }
    public void DecreaseCurrentEnemies()
    {
        this.currentEnemies--;
    }
    public int GetPoints()
    {
        return this.points;
    }
    public int GetHighScore()
    {
        return this.highScore;
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
        EventManager.Instance.AddListener<TogglePauseEvent>(this.OnTogglePause);
        this.CheckSpawnEnemy(true);
        this.highScore = StorageManager.Instance.GetInt(Env.HIGHSCORE_KEY, 0);
    }

    void Update()
    {
        if (this.playerIsAlive) {
            this.CheckSpawnEnemy();
        }
        this.InputPause();
    }

    public void InputPause()
    {
        if (Input.GetButtonDown("Pause")) {
            EventManager.Instance.TriggerEvent(new TogglePauseEvent
            {
                setPause=!this.isPaused
            });
        }
    }



    #region EnemySpawn

    private void CheckSpawnEnemy(bool forceToSpawn=false)
    {
        if (this.currentTimeToSpawnEnemies > this.timeToSpawnEnemies || forceToSpawn) {
            this.currentTimeToSpawnEnemies = 0;
          //  this.currentEnemies += enemiesToSpawn;
            if (this.currentEnemies < 80) {
                if(this.currentEnemies + enemiesToSpawn > 80) {
                    this.enemiesToSpawn = 80 - this.currentEnemies;
                }
                EventManager.Instance.TriggerEvent(new SpawnEnemiesEvent
                {
                    enemiesQuantity = this.enemiesToSpawn
                });
            }
        }
        this.currentTimeToSpawnEnemies += Time.deltaTime;
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
        this.CheckHighScore();
        EventManager.Instance.TriggerEvent(new UpdatePointsEvent());

        if (points % 10 == 0) {

            if (this.playerIsAlive) {
                EventManager.Instance.TriggerEvent(new SpawnPowerUpEvent());
            }

            EventManager.Instance.TriggerEvent(new DiffcultLevelUpEvent());
            if (this.enemiesToSpawn < 20) {
                this.enemiesToSpawn += 2;
            }
            if (this.timeToSpawnEnemies > 2) {
                this.timeToSpawnEnemies -= 0.5f;
            }
        }
        this.currentEnemies--;
        if (this.currentEnemies <= 0) {
            this.CheckSpawnEnemy(true);
        }
    }

    private void OnTogglePause(TogglePauseEvent e)
    {
        this.isPaused = e.setPause;
        if (this.isPaused) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }
    #endregion

    private void CheckHighScore()
    {
        if (this.highScore < this.points) {
            this.highScore = this.points;
            StorageManager.Instance.SetInt(Env.HIGHSCORE_KEY, highScore);
        }

    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<EndGameEvent>(this.OnEndGame);
            EventManager.Instance.RemoveListener<EnemyWasDestroyedEvent>(this.OnEnemyWasDestroyed);
            EventManager.Instance.RemoveListener<TogglePauseEvent>(this.OnTogglePause);
        }
    }
}
