﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private string destroyVfxPath;

    [Header("Move")]
    private Vector2 spawnPosition;
    private float speedMultiply = 1;
    private float speedX = 100;
    private float speedXDirection;
    private float speedY = -100;
    private float distance = 0.3f;
    private Vector2 speedDirection;

    [Header("Sides checker")]
    public Transform rightRayStartPos;
    public Transform leftRayStartPos;
    private RaycastHit2D rightHit;
    private RaycastHit2D leftHit;
    public LayerMask layerMask;
    private bool canChangeDirection=true;
    private float timeToChangeDirection = 0.15f;
    private float currentTime;

    void Awake()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.speedX *= Random.Range(-1, 2);
    }
    private void Start()
    {
        EventManager.Instance.AddListener<DiffcultLevelUpEvent>(this.OnLevelUp);
        EventManager.Instance.AddListener<EndGameEvent>(this.OnEndGame);
        EventManager.Instance.AddListener<ReturnToMenuEvent>(this.OnReturnToMenu);
    }

    private void OnEnable()
    {
        this.destroyVfxPath = ResourceManager.Instance.GetBulletVFXPath(GameManager.Instance.GetPlayerShip());
        this.spawnPosition = this.transform.position;
    }

    private void Update()
    {
        if (this.currentTime > this.timeToChangeDirection) {
            this.ChangeDirection(false);
            this.currentTime = 0;
        }
        this.currentTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        this.RaysChecker();
        this.Move();
    }

    #region Events

    private void OnLevelUp(DiffcultLevelUpEvent e)
    {
        this.speedMultiply *= Env.SPEED_MULTIPLIER;
    }

    private void OnEndGame(EndGameEvent e)
    {
        this.speedX = 100;
        this.speedY = -100;
        this.speedMultiply = 1;
        if (this.gameObject.activeInHierarchy) {
            this.DestroyEnemy();
        }
    }

    private void OnReturnToMenu(ReturnToMenuEvent e)
    {
        PoolManager.Instance.ReleaseObject(Env.ENEMY_PATH, this.gameObject);
    }

    #endregion

    #region Move

    private void Move()
    {
        this.speedDirection.x = (speedXDirection * speedX * speedMultiply) * Time.fixedDeltaTime;
        this.speedDirection.y = (speedY * speedMultiply) * Time.fixedDeltaTime;
        rb.velocity = this.speedDirection;
    }
    private void RaysChecker()
    {
        this.rightHit = Physics2D.Raycast(rightRayStartPos.position, Vector2.right, distance, layerMask);
        this.leftHit = Physics2D.Raycast(leftRayStartPos.position, -Vector2.right, distance, layerMask);
        if (this.rightHit.collider != null) {
            this.ChangeDirection(true);
        }

        if (this.leftHit.collider != null && this.leftHit.collider != this.GetComponent<Collider2D>()) {
            this.ChangeDirection(true);
        }
    }
    public void ChangeDirection(bool needToChangeDirection)
    {
        if (this.canChangeDirection) {
            this.canChangeDirection = false;
            speedXDirection = needToChangeDirection ? Random.Range(-1, 1) * Env.SPEED_MULTIPLIER : Random.Range(-1, 2)* Env.SPEED_MULTIPLIER;
            StartCoroutine(WaitToChangeItAgain());
        }
    }
    IEnumerator WaitToChangeItAgain()
    {
        yield return new WaitForSeconds(this.timeToChangeDirection);
        this.canChangeDirection = true;
    }

    #endregion

    #region Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet") {
            EventManager.Instance.TriggerEvent(new EnemyWasDestroyedEvent());
            PoolManager.Instance.ReleaseObject(ResourceManager.Instance.GetBulletPath(GameManager.Instance.GetPlayerShip()), collision.gameObject);
            PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_TWO_TONE);
            this.DestroyEnemy();
        } else if (collision.tag == "DownLimit") {
            GameManager.Instance.DecreaseCurrentEnemies();
            PoolManager.Instance.ReleaseObject(Env.ENEMY_PATH, this.gameObject);
            EventManager.Instance.TriggerEvent(new SpawnEnemiesEvent
            {
                enemiesQuantity = 1
            });
        }
    }
    #endregion

    private void DestroyEnemy()
    {
        GameObject bulletVfx = PoolManager.Instance.GetObject(this.destroyVfxPath);
        bulletVfx.transform.localPosition = this.transform.localPosition;
        PoolManager.Instance.ReleaseObject(Env.ENEMY_PATH, this.gameObject);
    }


    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<DiffcultLevelUpEvent>(this.OnLevelUp);
            EventManager.Instance.RemoveListener<EndGameEvent>(this.OnEndGame);
            EventManager.Instance.RemoveListener<ReturnToMenuEvent>(this.OnReturnToMenu);
        }
    }
}
