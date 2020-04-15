using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private float bulletSpeed = 10;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        EventManager.Instance.AddListener<ReturnToMenuEvent>(this.OnReturnToMenu);
    }

    public void Shoot(Vector2 startPosition)
    {
        this.transform.position = startPosition;
        rb.velocity = new Vector2(0, bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MapLimits") {
            PoolManager.Instance.ReleaseObject(Env.BULLET_PATH, this.gameObject);
        }
    }

    #region Events
    private void OnReturnToMenu(ReturnToMenuEvent e)
    {
        PoolManager.Instance.ReleaseObject(Env.BULLET_PATH, this.gameObject);
    }

    #endregion

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<ReturnToMenuEvent>(this.OnReturnToMenu);
        }
    }

}
