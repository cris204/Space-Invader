using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    public void ReturnObjectToPool()
    {
        PoolManager.Instance.ReleaseObject(ResourceManager.Instance.GetBulletVFXPath(GameManager.Instance.GetPlayerShip()), this.gameObject);
    }
}
