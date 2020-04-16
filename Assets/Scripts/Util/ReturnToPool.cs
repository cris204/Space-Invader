using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    public void ReturnObjectToPool()
    {
        PoolManager.Instance.ReleaseObject(Env.BULLET_BLUE_VFX_PATH, this.gameObject);
    }
}
