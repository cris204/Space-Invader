using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public Vector2 size;
    private Vector2 spawnPos;
    private void Start()
    {
        EventManager.Instance.AddListener<SpawnPowerUpEvent>(this.OnSpawnPowerUpEvent);
    }

    public void OnSpawnPowerUpEvent(SpawnPowerUpEvent e)
    {
       this.SpawnPowerUp();
    }

    public void SpawnPowerUp()
    {
        string powerUpToSpawn="";
        switch (Random.Range(0,Env.POWER_UPS_COUNT)) {
            case 0:
                powerUpToSpawn = Env.SHIELD_POWER;
                break;
        }

        if (!string.IsNullOrEmpty(powerUpToSpawn)){
            spawnPos = new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(this.transform.position.y - size.y / 2, this.transform.position.y + size.y / 2));
            Instantiate(ResourceManager.Instance.GetGameObject(powerUpToSpawn), spawnPos, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<SpawnPowerUpEvent>(this.OnSpawnPowerUpEvent);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.localPosition, size);
    }
}
