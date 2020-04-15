using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject principalMenu;
    [SerializeField] private GameObject changeShipContainer;


    #region ButtonEvents
    public void StartGame()
    {
        SceneLoaderManager.Instance.LoadScene(Env.GAME_SCENE);
    }

    public void ChangeShip()
    {
        this.principalMenu.SetActive(false);
        this.changeShipContainer.SetActive(true);
    }

    public void BackFromChangeShip()
    {
        this.principalMenu.SetActive(true);
        this.changeShipContainer.SetActive(false);
    }
    #endregion

    public void Quit()
    {
        Application.Quit();
    }

}
