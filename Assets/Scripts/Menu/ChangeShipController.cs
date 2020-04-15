using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShipController : MonoBehaviour
{
    [SerializeField] private Image shipImage;

    //Called with Button
    public void ChangeShip(string path)
    {
        this.shipImage.sprite = ResourceManager.Instance.GetSprite(path);
        StorageManager.Instance.SetString(Env.CURRENT_SHIP_KEY, path);
    }

}
