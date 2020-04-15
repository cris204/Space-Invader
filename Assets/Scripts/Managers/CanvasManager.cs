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

    [Header("Pause"), SerializeField]
    private GameObject pauseContainer;

    void Start()
    {
        EventManager.Instance.AddListener<EndGameEvent>(this.OnEndGame);
        EventManager.Instance.AddListener<UpdatePointsEvent>(this.OnEnemyWasDestroyed);
        EventManager.Instance.AddListener<TogglePauseEvent>(this.OnTogglePauseEvent);
    }



    #region ButtonEvents
    public void Continue()
    {
        EventManager.Instance.TriggerEvent(new TogglePauseEvent {
            setPause=false
        });
    }
    public void GoToMenu()
    {
        EventManager.Instance.TriggerEvent(new TogglePauseEvent
        {
            setPause = false
        });

        EventManager.Instance.TriggerEvent(new ReturnToMenuEvent());
        SceneLoaderManager.Instance.LoadScene(Env.MENU_SCENE);
    }
    public void Quit()
    {
        Application.Quit();
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
    #endregion

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

    private void OnTogglePauseEvent(TogglePauseEvent e)
    {
        this.pauseContainer.SetActive(e.setPause);
    }

    #endregion
    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<EndGameEvent>(this.OnEndGame);
            EventManager.Instance.RemoveListener<UpdatePointsEvent>(this.OnEnemyWasDestroyed);
            EventManager.Instance.RemoveListener<TogglePauseEvent>(this.OnTogglePauseEvent);
        }
    }


}
