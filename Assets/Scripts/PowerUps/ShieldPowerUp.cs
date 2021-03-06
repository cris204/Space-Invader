﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    private Vector2 speedVector;
    private Vector2 startPosition;
    private Rigidbody2D rb;
    public float speedY = -50;
    private float speedX = 1;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        EventManager.Instance.AddListener<ReturnToMenuEvent>(this.OnReturnToMenu);
    }

    private void OnEnable()
    {
        this.SetStartPosition();
    }

    public void SetStartPosition()
    {
        startPosition = transform.position;
           speedVector = transform.position;
    }

    void FixedUpdate()
    {
        speedVector.x = Mathf.Sin(Time.time);
        speedVector.y = speedY * Time.fixedDeltaTime;
        rb.velocity =  new Vector3 (speedVector.x, speedVector.y, 0);
    }
    #region Events
    private void OnReturnToMenu(ReturnToMenuEvent e)
    {
        PoolManager.Instance.ReleaseObject(Env.ENEMY_PATH, this.gameObject);
    }

    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DownLimit") {
            PoolManager.Instance.ReleaseObject(Env.SHIELD_POWER,this.gameObject);
        }
    }
    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<ReturnToMenuEvent>(this.OnReturnToMenu);
        }
    }

}
