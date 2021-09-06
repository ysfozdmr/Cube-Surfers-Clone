using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Bools")]
    public bool isLevelStart;
    public bool isLevelDone;
    public bool isLevelFail;

    [Header("Tap To Start")]
    public GameObject PanelTapToStart;

	[Header("In Game Panel")]
	public GameObject PanelInGame;
	public TextMeshProUGUI TextScore;

	[Header("End Game Panel")]
	public GameObject PanelEndGame;
	public TextMeshProUGUI TextEndGame;
	public TextMeshProUGUI TextEndGameButton;
	public Button ButtonEndGame;
	public TextMeshProUGUI TextFinalScore;

	[Header("Strings")]
	public List<string> TextsEndGameWin;
	public List<string> TextsEndGameFail;

	GameController GC;
	PlayerController Player;

    public static UIController instance;

	private void Awake()
	{
		if (!instance)
		{
            instance = this;
		}
	}
	// Start is called before the first frame update
	void Start()
    {
        StartMethods();
    }
	#region StartMethods

    void StartMethods()
	{
		GC = GameController.instance;
		Player = PlayerController.instance;
		ShowTapToStartPanel();
	}



	#endregion

	#region TapToStart

	void ShowTapToStartPanel()
	{
		PanelTapToStart.SetActive(true);
	}

	void CloseTapToStart()
	{
		PanelTapToStart.SetActive(false);
	}

	public void ButtonActionTapToStart()
	{
		CloseTapToStart();
		GC.TapToStartActions();
		ShowInGamePanel();
	}

	#endregion

	#region InGamePanel
	void ShowInGamePanel()
	{
		PanelInGame.SetActive(true);
	}

	void CloseInGamePanel()
	{
		PanelEndGame.SetActive(false);
	}

	public void UpdateScoreText(string scoreText)
	{
		TextScore.text = scoreText;
	}

	

	#endregion

	#region EndGamePanel

	public void ShowEndGamePanel()
	{
		CloseInGamePanel();
		PanelEndGame.SetActive(true);
		FillEndGameTexts();
	}

	public void ButtonActionEndGame()
	{
		ButtonEndGame.gameObject.SetActive(false);
		GC.EndGameButtonAction();
	}

	void FillEndGameTexts()
	{
		if (isLevelDone)
		{
			TextEndGame.text = TextsEndGameWin[Random.Range(0, TextsEndGameWin.Count)];
			TextEndGameButton.text = "Next Level";
		}
		else if (isLevelFail)
		{
			TextEndGame.text = TextsEndGameFail[Random.Range(0, TextsEndGameFail.Count)];
			TextEndGameButton.text = "Retry";
		}
	}


	#endregion
}
