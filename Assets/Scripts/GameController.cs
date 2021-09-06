using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Bools")]
    public bool isLevelStart;
    [Tooltip("Level'ýn Baþarýlý Bitiþ Durumu")]
    public bool isLevelDone;
    public bool isLevelFail;
    public bool isLevelFinish;
    public bool isMagnetTriggered;
    


    [Space(15)]
    public int level;

    [Header("Tags")]
    public string TagObstacle;
    public string TagFinish;
    public string TagCollectable;
    public string TagCube;
    public string TagWall;
    public string TagFallTrigger;
    public string TagBoosterStart;
    public string TagMagnetTrigger;
    public string TagMagnet;
    public string TagLava;
    public string TagFlag;
    public string TagMultiplierBox;
    public string TagWayPoint;
    public string TagTurnTrigger;
    public string TagEndGameTrigger;
   
    
   
    
   
    PlayerController Player;
    UIController UI;
    CameraController Camera;
    FinishTrigger finishTrigger;
   

    public static GameController instance;

    private void Awake()
    {
        AwakeMethods();
    }
    #region AwakeMethods

    void AwakeMethods()
    {
        SetInstance();
        GetLevel();
    }

    void SetInstance()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    void GetLevel()
    {
        if (PlayerPrefs.GetInt("Level") == 0)
        {
            level = 1;
            PlayerPrefs.SetInt("Level", 1);
        }
        else
        {
            level = PlayerPrefs.GetInt("Level");
        }
    }




    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Player = PlayerController.instance;
        UI = UIController.instance;
        Camera = CameraController.instance;
        finishTrigger = FinishTrigger.instance;
       
    }


    #region TapToStart

    public void TapToStartActions()
    {
        SendLevelStart();
        Player.TapToStartActions();
    }
    void SendLevelStart()
    {
        UI.isLevelStart = true;
        isLevelStart = true;
        Player.isLevelStart = true;
        Camera.isLevelStart = true;
        Player.isMagnetTriggered = false;
        
    }


    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnCamera()
    {

        Camera.StartTurning();


    }

    #region EndGame

    public void LevelComplete()
    {
        isLevelFinish = true;
        isLevelDone = true;
        Player.isLevelDone = true;
        UI.isLevelDone = true;
        Camera.isLevelDone = true;
        SetCoin();
        UI.ShowEndGamePanel();
        Player.ActionLevelDone();
    }
    public void LevelFail()
    {
        isLevelFail = true;
        Player.isLevelFail = true;
        UI.isLevelFail = true;
        Camera.isLevelFail = true;
        SetCoin();
        UI.ShowEndGamePanel();
        Player.ActionLevelFail();
    }

    void SetCoin()
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + Player.score);
    }


    public void EndGameButtonAction()
    {
       
        if (isLevelDone)
        {
            if (level == SceneManager.sceneCount)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                PlayerPrefs.SetInt("Level", level + 1);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

        }
        else if (isLevelFail)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void EndGameTrigger()
    {
        if (isLevelFinish)
        {
           




        }
    }
    #endregion
}
