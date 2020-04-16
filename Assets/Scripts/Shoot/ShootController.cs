using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    protected Ship currentShip;
    protected string bulletPath;

    [Header("Shoot")]
    public float shootDelay = 0.2f;
    protected Coroutine waitToCanShoot;
    protected bool canShoot = true;

    private void Start()
    {
        this.currentShip = (Ship)StorageManager.Instance.GetInt(Env.CURRENT_SHIP_KEY, (int)Ship.green);
        this.bulletPath = ResourceManager.Instance.GetBulletPath(currentShip);
    }

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
