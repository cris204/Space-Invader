using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet") {
            GameObject bulletVfx = PoolManager.Instance.GetObject(Env.BULLET_VFX_PATH);
            bulletVfx.transform.localPosition = this.transform.localPosition;
            PoolManager.Instance.ReleaseObject(Env.BULLET_PATH, collision.gameObject);
        }
    }


}
