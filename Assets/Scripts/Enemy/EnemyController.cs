using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float speedX;
    private float speedXMultiply;
    [SerializeField]
    private float speedY;
    private float distance = 0.5f;
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

    private void Move()
    {
        this.speedDirection.x = (speedXMultiply * speedX) * Time.fixedDeltaTime;
        this.speedDirection.y = speedY * Time.fixedDeltaTime;
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
            speedXMultiply = needToChangeDirection ? Random.Range(-1, 1): Random.Range(-1, 2);
            StartCoroutine(WaitToChangeItAgain());
        }
    }


    IEnumerator WaitToChangeItAgain()
    {
        yield return new WaitForSeconds(this.timeToChangeDirection);
        this.canChangeDirection = true;
    }

    #region Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet") {
            GameObject bulletVfx = PoolManager.Instance.GetObject(Env.BULLET_VFX_PATH);
            bulletVfx.transform.localPosition = this.transform.localPosition;
            PoolManager.Instance.ReleaseObject(Env.BULLET_PATH, collision.gameObject);
            PoolManager.Instance.ReleaseObject(Env.ENEMY_PATH, this.gameObject);
        }
    }
    #endregion

}
