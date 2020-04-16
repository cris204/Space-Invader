using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleShoot : ShootController
{
    public GameObject[] muzzles;
    public override void Shoot()
    {
        base.Shoot();

        if (this.canShoot) {
            for (int i = 0; i < muzzles.Length; i++) {
                GameObject bullet;
                bullet = PoolManager.Instance.GetObject(Env.BULLET_BLUE_PATH);
                bullet.GetComponent<Bullet>().Shoot(muzzles[i].transform.position);
                PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_LASER, 0.06f);
            }
            if (this.waitToCanShoot == null) {
                this.waitToCanShoot = StartCoroutine(WaitToCanShoot());
            }
        }
    }
}
