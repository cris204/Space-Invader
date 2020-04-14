using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{

    public Vector2 size;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        Vector2 pos = new Vector3(Random.Range(-size.x / 2, size.x / 2),this.transform.position.y);
        GameObject enemySpawned = Instantiate(ResourceManager.Instance.GetGameObject(Env.ENEMY_PATH), pos, Quaternion.identity);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.localPosition, size);
    }

}
