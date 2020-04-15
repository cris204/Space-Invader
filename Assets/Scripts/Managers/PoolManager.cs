using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{

    private Dictionary<string,List<GameObject>> objectsPool = new Dictionary<string,List<GameObject>>(); 

    public GameObject GetObject(string path)
    {
        if (!objectsPool.ContainsKey(path)) {
            objectsPool.Add(path, new List<GameObject>());
        }
     
        if (objectsPool[path].Count == 0)
            AddObject(path);

        return AllocateObject(path);
    }

    public void ReleaseObject(string path, GameObject prefab)
    {
        prefab.gameObject.SetActive(false);

        if (!objectsPool.ContainsKey(path)) {
            objectsPool.Add(path, new List<GameObject>());
        }

        objectsPool[path].Add(prefab);
    }

    private void AddObject(string path)
    {
        GameObject instance = Instantiate(ResourceManager.Instance.GetGameObject(path), transform);
        instance.transform.position = this.transform.position;
        instance.gameObject.SetActive(false);
        objectsPool[path].Add(instance);
    }

    private GameObject AllocateObject(string path)
    {
        GameObject obstacle = objectsPool[path][0];
        objectsPool[path].RemoveAt(0);
        obstacle.gameObject.SetActive(true);
        return obstacle;
    }
}
