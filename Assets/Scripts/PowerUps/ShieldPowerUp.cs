using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    private Vector2 _startPosition;
    private Rigidbody2D rb;
    private float speedY = -50;
    private float speedX = 100;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.SetStartPosition();
    }

    public void SetStartPosition()
    {
        _startPosition = transform.position;
    }

    void FixedUpdate()
    {
        rb.velocity = _startPosition + new Vector2(Mathf.Sin(Time.time)* speedX * Time.fixedDeltaTime, speedY * Time.fixedDeltaTime);
    }
}
