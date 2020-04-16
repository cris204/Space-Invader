using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShoot : ShootController
{
    public GameObject muzzle;
    public override void Shoot()
    {
        base.Shoot();

        if (this.canShoot) {
            GameObject bullet;
            bullet = PoolManager.Instance.GetObject(this.bulletPath);
            bullet.GetComponent<Bullet>().Shoot(muzzle.transform.position);
            PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_LASER, 0.06f);
            if (this.waitToCanShoot == null) {
                this.waitToCanShoot = StartCoroutine(WaitToCanShoot());
            }
        }

    }
}
