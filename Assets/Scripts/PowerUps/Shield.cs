using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void Start()
    {
        this.transform.rotation = Quaternion.Euler(Vector2.zero);
        EventManager.Instance.AddListener<ReturnToMenuEvent>(this.OnReturnToMenu);
    }
    private void Update()
    {
        this.transform.rotation = Quaternion.identity;
    }

#region Events
    private void OnReturnToMenu(ReturnToMenuEvent e)
    {
        PoolManager.Instance.ReleaseObject(Env.ENEMY_PATH, this.gameObject);
    }

#endregion


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") {
            EventManager.Instance.TriggerEvent(new LostShieldEvent());
            PoolManager.Instance.ReleaseObject(Env.ENEMY_PATH, collision.gameObject);
            PoolManager.Instance.ReleaseObject(Env.SHIELD, this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<ReturnToMenuEvent>(this.OnReturnToMenu);
        }
    }

}
