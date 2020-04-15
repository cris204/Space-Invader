using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontal;
    public float speed;
    public GameObject muzzle;
    private SpriteRenderer sprRenderer;

    [Header("PowerUps")]
    private bool hasShield;
    private GameObject shieldGO;

    [Header("Shoot")]
    private float shootDelay = 0.5f;
    private Coroutine  waitToCanShoot;
    private bool canShoot=true;

    void Start()
    {
        this.sprRenderer = GetComponent<SpriteRenderer>();
        this.rb = this.GetComponent<Rigidbody2D>();
        this.sprRenderer.sprite = ResourceManager.Instance.GetSprite(StorageManager.Instance.GetString(Env.CURRENT_SHIP_KEY, "playerShip1_blue.png"));
        EventManager.Instance.AddListener<LostShieldEvent>(this.OnLostShieldEvent);
    }

    void Update()
    {
        this.MoveInput();
        this.ShootInput();
    }

    private void FixedUpdate()
    {
        this.Move();
    }

    #region Move

    private void MoveInput()
    {
        this.horizontal = Input.GetAxis("Horizontal");
        this.transform.rotation = Quaternion.Euler(new Vector2(0, horizontal * 40));
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontal * speed, 0);
    }
    #endregion

    #region shoot

    private void ShootInput()
    {
        if (Input.GetButton("Shoot")) {
            this.Shoot();
        }
    }

    private void Shoot()
    {
        if (this.canShoot) {
            GameObject bullet;
            bullet = PoolManager.Instance.GetObject(Env.BULLET_PATH);
            bullet.GetComponent<Bullet>().Shoot(muzzle.transform.position);
            PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_LASER,0.06f);
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
    #endregion

    private void AddShield()
    {
        this.hasShield = true;
        this.shieldGO = PoolManager.Instance.GetObject(Env.SHIELD);
        this.shieldGO.transform.SetParent(this.transform);
        this.shieldGO.transform.localPosition = Vector2.zero;
    }


    #region Events

    private void OnLostShieldEvent(LostShieldEvent e)
    {
        PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_SHIELD_DOWN,0.2f);
        this.hasShield = false;
        this.shieldGO = null;
    }

    #endregion


    #region Collision

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ShieldPowerUp") {
            PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_SHIELD_UP);
            PoolManager.Instance.ReleaseObject(Env.SHIELD_POWER, collision.gameObject);
            this.AddShield();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !this.hasShield) {

            PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_LOSE, 0.8f);
            this.gameObject.SetActive(false);
            EventManager.Instance.TriggerEvent(new EndGameEvent());

        }
    }

    #endregion
    private void OnDestroy()
    {
        if (EventManager.Instance != null) {
            EventManager.Instance.RemoveListener<LostShieldEvent>(this.OnLostShieldEvent);
        }
    }

}
