using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeShipController : MonoBehaviour
{
    [SerializeField] private Image shipImage;
    [SerializeField] private Image[] shipButtonImages;
    [SerializeField] private TextMeshProUGUI shipDescriptionTxt;


    private void OnEnable()
    {
        this.shipImage.sprite = ResourceManager.Instance.GetShipSprite((Ship)StorageManager.Instance.GetInt(Env.CURRENT_SHIP_KEY, (int)Ship.green));
        this.ChangeHighlightIcon(StorageManager.Instance.GetInt(Env.CURRENT_SHIP_KEY, 1));
        this.SetDescription();
    }

    #region ButtonEvents
    public void ChangeShip(string currentShip)
    {
        this.shipImage.sprite = ResourceManager.Instance.GetShipSprite(Env.ReturnShipEnum(currentShip));
        StorageManager.Instance.SetInt(Env.CURRENT_SHIP_KEY, (int)Env.ReturnShipEnum(currentShip));
        this.ChangeHighlightIcon((int)Env.ReturnShipEnum(currentShip));

    }
    public void ChangeHighlightIcon(int buttonId)
    {
        for (int i = 0; i < this.shipButtonImages.Length; i++) {
            this.shipButtonImages[i].color = Color.white;
        }
        this.shipButtonImages[buttonId].color = Color.green;
        this.SetDescription();
    }

    #endregion

    public void SetDescription()
    {
      this.shipDescriptionTxt.text= Env.GetShipDescription((Ship)StorageManager.Instance.GetInt(Env.CURRENT_SHIP_KEY, (int)Ship.green));
    }

}
