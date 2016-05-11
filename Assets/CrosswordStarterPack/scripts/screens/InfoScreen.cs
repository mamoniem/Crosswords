using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour {

	public static InfoScreen Instance;

	public Text puzzleNumerDisplay;
	public Text completePercentDisplay;
	public Text timeDisplay;
	public Text altUsedDisplay;
	public Text hintUsedDisplay;

	public Text completeWordsDisplay;
	public Text correctWordsDisplay;
	public Text incorrectWordsDisplay;
	public Text correctBoxesDisplay;
	public Text inCorrectBoxesDisplay;

	private string m_puzzleNumerPrefix = "Puzzle ";
	private string m_completePercentPrefix = "Percent Complete: ";
	private string m_timePrefix = "Time so far: ";
	private string m_altUsedPrefix = "Alt. Clues Used: ";
	private string m_hintUsedPrefix = "Hints Used: ";
	
	private string m_completeWordsPrefix = "Complete Words: ";
	private string m_correctWordsPrefix = "Correct Words: ";
	private string m_incorrectWordsPrefix = "Incorrect Words: ";
	private string m_correctBoxesPrefix = "Correct Boxes: ";
	private string m_inCorrectBoxesPrefix = "Incorrect Boxes: ";

	public EmptyGridRepresentation graphicalRepresentation;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		OnUpdateTheInfoScreen();
	}

	public void OnUpdateTheInfoScreen()
	{
		graphicalRepresentation .BuildTheEmptyGrid();

		GridManager.Instance.OnUpdateStatistics();

		puzzleNumerDisplay.text = m_puzzleNumerPrefix + GridManager.Instance.currentStatistics.puzzleNumer;

		completePercentDisplay.text = m_completePercentPrefix + GridManager.Instance.currentStatistics.completePercent + "%";
		timeDisplay.text = m_timePrefix + GridManager.Instance.currentStatistics.startTime;
		altUsedDisplay.text = m_altUsedPrefix + GridManager.Instance.currentStatistics.altUsed;
		hintUsedDisplay.text = m_hintUsedPrefix + GridManager.Instance.currentStatistics.hintUsed;
		
		completeWordsDisplay.text = m_completeWordsPrefix + GridManager.Instance.currentStatistics.completeWords;
		correctWordsDisplay.text = m_correctWordsPrefix + GridManager.Instance.currentStatistics.correctWords;
		incorrectWordsDisplay.text = m_incorrectWordsPrefix + GridManager.Instance.currentStatistics.incorrectWords;
		correctBoxesDisplay.text = m_correctBoxesPrefix + GridManager.Instance.currentStatistics.correctBoxes;
		inCorrectBoxesDisplay.text = m_inCorrectBoxesPrefix + GridManager.Instance.currentStatistics.inCorrectBoxes;
	}
}
