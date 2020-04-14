using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontal;
    public float speed;
    public GameObject muzzle;

    [Header("Shoot")]
    private float shootDelay = 0.5f;
    private Coroutine  waitToCanShoot;
    private bool canShoot=true;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        this.Move();
        if (Input.GetButton("Shoot")) {
            this.Shoot();
        }
    }
    private void Move()
    {
        horizontal = Input.GetAxis("Horizontal") ;
        this.transform.rotation=Quaternion.Euler(new Vector2(0, horizontal * 40));
        rb.velocity = new Vector2(horizontal * speed, 0);
    }

    private void Shoot()
    {
        if (this.canShoot) {
            GameObject bullet;
            bullet = PoolManager.Instance.GetObject(Env.BULLET_PATH);
            bullet.GetComponent<Bullet>().Shoot(muzzle.transform.position);
            if (this.waitToCanShoot == null) {
                this.waitToCanShoot = StartCoroutine(WaitToCanShoot());
            }
        }
    }

    private IEnumerator WaitToCanShoot()
    {
        this.canShoot = false;
        yield return new WaitForSeconds(this.shootDelay);
        this.canShoot = true;
        this.waitToCanShoot = null;
    }

    #region Collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            Destroy(this.gameObject);
            EventManager.Instance.TriggerEvent(new EndGameEvent());
        }
    }

    #endregion


}
