using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontal;
    private float vertical;
    public float speed;
    public GameObject muzzle;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        this.Move();
        if (Input.GetKeyDown(KeyCode.Space)) {
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
        GameObject bullet;
        bullet = PoolManager.Instance.GetObject(Env.BULLET_PATH);
        bullet.GetComponent<Bullet>().Shoot(muzzle.transform.position);
    }


}
