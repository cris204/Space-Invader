using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private float bulletSpeed = 10;
    public Ship currentShip;
    private string bulletPath;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        EventManager.Instance.AddListener<ReturnToMenuEvent>(this.OnReturnToMenu);
    }

    private void OnEnable()
    {
        this.bulletPath = ResourceManager.Instance.GetBulletPath(this.currentShip);
    }

    public void Shoot(Vector2 startPosition)
    {
        this.transform.position = startPosition;
        rb.velocity = new Vector2(0, bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MapLimits") {
            PoolManager.Instance.ReleaseObject(this.bulletPath, this.gameObject);
        }
    }

    #region Events
    private void OnReturnToMenu(ReturnToMenuEvent e)
    {
        PoolManager.Instance.ReleaseObject(this.bulletPath, this.gameObject);
    }

    #endregion

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<ReturnToMenuEvent>(this.OnReturnToMenu);
        }
    }

}
