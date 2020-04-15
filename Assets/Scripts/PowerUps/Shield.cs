using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void Start()
    {
        this.transform.rotation = Quaternion.Euler(Vector2.zero);
    }
    private void Update()
    {
        this.transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") {
            EventManager.Instance.TriggerEvent(new LostShieldEvent());
            PoolManager.Instance.ReleaseObject(Env.ENEMY_PATH, collision.gameObject);
            PoolManager.Instance.ReleaseObject(Env.SHIELD, this.gameObject);
        }
    }
}
