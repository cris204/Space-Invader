using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShipController : MonoBehaviour
{
    [SerializeField] private Image shipImage;

    private void OnEnable()
    {
        this.shipImage.sprite = ResourceManager.Instance.GetShipSprite((Ship)StorageManager.Instance.GetInt(Env.CURRENT_SHIP_KEY, (int)Ship.green));
    }

    #region ButtonEvents
    public void ChangeShip(string currentShip)
    {
        this.shipImage.sprite = ResourceManager.Instance.GetShipSprite(Env.ReturnShipEnum(currentShip));
        StorageManager.Instance.SetInt(Env.CURRENT_SHIP_KEY, (int)Env.ReturnShipEnum(currentShip));
    }

    #endregion

}
