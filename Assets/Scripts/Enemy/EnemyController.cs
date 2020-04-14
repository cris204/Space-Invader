using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Move")]
    private Vector2 spawnPosition;
    private float speedMultiply = 1;
    [SerializeField]
    private float speedX;
    private float speedXDirection;
    [SerializeField]
    private float speedY;
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
    }

    private void OnEnable()
    {
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
        this.DestroyEnemy();
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
            PoolManager.Instance.ReleaseObject(Env.BULLET_PATH, collision.gameObject);
            this.DestroyEnemy();
        } else if (collision.tag == "DownLimit") {
            this.transform.position = this.spawnPosition;
        }
    }
    #endregion

    private void DestroyEnemy()
    {
        GameObject bulletVfx = PoolManager.Instance.GetObject(Env.BULLET_VFX_PATH);
        bulletVfx.transform.localPosition = this.transform.localPosition;
        PoolManager.Instance.ReleaseObject(Env.ENEMY_PATH, this.gameObject);
    }


    private void OnDestroy()
    {
        if (EventManager.Instance != null) {
            EventManager.Instance.RemoveListener<DiffcultLevelUpEvent>(this.OnLevelUp);
            EventManager.Instance.RemoveListener<EndGameEvent>(this.OnEndGame);
        }
    }
}
