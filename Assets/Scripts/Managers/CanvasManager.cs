using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsTxt;

    [Header("ChangeShip"), SerializeField]
    private GameObject changeShipContainer;
    [Header("EndGame"), SerializeField]
    private GameObject endGameContainer;
    [SerializeField] private TextMeshProUGUI pointsEndGameTxt;
    [SerializeField] private TextMeshProUGUI highScorePointsEndGameTxt;
    [SerializeField] private Image spaceShip;

    void Start()
    {
        EventManager.Instance.AddListener<EndGameEvent>(this.OnEndGame);
        EventManager.Instance.AddListener<UpdatePointsEvent>(this.OnEnemyWasDestroyed);
    }

    public void PlayAgain()
    {
        SceneLoaderManager.Instance.LoadScene(Env.GAME_SCENE);
    }

    public void ChangeShip()
    {
        this.endGameContainer.SetActive(false);
        this.changeShipContainer.SetActive(true);
    }


    #region Events
    private void OnEndGame(EndGameEvent e)
    {
        this.endGameContainer.SetActive(true);
        this.pointsEndGameTxt.text = string.Format("Points {0}", GameManager.Instance.GetPoints());
        this.highScorePointsEndGameTxt.text = string.Format("HighScore {0}", GameManager.Instance.GetHighScore());
        this.spaceShip.sprite= ResourceManager.Instance.GetSprite(StorageManager.Instance.GetString(Env.CURRENT_SHIP_KEY, "playerShip1_blue.png"));

    }
    private void OnEnemyWasDestroyed(UpdatePointsEvent e)
    {
        pointsTxt.text = string.Format("Points {0}", GameManager.Instance.GetPoints());
    }


    #endregion
    private void OnDestroy()
    {
        if (EventManager.Instance != null) {
            EventManager.Instance.RemoveListener<EndGameEvent>(this.OnEndGame);
            EventManager.Instance.RemoveListener<UpdatePointsEvent>(this.OnEnemyWasDestroyed);
        }
    }


}
