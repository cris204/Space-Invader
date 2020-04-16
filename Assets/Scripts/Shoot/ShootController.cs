using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [Header("Shoot")]
    protected float shootDelay = 0.2f;
    protected Coroutine waitToCanShoot;
    protected bool canShoot = true;

    public virtual void Shoot()
    {

    }

    protected IEnumerator WaitToCanShoot()
    {
        this.canShoot = false;
        yield return new WaitForSeconds(this.shootDelay);
        this.canShoot = true;
        this.waitToCanShoot = null;
    }

}
