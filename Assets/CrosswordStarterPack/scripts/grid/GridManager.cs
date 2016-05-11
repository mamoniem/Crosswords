using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {

	public static GridManager Instance;
	
	[Serializable]
	public class GridCellSizes {
		public int gridRange;
		public float cellSize;
		public GameObject cellPrefab;
	}
	public List<GridCellSizes> GridCoreInfo;

	public XMLParser.aPuzzle currentPuzzle;

	public GameObject usedGridCell;
	public float usedGridCellSize;

	public bool isAutoSelectFirstPuzzle;

	public List<GameObject> currentPuzzleCells = new List<GameObject>();

	[Serializable]
	public class FoundPuzzle {
		public SingleCell startingCell;
		public int puzzleIndex;
		public string puzzleSolution;
		public bool isDown;
		public bool isAcross;
		public string downPrimaryClue;
		public string downAlternateClue;
		public string acrossPrimaryClue;
		public string acrossAlternateClue;
	}
	public List<FoundPuzzle> FoundPuzzles = new List<FoundPuzzle>();

	[Serializable]
	public class DefinedWord {
		public List<SingleCell> wordLetters = new List<SingleCell>();
		public int wordNumber;
		public string puzzleSolution;
		public bool isDown;
		public bool isAcross;
		public string downPrimaryClue;
		public string downAlternateClue;
		public string acrossPrimaryClue;
		public string acrossAlternateClue;
	}
	public List<DefinedWord> Words = new List<DefinedWord>();

	[Serializable]
	public class PuzzleStatistics{
		public int puzzleNumer;
		
		public int completePercent;
		public string startTime;
		public int altUsed;
		public int hintUsed;
		
		public int completeWords;
		public int correctWords;
		public int incorrectWords;
		public int correctBoxes;
		public int inCorrectBoxes;
	}
	public PuzzleStatistics currentStatistics;

	public Color selectedPrimary;
	public Color selectedSecondary;
	public Color selectedSecondaryAutoLoad;

	public SingleCell selectedCell;

	public Toggle acrossSelectionToggle;
	public bool acrossSelecion = true;
	public List<SingleCell> selectedWordCells = new List<SingleCell>();
	public List<SingleCell> lastSelectedWordCells = new List<SingleCell>();
	public int selectedWordPuzzleIndex = 0;

	public Color penColor;
	public Color pencilColor;
	public Toggle penSelectionToggle;
	public bool penFont = false;

	public Color errorsColor;
	public Toggle errorShowToggle;
	public bool displayErrorsOff = true;

	public Toggle clueTypeToggle;
	public bool originalClue = true;

	public List<GameObject> hintPopupMenuGroup = new List<GameObject>();
	public List<GameObject> optionsMenuGroup = new List<GameObject>();
	public List<GameObject> infoMenuGroup = new List<GameObject>();
	public List<GameObject> completionGroup = new List<GameObject>();

	private int capturedTime = 0;
	private int seconds = 0;
	private int minutes = 0;
	private int hours = 0;
	private string secondsCorrection ;
	private string minutesCorrection ;
	private string hoursCorrection ;

	public List <SingleCell> selectionHistory = new List<SingleCell>();

	void Awake()
	{
		Instance = this;
	}
	// Use this for initialization
	void Start () {
		currentPuzzle = XMLParser.Instance.puzzles[XMLParser.Instance.puzzleToLoad];
		BuildTheGrid();
	}
	
	// Update is called once per frame
	void Update () {
		//print(Time.realtimeSinceStartup);

		capturedTime = (int)(Time.realtimeSinceStartup);
		seconds = capturedTime % 60;
		minutes = capturedTime / 60 ;
		hours = (capturedTime / 60)/60 ;
		
		if (seconds <= 9){
			secondsCorrection = "0";
		}else {
			secondsCorrection = "";
		}
		
		if (minutes <= 9){
			minutesCorrection = "0";
		}else {
			minutesCorrection = "";
		}

		if (hours <= 9){
			hoursCorrection = "0";
		}else {
			hoursCorrection = "";
		}

		currentStatistics.startTime = (hoursCorrection + hours.ToString()) +
						":" + (minutesCorrection + minutes.ToString()) +
						":" + (secondsCorrection + seconds.ToString());
	}

	public void BuildTheGrid()
	{
		int gridSize = int.Parse(currentPuzzle.puzzleSize);
		for (int c=0; c<GridCoreInfo.Count; c++)
		{
			if (GridCoreInfo[c].gridRange == gridSize)
			{
				usedGridCell = GridCoreInfo[c].cellPrefab;
				usedGridCellSize = GridCoreInfo[c].cellSize;
			}
		}

		GameObject _tempProcessedCell;
		SingleCell _tempProcessedCellComponent;
		RectTransform _tempProcessedCellRect;
		Vector3 _tempProcessedCellPosition = new Vector3();

		float _tempCellSizeNew = Mathf.Abs((this.gameObject.GetComponent<RectTransform>().sizeDelta.y)/gridSize);
		//usedGridCellSize = _tempCellSizeNew;

		//Debug.LogError(_tempCellSizeNew);
		//Debug.LogError (Screen.currentResolution.width);

		RectTransform theGridRect = this.gameObject.GetComponent<RectTransform>();
		
		float screenRatio = (((float)Screen.height)/((float)Screen.width)) ;
		//LogManager.Instance.LogMessage (screenRatio.ToString());
		if (screenRatio >= 1.7)
		{
			//LogManager.Instance.LogMessage("16:9");
			theGridRect.localScale = new Vector3 (0.965f, 0.965f, 0.965f);
		}
		else if (screenRatio >= 1.5)
		{
			//LogManager.Instance.LogMessage("3:2");
			theGridRect.localScale = new Vector3 (1, 1, 1);
		}
		else
		{
			//LogManager.Instance.LogMessage("4:3");
			theGridRect.localScale = new Vector3 (0.84f, 0.84f, 0.84f);

			Vector2 tempPivot;
			tempPivot = theGridRect.pivot;
			tempPivot.x = 0.15f;
			theGridRect.pivot = tempPivot;
		}

		for (int x=0; x<gridSize; x++)
		{
			for (int y=0; y<gridSize; y++)
			{
				if (y==0)
				{
					_tempProcessedCellPosition.x = this.transform.position.x;
				}
				else{
					_tempProcessedCellPosition.x += usedGridCellSize;
				}
				_tempProcessedCell = (GameObject) Instantiate(usedGridCell.gameObject, this.transform.position, this.transform.rotation);
				currentPuzzleCells.Add(_tempProcessedCell);

				_tempProcessedCellComponent = _tempProcessedCell.GetComponent<SingleCell>();
				_tempProcessedCellRect = _tempProcessedCell.GetComponent<RectTransform>();
				
				_tempProcessedCell.transform.SetParent(this.transform);
				//_tempProcessedCellRect.sizeDelta = new Vector2(usedGridCellSize, usedGridCellSize);
				_tempProcessedCellRect.localScale = new Vector3(1, 1, 1);
				_tempProcessedCell.name = (x+1).ToString() + "-" + (y+1).ToString();

				_tempProcessedCellComponent.cellAcrossOrder = y+1;
				_tempProcessedCellComponent.cellDownOrder = x+1;

				Vector3 tempPos;
				tempPos = _tempProcessedCellRect.localPosition;
				tempPos.x = usedGridCellSize*y;
				tempPos.y = usedGridCellSize*-x;
				_tempProcessedCellRect.localPosition = tempPos;

				_tempProcessedCellComponent.defaultValue = currentPuzzle.puzzleData[(gridSize*x)+y].ToString();
#if DEBUG_BUILD
				if (DebugManager.Instance.isAutoSolveGridOnGeneration)
				{
					_tempProcessedCellComponent.theValueObject.text = currentPuzzle.puzzleData[(gridSize*x)+y].ToString();
				}
				else{
					_tempProcessedCellComponent.theValueObject.text = "";
				}
#else
				_tempProcessedCellComponent.theValueObject.text = "";
#endif
				//this couple of lines was there to test the nubering and it's position.
				//_tempProcessedCellComponent.cellNumeredWith = y+1;
				//_tempProcessedCellComponent.cellNumberingObject.text = _tempProcessedCellComponent.cellNumeredWith.ToString();

				_tempProcessedCellComponent.theValueObject.fontSize = _tempProcessedCellComponent.fontSize;
				if (currentPuzzle.puzzleData[(gridSize*x)+y].ToString() == currentPuzzle.puzzleSolid)
				{
					_tempProcessedCellComponent.theBackgroundObject.color = Color.black;
				}
				_tempProcessedCellComponent.theCornerTriangleObject.gameObject.SetActive(false);

				//load if it was saved
				if ((PlayerPrefs.GetString (currentPuzzle.tag + " ** " + _tempProcessedCellComponent.gameObject.name) != "" ||
				    PlayerPrefs.GetString (currentPuzzle.tag + " ** " + _tempProcessedCellComponent.gameObject.name) != null) &&
				    _tempProcessedCellComponent.defaultValue != currentPuzzle.puzzleSolid)
				{
					_tempProcessedCellComponent.inputValue = PlayerPrefs.GetString (currentPuzzle.tag + " ** " + _tempProcessedCellComponent.gameObject.name);
					_tempProcessedCellComponent.theValueObject.text = _tempProcessedCellComponent.inputValue;
					_tempProcessedCellComponent.theValueObject.color = penColor;
					_tempProcessedCellComponent.theBackgroundObject.color = selectedSecondaryAutoLoad;
					//Debug.Log (_tempProcessedCellComponent.theBackgroundObject.color.ToString());
				}
			}
			_tempProcessedCellPosition.x = this.transform.position.x;
		}

		currentStatistics.puzzleNumer = int.Parse(currentPuzzle.number);

		if (PlayerPrefs.GetInt (XMLParser.Instance.puzzles[XMLParser.Instance.puzzleToLoad].tag) == 1)
		{
			OnCheckForCompletion();
		}
		else
		{
			NumberTheCells();
		}
	}

	void NumberTheCells()
	{
		if (currentPuzzleCells.Count > 0)
		{
			SingleCell _tempProcessedCellComponent;
			int _tempCellNumber = 1;

			for (int c=0; c<currentPuzzleCells.Count; c++)
			{
				FoundPuzzle _tempFoundPuzzle = new FoundPuzzle();
				_tempProcessedCellComponent = currentPuzzleCells[c].GetComponent<SingleCell>();

				if (currentPuzzleCells[c].gameObject.name.StartsWith("1-") && _tempProcessedCellComponent.defaultValue != currentPuzzle.puzzleSolid) //the first row cells will always have numbers (except if it is solid)
				{
					if (!_tempProcessedCellComponent.numbered)
					{
						_tempProcessedCellComponent.cellNumeredWith = _tempCellNumber;
						_tempProcessedCellComponent.cellNumberingObject.text = _tempProcessedCellComponent.cellNumeredWith.ToString();
						_tempProcessedCellComponent.numbered = true;
						_tempProcessedCellComponent.isDownStartWord = true;

						_tempFoundPuzzle.puzzleIndex = _tempCellNumber;
						_tempFoundPuzzle.startingCell = _tempProcessedCellComponent;
						_tempFoundPuzzle.isDown = _tempProcessedCellComponent.isDownStartWord;
						FoundPuzzles.Add(_tempFoundPuzzle);

						_tempCellNumber ++;
					}
				}
				else if (currentPuzzleCells[c].gameObject.name.EndsWith("-1") && _tempProcessedCellComponent.defaultValue != currentPuzzle.puzzleSolid) //the first cell of each row will always have number (except if it is solid)
				{
					if (!_tempProcessedCellComponent.numbered)
					{
						_tempProcessedCellComponent.cellNumeredWith = _tempCellNumber;
						_tempProcessedCellComponent.cellNumberingObject.text = _tempProcessedCellComponent.cellNumeredWith.ToString();
						_tempProcessedCellComponent.numbered = true;
						_tempProcessedCellComponent.isAcrossStartWord = true;

						_tempFoundPuzzle.puzzleIndex = _tempCellNumber;
						_tempFoundPuzzle.startingCell = _tempProcessedCellComponent;
						_tempFoundPuzzle.isAcross = _tempProcessedCellComponent.isAcrossStartWord;
						FoundPuzzles.Add(_tempFoundPuzzle);

						_tempCellNumber ++;
					}
				}
				else if (_tempProcessedCellComponent.defaultValue == currentPuzzle.puzzleSolid) //the cell has solid into it's left
				{
					if (!_tempProcessedCellComponent.numbered)
					{
						_tempProcessedCellComponent.cellNumeredWith = 0;
						_tempProcessedCellComponent.cellNumberingObject.text = "";

						//fixed the issue of the 10th and 11th
						if (c+1 < currentPuzzleCells.Count && currentPuzzleCells[c+1].gameObject != null)
						{
							_tempProcessedCellComponent = currentPuzzleCells[c+1].gameObject.GetComponent<SingleCell>();
							if (_tempProcessedCellComponent.defaultValue != currentPuzzle.puzzleSolid)
							{
								_tempProcessedCellComponent.cellNumeredWith = _tempCellNumber;
								_tempProcessedCellComponent.cellNumberingObject.text = _tempProcessedCellComponent.cellNumeredWith.ToString();
								_tempProcessedCellComponent.numbered = true;
								_tempProcessedCellComponent.isAcrossStartWord = true;

								_tempFoundPuzzle.puzzleIndex = _tempCellNumber;
								_tempFoundPuzzle.startingCell = _tempProcessedCellComponent;
								_tempFoundPuzzle.isAcross = _tempProcessedCellComponent.isAcrossStartWord;
								FoundPuzzles.Add(_tempFoundPuzzle);

								_tempCellNumber ++;
							}
						}
					}
				}
				else if (_tempProcessedCellComponent.defaultValue != currentPuzzle.puzzleSolid && currentPuzzleCells[c-(int.Parse(currentPuzzle.puzzleSize))].GetComponent<SingleCell>().defaultValue == currentPuzzle.puzzleSolid) //the cell has solid above
				{
					if (!_tempProcessedCellComponent.numbered)
					{
						_tempProcessedCellComponent.cellNumeredWith = _tempCellNumber;
						_tempProcessedCellComponent.cellNumberingObject.text = _tempProcessedCellComponent.cellNumeredWith.ToString();
						_tempProcessedCellComponent.numbered = true;
						_tempProcessedCellComponent.isDownStartWord = true;

						_tempFoundPuzzle.puzzleIndex = _tempCellNumber;
						_tempFoundPuzzle.startingCell = _tempProcessedCellComponent;
						_tempFoundPuzzle.isDown = _tempProcessedCellComponent.isDownStartWord;
						FoundPuzzles.Add(_tempFoundPuzzle);

						_tempCellNumber ++;
					}
				}
				else //any other cells shouldn't have numbers
				{
					if (!_tempProcessedCellComponent.numbered)
					{
						_tempProcessedCellComponent.cellNumeredWith = 0;
						_tempProcessedCellComponent.cellNumberingObject.text = "";
						_tempProcessedCellComponent.numbered = true;
					}
				}
			}
			CheckedAndSettAllPuzzlesDataFromXML();
		}
		if (isAutoSelectFirstPuzzle)
		{
			//make sure to not select a solid block in case it was the first letter
			for (int s=0; s<currentPuzzleCells.Count; s++)
			{
				if (currentPuzzleCells[s].GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
				{
					OnSetSelectedCell(currentPuzzleCells[s].GetComponent<SingleCell>());
					break;
				}
			}

		}
		OnDefineAllWords();
	}

	void CheckedAndSettAllPuzzlesDataFromXML()
	{
		if (FoundPuzzles.Count != 0)
		{
			for (int c=0; c<currentPuzzle.acrossClues.Count; c++)
			{
				for (int v=0; v<FoundPuzzles.Count; v++)
				{
					if (FoundPuzzles[v].puzzleIndex == int.Parse(currentPuzzle.acrossClues[c].number))
					{
						FoundPuzzles[v].acrossAlternateClue = currentPuzzle.acrossClues[c].alternate;
						FoundPuzzles[v].acrossPrimaryClue = currentPuzzle.acrossClues[c].primary;
						FoundPuzzles[v].isAcross = true;
					}
				}
			}

			for (int d=0; d<currentPuzzle.downClues.Count; d++)
			{
				for (int f=0; f<FoundPuzzles.Count; f++)
				{
					if (FoundPuzzles[f].puzzleIndex == int.Parse(currentPuzzle.downClues[d].number))
					{
						FoundPuzzles[f].downAlternateClue = currentPuzzle.downClues[d].alternate;
						FoundPuzzles[f].downPrimaryClue = currentPuzzle.downClues[d].primary;
						FoundPuzzles[f].isDown = true;
					}
				}
			}
		}
	}

	//Chek the words status
	void OnDefineAllWords()
	{
		currentStatistics.completeWords = 0;
		currentStatistics.correctWords = 0;
		currentStatistics.incorrectWords = 0;

		//all cells one by one horizontally
		List<SingleCell> _tempLettersList = new List<SingleCell>();

		for (int c=0; c< ((int.Parse(currentPuzzle.puzzleSize))*(int.Parse(currentPuzzle.puzzleSize))); c++)
		{
			if (currentPuzzleCells[c].GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
			{
				if (!currentPuzzleCells[c].name.EndsWith("-"+currentPuzzle.puzzleSize))
				{
					_tempLettersList.Add(currentPuzzleCells[c].GetComponent<SingleCell>());
					//Debug.Log( currentPuzzleCells[c].gameObject.name);
				}
				else
				{
					_tempLettersList.Add(currentPuzzleCells[c].GetComponent<SingleCell>());
					//Debug.Log(_tempLettersList.Count);
					CheckWordValidation(_tempLettersList);
					_tempLettersList.Clear();
				}
			}
			else
			{
				//Debug.Log(_tempLettersList.Count);
				CheckWordValidation(_tempLettersList);
				_tempLettersList.Clear();
			}

		}

		//TODO:: Vertical words
		//all cells one by one vertically
		/*for (int x=0; x< ((int.Parse(currentPuzzle.puzzleSize))); x++)
		{
			for (int v=0; v< ((int.Parse(currentPuzzle.puzzleSize))); v++)
			{
				Debug.Log( currentPuzzleCells[x+(int.Parse(currentPuzzle.puzzleSize)*v)].gameObject.name);
			}

		}*/
		for (int v=0; v< ((int.Parse(currentPuzzle.puzzleSize))); v++)
		{
			for (int b=0; b< ((int.Parse(currentPuzzle.puzzleSize))); b++)
			{
				//Debug.Log( currentPuzzleCells[v+(int.Parse(currentPuzzle.puzzleSize)*b)].gameObject.name);
				if (currentPuzzleCells[v+(int.Parse(currentPuzzle.puzzleSize)*b)].GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
				{
					if (!currentPuzzleCells[v+(int.Parse(currentPuzzle.puzzleSize)*b)].name.EndsWith("-"+currentPuzzle.puzzleSize))
					{
						_tempLettersList.Add(currentPuzzleCells[v+(int.Parse(currentPuzzle.puzzleSize)*b)].GetComponent<SingleCell>());
						//Debug.Log( currentPuzzleCells[c].gameObject.name);
					}
					else
					{
						_tempLettersList.Add(currentPuzzleCells[v+(int.Parse(currentPuzzle.puzzleSize)*b)].GetComponent<SingleCell>());
						//Debug.Log(_tempLettersList.Count);
						CheckWordValidation(_tempLettersList);
						_tempLettersList.Clear();
					}
				}
				else
				{
					//Debug.Log(_tempLettersList.Count);
					CheckWordValidation(_tempLettersList);
					_tempLettersList.Clear();
				}
			}
		}
	}

	void CheckWordValidation(List<SingleCell> theWordLettersList)
	{

		int correctLetters = 0;
		int emptyLetters = 0;
		int wrongLetters = 0;

		if (theWordLettersList.Count >0)
		{
			//Debug.Log(theWordLettersList.Count);
			for(int c=0; c<theWordLettersList.Count; c++)
			{
				if (theWordLettersList[c].inputValue == theWordLettersList[c].defaultValue)
				{
					correctLetters ++;
				}
				else if (theWordLettersList[c].inputValue == "")
				{
					emptyLetters ++;
				}
				else
				{
					wrongLetters ++;
				}
			}
			
			if (correctLetters == theWordLettersList.Count)
			{
				currentStatistics.correctWords ++;
			}
			else if (correctLetters != theWordLettersList.Count)
			{
				if (correctLetters + wrongLetters == theWordLettersList.Count)
				{
					currentStatistics.completeWords ++;
					currentStatistics.incorrectWords ++;
				}
			}
		}

	}

	public void OnChangeSelectionType()
	{
		acrossSelecion = acrossSelectionToggle.isOn;
		if (selectedCell != null)
		{
			OnSetSelectedCell(selectedCell);
		}
		//Debug.Log (acrossSelecion);
	}

	public void OnChangeClueType()
	{
		originalClue = clueTypeToggle.isOn;
		if (originalClue == false)
		{
			currentStatistics.altUsed ++;
		}


		OnUpdateTheClue(true);
	}

	public void OnChangeFontType()
	{
		penFont = penSelectionToggle.isOn;
		/*for (int c=0; c<currentPuzzleCells.Count; c++)
		{
			SingleCell _tempProcessedCell;
			_tempProcessedCell = currentPuzzleCells[c].GetComponent<SingleCell>();

			if (_tempProcessedCell.defaultValue != currentPuzzle.puzzleSolid)
			{
				if (penFont)
				{
					_tempProcessedCell.theValueObject.color = penColor;
				}
				else
				{
					_tempProcessedCell.theValueObject.color = pencilColor;
				}
			}
		}*/
	}

	public void OnChnageErrorDisplay()
	{
		displayErrorsOff = errorShowToggle.isOn;
		for (int c=0; c<currentPuzzleCells.Count; c++)
		{
			SingleCell _tempProcessedCell;
			_tempProcessedCell = currentPuzzleCells[c].GetComponent<SingleCell>();
			
			if (_tempProcessedCell.defaultValue != currentPuzzle.puzzleSolid)
			{
				if (_tempProcessedCell.defaultValue != _tempProcessedCell.inputValue)
				{
					if (displayErrorsOff)
					{
						if (_tempProcessedCell.isPencil)
						{
							_tempProcessedCell.theValueObject.color = pencilColor;
						}
						else
						{
							_tempProcessedCell.theValueObject.color = penColor;
						}
					}
					else
					{
						if (_tempProcessedCell.isHint)
						{
							_tempProcessedCell.theValueObject.color = penColor;
						}
						else
						{
							_tempProcessedCell.theValueObject.color = errorsColor;
						}
					}
				}
			}
		}

		UpdateTheKeyboardInputCells();
	}

	#region Options
	public void OnOptionsMenuDisplay(bool status)
	{
		for(int c=0; c<optionsMenuGroup.Count; c++)
		{
			optionsMenuGroup[c].gameObject.SetActive(status);
		}
	}

	public void OnDisplayInfoScreen(bool status)
	{
		for(int c=0; c<infoMenuGroup.Count; c++)
		{
			infoMenuGroup[c].gameObject.SetActive(status);
		}
		InfoScreen.Instance.OnUpdateTheInfoScreen();
	}

	public void OnDisplaySettingsScreen()
	{

	}


	public void OnDisplayUserGuideScreen()
	{
		
	}
	#endregion

	#region Hint
	public void OnHintMenuDisplay(bool status)
	{
		for(int c=0; c<hintPopupMenuGroup.Count; c++)
		{
			hintPopupMenuGroup[c].gameObject.SetActive(status);
		}
	}

	public void OnGiveHintLetter()
	{
		if (selectedWordCells.Count != 0)
		{
			for (int c=0; c<selectedWordCells.Count; c++)
			{
				if (selectedWordCells[c].theBackgroundObject.color == selectedPrimary)
				{
					if (!selectedWordCells[c].isHint)
					{
						selectedWordCells[c].inputValue = selectedWordCells[c].defaultValue;
						selectedWordCells[c].theValueObject.text = selectedWordCells[c].inputValue;
						selectedWordCells[c].isHint = true;
						selectedWordCells[c].theCornerTriangleObject.gameObject.SetActive(true);
						currentStatistics.hintUsed ++;
					}
				}
			}
			OnHintMenuDisplay(false);
			UpdateTheKeyboardInputCells();
		}
	}

	public void OnGiveHintWord()
	{
		if (selectedWordCells.Count != 0)
		{
			for (int c=0; c<selectedWordCells.Count; c++)
			{
				if (!selectedWordCells[c].isHint)
				{
					selectedWordCells[c].inputValue = selectedWordCells[c].defaultValue;
					selectedWordCells[c].theValueObject.text = selectedWordCells[c].inputValue;
					selectedWordCells[c].isHint = true;
					selectedWordCells[c].theCornerTriangleObject.gameObject.SetActive(true);
					selectedWordCells[c].theValueObject.color = penColor;
					PlayerPrefs.SetString ((currentPuzzle.tag + " ** " + selectedWordCells[c].name), selectedWordCells[c].inputValue);
				}
			}
			currentStatistics.hintUsed ++;
			OnHintMenuDisplay(false);
			UpdateTheKeyboardInputCells();
		}
	}
	#endregion

	public void OnSetSelectedCell(SingleCell theCell)
	{
		lastSelectedWordCells = selectedWordCells;

		DesellectAllCells();
		selectedCell = null;

		if (selectedCell != null)
		{
			checkIfIwIsSavedCell (selectedCell);
		}
		selectedCell = theCell;
		selectedCell.theBackgroundObject.color = selectedPrimary;

		string _tempScelectedCellName;

		_tempScelectedCellName = selectedCell.gameObject.name;
		//Debug.Log ("the selected cell Across is: [" + selectedCell.cellAcrossOrder + "] And the Down is: [" + selectedCell.cellDownOrder +"]");

		if (acrossSelecion) //an across word should be selected
		{
			bool _tempSolidBlockChecked = false;
			GameObject _tempLookedCell;

			//check before
			for (int y=1; y<(int.Parse(currentPuzzle.puzzleSize)); y++)
			{
				if (GameObject.Find((selectedCell.cellDownOrder).ToString() + "-" + (selectedCell.cellAcrossOrder-y).ToString()) && !_tempSolidBlockChecked)
				{
					_tempLookedCell = GameObject.Find((selectedCell.cellDownOrder).ToString() + "-" + (selectedCell.cellAcrossOrder-y).ToString());
					if (_tempLookedCell.GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
					{
						_tempLookedCell.GetComponent<SingleCell>().theBackgroundObject.color = selectedSecondary;
					}
					else if (_tempLookedCell.GetComponent<SingleCell>().defaultValue == currentPuzzle.puzzleSolid)
					{
						_tempSolidBlockChecked = true;
					}
				}

			}
			//check after
			_tempSolidBlockChecked = false;
			for (int y=1; y<(int.Parse(currentPuzzle.puzzleSize)); y++)
			{
				if (GameObject.Find((selectedCell.cellDownOrder).ToString() + "-" + (selectedCell.cellAcrossOrder+y).ToString()) && !_tempSolidBlockChecked)
				{
					_tempLookedCell = GameObject.Find((selectedCell.cellDownOrder).ToString() + "-" + (selectedCell.cellAcrossOrder+y).ToString());
					if (_tempLookedCell.GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
					{
						_tempLookedCell.GetComponent<SingleCell>().theBackgroundObject.color = selectedSecondary;
					}
					else if (_tempLookedCell.GetComponent<SingleCell>().defaultValue == currentPuzzle.puzzleSolid)
					{
						_tempSolidBlockChecked = true;
					}
				}
				
			}
		}
		else //a down word should be selected
		{
			bool _tempSolidBlockChecked = false;
			GameObject _tempLookedCell;
			
			//checkabove
			for (int y=1; y<(int.Parse(currentPuzzle.puzzleSize)); y++)
			{
				if (GameObject.Find((selectedCell.cellDownOrder-y).ToString() + "-" + (selectedCell.cellAcrossOrder).ToString()) && !_tempSolidBlockChecked)
				{
					_tempLookedCell = GameObject.Find((selectedCell.cellDownOrder-y).ToString() + "-" + (selectedCell.cellAcrossOrder).ToString());
					if (_tempLookedCell.GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
					{
						_tempLookedCell.GetComponent<SingleCell>().theBackgroundObject.color = selectedSecondary;
					}
					else if (_tempLookedCell.GetComponent<SingleCell>().defaultValue == currentPuzzle.puzzleSolid)
					{
						_tempSolidBlockChecked = true;
					}
				}
				
			}
			//check down
			_tempSolidBlockChecked = false;
			for (int y=1; y<(int.Parse(currentPuzzle.puzzleSize)); y++)
			{
				if (GameObject.Find((selectedCell.cellDownOrder+y).ToString() + "-" + (selectedCell.cellAcrossOrder).ToString()) && !_tempSolidBlockChecked)
				{
					_tempLookedCell = GameObject.Find((selectedCell.cellDownOrder+y).ToString() + "-" + (selectedCell.cellAcrossOrder).ToString());
					if (_tempLookedCell.GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
					{
						_tempLookedCell.GetComponent<SingleCell>().theBackgroundObject.color = selectedSecondary;
					}
					else if (_tempLookedCell.GetComponent<SingleCell>().defaultValue == currentPuzzle.puzzleSolid)
					{
						_tempSolidBlockChecked = true;
					}
				}
				
			}
		}

		ZoomPanView.Instance.OnAutoPanTo(selectedCell.gameObject.transform.position);

		//set the selectedCellNeighbours & their colors
		DefineSelectedGroup();
		//reset the clue button first
	}

	void DesellectAllCells()
	{
		for (int c=0; c<currentPuzzleCells.Count; c++)
		{
			if (currentPuzzleCells[c].GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
			{
				checkIfIwIsSavedCell (currentPuzzleCells[c].GetComponent<SingleCell>());
			}
		}
	}

	void checkIfIwIsSavedCell(SingleCell theCell)
	{
		if ((PlayerPrefs.GetString (currentPuzzle.tag + " ** " + theCell.gameObject.name) != "" /*||
		     PlayerPrefs.GetString (currentPuzzle.tag + " ** " + theCell.gameObject.name) != null*/) && 
			theCell.defaultValue != currentPuzzle.puzzleSolid)
		{
			theCell.theBackgroundObject.color = selectedSecondaryAutoLoad;
		}
		else
		{
			theCell.theBackgroundObject.color = Color.white;
		}
	}
	void DefineSelectedGroup()
	{
		selectedWordCells.Clear();
		for (int c=0; c<currentPuzzleCells.Count; c++)
		{
			if (currentPuzzleCells[c].GetComponent<SingleCell>().theBackgroundObject.color == selectedPrimary ||
			    currentPuzzleCells[c].GetComponent<SingleCell>().theBackgroundObject.color == selectedSecondary)
			{
				selectedWordCells.Add(currentPuzzleCells[c].GetComponent<SingleCell>());
				//break;
			}
		}

		//update the clear button if the selected letter is not the first in the word
		if (selectedCell != selectedWordCells[0])
		{
			InputKeyboard.Instance.OnUpdateTheClearButtonStatus(true);
		}
		else
		{
			if (selectedCell.inputValue != "")
			{
				InputKeyboard.Instance.OnUpdateTheClearButtonStatus(true);
			}
			else
			{
				InputKeyboard.Instance.OnUpdateTheClearButtonStatus(false);
			}
		}
		DefineTheSelectedWordPuzzleIndex();
		//Debug.Log ("<color=green>**The selected word made of [</color>" + selectedWordCells.Count +"<color=green>] cells]</color>");
	}

	void DefineTheSelectedWordPuzzleIndex(){
		//selectedWordPuzzleIndex
		for (int c=0; c<selectedWordCells.Count; c++)
		{
			if (selectedWordCells[c].cellNumeredWith != 0)
			{
				selectedWordPuzzleIndex = selectedWordCells[c].cellNumeredWith;
				if (selectionHistory != null && selectionHistory.Count > 0)
				{
					if (selectionHistory[selectionHistory.Count-1].cellNumeredWith == selectedWordCells[c].cellNumeredWith)
					{

					}
					else
					{
						selectionHistory.Add(selectedWordCells[c]);
						//Debug.Log("Added [" + selectedWordCells[c].cellNumeredWith + "] To the history");
					}

				}
				else if (selectionHistory.Count <= 0)
				{
					selectionHistory.Add(selectedWordCells[c]);
					//Debug.Log("Added [" + selectedWordCells[c].cellNumeredWith + "] To the history");
				}
				break;
			}
		}
		UpdateTheKeyboardInputCells();
	}

	void UpdateTheKeyboardInputCells()
	{
		//disable all first
		for (int c=0; c<InputKeyboard.Instance.keyboardInputCells.Count; c++)
		{
			InputKeyboard.Instance.keyboardInputCells[c].gameObject.SetActive(false);
		}

		//then display the ones matches the selected word
		if (selectedWordCells.Count != 0)
		{
			for (int v=0; v<selectedWordCells.Count; v++)
			{
				InputKeyboard.Instance.keyboardInputCells[v].gameObject.SetActive(true);

				InputKeyboard.Instance.keyboardInputCells[v].reflectedCell = selectedWordCells[v];

				InputKeyboard.Instance.keyboardInputCells[v].CellType = selectedWordCells[v].CellType;
				InputKeyboard.Instance.keyboardInputCells[v].inputValue = selectedWordCells[v].inputValue;
				InputKeyboard.Instance.keyboardInputCells[v].theValueObject.color = selectedWordCells[v].theValueObject.color;
				InputKeyboard.Instance.keyboardInputCells[v].defaultValue = selectedWordCells[v].defaultValue;

				InputKeyboard.Instance.keyboardInputCells[v].theBackgroundObject.color = selectedWordCells[v].theBackgroundObject.color;
				InputKeyboard.Instance.keyboardInputCells[v].theCornerTriangleObject.gameObject.SetActive(selectedWordCells[v].theCornerTriangleObject.gameObject.activeInHierarchy);
				InputKeyboard.Instance.keyboardInputCells[v].theValueObject.text = selectedWordCells[v].theValueObject.text;

			}
		}
		OnUpdateTheClue(false);

		int _tempCompletedCellsOfWord = 0;
		for (int c=0; c<selectedWordCells.Count; c++)
		{
			if (selectedWordCells[c].inputValue != ""){
				_tempCompletedCellsOfWord ++;
			}
		}

		if (_tempCompletedCellsOfWord == selectedWordCells.Count)
		{
			//Debug.Log ("Skip that filled word!!!");
			OnMoveToTheNextWord();
		}
	}

	void OnUpdateTheClue(bool isAnimated)
	{
		if (isAnimated)
		{
			StartCoroutine(DisplayTheClue());
		}
		else
		{

		}
		//Debug.Log ("Updating the clue text......");
		for (int c=0; c<FoundPuzzles.Count; c++)
		{
			if (FoundPuzzles[c].puzzleIndex == selectedWordPuzzleIndex)
			{
				if (acrossSelecion)
				{
					if (originalClue)
					{
						InputKeyboard.Instance.clueDisplay.text = selectedWordPuzzleIndex.ToString() + ". " + FoundPuzzles[c].acrossPrimaryClue;
					}
					else
					{
						InputKeyboard.Instance.clueDisplay.text = selectedWordPuzzleIndex.ToString() + ". " + FoundPuzzles[c].acrossAlternateClue;
					}
				}
				else
				{
					if (originalClue)
					{
						InputKeyboard.Instance.clueDisplay.text = selectedWordPuzzleIndex.ToString() + ". " + FoundPuzzles[c].downPrimaryClue;
					}
					else
					{
						InputKeyboard.Instance.clueDisplay.text = selectedWordPuzzleIndex.ToString() + ". " + FoundPuzzles[c].downAlternateClue;
					}
				}
			}
		}
	}

	IEnumerator DisplayTheClue()
	{
		InputKeyboard.Instance.clueDisplay.gameObject.GetComponent<Animator>().SetBool("isClue", true);
		yield return new WaitForSeconds (0.05f);
		InputKeyboard.Instance.clueDisplay.gameObject.GetComponent<Animator>().SetBool("isClue", false);
	}

	public void OnKeyboardInputValue(string value)
	{
		//Debug.Log ("start the key function");
		if (selectedCell != null && !selectedCell.isHint)
		{
			//Debug.Log ("PASSED!!!!");
			int _tempCurrentCellIndexInTheWord = 0;

			selectedCell.inputValue = value;
			selectedCell.theValueObject.text = selectedCell.inputValue;

			if (selectedCell.defaultValue != currentPuzzle.puzzleSolid)
			{
				PlayerPrefs.SetString ((currentPuzzle.tag + " ** " + selectedCell.name), selectedCell.inputValue);
			}

			if (!displayErrorsOff)
			{
				if (selectedCell.inputValue == selectedCell.defaultValue)
				{
					if (penFont)
					{
						selectedCell.theValueObject.color = penColor;
						//Debug.Log ("colored as Pen");
					}
					else
					{
						selectedCell.theValueObject.color = pencilColor;
						selectedCell.isPencil = true;
						//Debug.Log ("colored as PenCil");
					}

				}
				else{
					selectedCell.theValueObject.color = errorsColor;
				}
			}
			else
			{
				if (penFont)
				{
					selectedCell.theValueObject.color = penColor;
					//Debug.Log ("colored as Pen");
				}
				else
				{
					selectedCell.theValueObject.color = pencilColor;
					selectedCell.isPencil = true;
					//Debug.Log ("colored as PenCil");
				}
			}

			//Debug.Log("foor LoaderOptimization");
			for (int c=0; c<selectedWordCells.Count; c++)
			{
				if (selectedWordCells.Count >0 && selectedWordCells[c] == selectedCell)
				{
					_tempCurrentCellIndexInTheWord = c;
					break;
				}
			}

			//Debug.Log("if statement");
			//check if to move to the next letter in a word, or to move to a whole new word
			if (_tempCurrentCellIndexInTheWord+1 < int.Parse(currentPuzzle.puzzleSize)){
				if (_tempCurrentCellIndexInTheWord+1 < selectedWordCells.Count)
				{
					//Debug.Log("set the next selection");
					if (selectedWordCells[_tempCurrentCellIndexInTheWord+1] != null){
					    selectedCell = selectedWordCells[_tempCurrentCellIndexInTheWord+1];
						if (selectedCell != null)
						{
							OnSetSelectedCell(selectedCell);
						}
					}
				}
				else
				{
					//Debug.Log("Will move next");
					OnMoveToTheNextWord();
				}
			}
		}
		//Debug.Log("Will look for compilation");
		OnCheckForCompletion();
	}

	public void OnMoveToTheNextWord()
	{
		clueTypeToggle.isOn = true;

		InputKeyboard.Instance.clueDisplay.gameObject.GetComponent<Animator>().SetBool("isClue", false);

		int _tempCurrentCellIndexInTheWord = 0;

		for (int x=0; x<currentPuzzleCells.Count; x++)
		{
			//move to the next word
			if (currentPuzzleCells[x].GetComponent<SingleCell>().cellNumeredWith == selectedWordPuzzleIndex+1)
			{
				selectedCell = currentPuzzleCells[x].GetComponent<SingleCell>();
				selectedWordPuzzleIndex ++;
				break;
			}

			//move to the first word
			else if (currentPuzzleCells[x].GetComponent<SingleCell>().cellNumeredWith == FoundPuzzles.Count)
			{
				selectedCell = currentPuzzleCells[0].GetComponent<SingleCell>();
				selectedWordPuzzleIndex = 0;
				break;
			}
		}
		
		for (int b=0; b<FoundPuzzles.Count; b++)
		{
			if (FoundPuzzles[b].puzzleIndex == selectedWordPuzzleIndex)
			{
				if (FoundPuzzles[b].isAcross)
				{
					acrossSelectionToggle.isOn = true;
				}
				else if (!FoundPuzzles[b].isAcross)
				{
					acrossSelectionToggle.isOn = false;
				}
			}
		}

		OnSetSelectedCell(selectedCell);

		//check if the auto select got an already filled cell
		for (int c=0; c<selectedWordCells.Count; c++)
		{
			if (selectedWordCells[c] == selectedCell)
			{
				_tempCurrentCellIndexInTheWord = c;
				break;
			}
		}

		for (int m=0; m<selectedWordCells.Count; m++)
		{
			if (selectedCell == selectedWordCells[m] && selectedCell.inputValue != "")
			{
				//Debug.Log ("Ooops, came to an already filled cell...!  " + selectedWordCells[m].inputValue);
				if (_tempCurrentCellIndexInTheWord+m < selectedWordCells.Count)
				{
					selectedCell = selectedWordCells[_tempCurrentCellIndexInTheWord+1];
					_tempCurrentCellIndexInTheWord++;
					OnSetSelectedCell(selectedCell);
				}
			}
			//Debug.LogWarning("No more letters!!!!!!!");
		}
	}

	//13
	public void OnMoveToThePreviousWord()
	{
		for (int c=0; c<currentPuzzleCells.Count; c++)
		{
			if (selectionHistory.Count-1 >= 0)
			{
				if (currentPuzzleCells[c].GetComponent<SingleCell>().cellNumeredWith == selectionHistory[selectionHistory.Count-1].cellNumeredWith)
				{
					Debug.Log ("Working case!");
					OnSetSelectedCell(currentPuzzleCells[c].GetComponent<SingleCell>());
					selectionHistory.Remove(selectedCell);
				}
			}
		}
		/*
		int _tempCurrentCellIndexInTheWord = 0;
		
		for (int x=0; x<currentPuzzleCells.Count; x++)
		{
			//move to the next word
			if (selectedCell.numbered && selectedCell.cellNumeredWith != 1){
				if (currentPuzzleCells[x].GetComponent<SingleCell>().cellNumeredWith == selectedWordPuzzleIndex-1)
				{
					selectedCell = currentPuzzleCells[x].GetComponent<SingleCell>();
					selectedWordPuzzleIndex --;
					break;
				}
			}
			else
			{
				Debug.Log ("First !!!!");
				for (int v=0; v<currentPuzzleCells.Count; v++)
				{
					if (currentPuzzleCells[v].GetComponent<SingleCell>().cellNumeredWith == FoundPuzzles.Count)
					{
						selectedCell =currentPuzzleCells[FoundPuzzles.Count+1].GetComponent<SingleCell>();
						selectedWordPuzzleIndex = FoundPuzzles.Count+1;
						break;
					}
				}

			}

		}
		
		for (int b=0; b<FoundPuzzles.Count; b++)
		{
			if (FoundPuzzles[b].puzzleIndex == selectedWordPuzzleIndex)
			{
				if (FoundPuzzles[b].isAcross)
				{
					acrossSelectionToggle.isOn = true;
				}
				else if (!FoundPuzzles[b].isAcross)
				{
					acrossSelectionToggle.isOn = false;
				}
			}
		}
		
		OnSetSelectedCell(selectedCell);


		//check if the auto select got an already filled cell
		for (int c=0; c<selectedWordCells.Count; c++)
		{
			if (selectedWordCells[c] == selectedCell)
			{
				_tempCurrentCellIndexInTheWord = c;
				break;
			}
		}
		
		for (int m=0; m<selectedWordCells.Count; m++)
		{
			if (selectedCell == selectedWordCells[m] && selectedCell.inputValue != "")
			{
				//Debug.Log ("Ooops, came to an already filled cell...!  " + selectedWordCells[m].inputValue);
				if (_tempCurrentCellIndexInTheWord+m < selectedWordCells.Count)
				{
					selectedCell = selectedWordCells[_tempCurrentCellIndexInTheWord+1];
					_tempCurrentCellIndexInTheWord++;
					OnSetSelectedCell(selectedCell);
				}
			}
			//Debug.LogWarning("No more letters!!!!!!!");
		}
		*/
	}

	public void OnClearButtonPressed()
	{
		if (InputKeyboard.Instance.canPressClearButton)
		{
			int _tempCurrentCellIndexInTheWord = 0;
			
			selectedCell.inputValue = "";
			selectedCell.theValueObject.text = selectedCell.inputValue;
			for (int c=selectedWordCells.Count-1; c>0; c--)
			{
				if (selectedWordCells[c] != null)
				{
					if (selectedWordCells[c] == selectedCell)
					{
						_tempCurrentCellIndexInTheWord = c;
						break;
					}
				}
			}
			
			//check if to move to the next letter in a word, or to move to a whole new word
			if (_tempCurrentCellIndexInTheWord-1 < selectedWordCells.Count)
			{
				if (_tempCurrentCellIndexInTheWord-1 >= 0)
				{
					selectedCell = selectedWordCells[_tempCurrentCellIndexInTheWord-1];
					OnSetSelectedCell(selectedCell);

				}
				else
				{
					InputKeyboard.Instance.OnUpdateTheClearButtonStatus(false);
				}
			}
			else
			{
				//it is the first letter in the word
				InputKeyboard.Instance.OnUpdateTheClearButtonStatus(false);
			}
			UpdateTheKeyboardInputCells();
		}
	}

	public void OnUpdateStatistics()
	{
		#region Percentage
		int _tempFilledCells = 0;
		int _tempAllValidCells = 0;

		for(int f=0; f<currentPuzzleCells.Count; f++)
		{
			if (currentPuzzleCells[f].GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
			{
				_tempAllValidCells ++;
				if (currentPuzzleCells[f].GetComponent<SingleCell>().inputValue != "")
				{
					_tempFilledCells ++;
				}
			}

		}
		float _tempPercentage = 0;
		_tempPercentage = ((float)_tempFilledCells/(float)_tempAllValidCells)*100;
		currentStatistics.completePercent = (int) _tempPercentage;
		#endregion

		#region Hints
		/*
		int _temphintsCells = 0;

		for(int f=0; f<currentPuzzleCells.Count; f++)
		{
			if (currentPuzzleCells[f].GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
			{
				if (currentPuzzleCells[f].GetComponent<SingleCell>().inputValue != "")
				{
					_temphintsCells ++;
				}
			}
			
		}
		currentStatistics.hintUsed = _temphintsCells;
		*/
		#endregion
	
		#region boxes correct & incorrect
		int _tempCorrectCells = 0;
		int _tempIncorrectCells = 0;
		
		for(int f=0; f<currentPuzzleCells.Count; f++)
		{
			if (currentPuzzleCells[f].GetComponent<SingleCell>().defaultValue != currentPuzzle.puzzleSolid)
			{
				if (currentPuzzleCells[f].GetComponent<SingleCell>().inputValue == currentPuzzleCells[f].GetComponent<SingleCell>().defaultValue)
				{
					_tempCorrectCells ++;
				}
				else if (currentPuzzleCells[f].GetComponent<SingleCell>().inputValue != currentPuzzleCells[f].GetComponent<SingleCell>().defaultValue && currentPuzzleCells[f].GetComponent<SingleCell>().inputValue != "")
				{
					_tempIncorrectCells ++;
				}
			}
			
		}
		currentStatistics.correctBoxes = _tempCorrectCells;
		currentStatistics.inCorrectBoxes = _tempIncorrectCells;
		#endregion

		#region Words corrrect, wrong & incorrect
		OnDefineAllWords();
		#endregion
	}

	public void OnCheckForCompletion()
	{
		SingleCell _tempProcessedCell;
		int foundCells = 0;

		for (int c=0; c<currentPuzzleCells.Count; c++)
		{
			_tempProcessedCell = currentPuzzleCells[c].GetComponent<SingleCell>();
			if (_tempProcessedCell.defaultValue == currentPuzzle.puzzleSolid)
			{
				foundCells ++;
			}
			else
			{
				if (_tempProcessedCell.inputValue == _tempProcessedCell.defaultValue)
				{
					foundCells ++;
				}
			}
		}

		if (foundCells == currentPuzzleCells.Count)
		{
			OnComplete();
		}
	}

	public void OnComplete()
	{
		StartCoroutine (DoCompleteSequence());
	}

	IEnumerator DoCompleteSequence()
	{
		PlayerPrefs.SetInt (currentPuzzle.tag, 1);
		//hide the debug menu if it is not hidden
		if (DebugManager.Instance.debugMenu.activeInHierarchy)
		{
			DebugManager.Instance.debugMenu.SetActive(false);
		}

		//display the animation
		SingleCell _tempProcessedCell;
		bool coloredCell = false;
		int flashingTime = 3;
		int _flashedTimes = 0;
		float flashingTimer = 0.1f;
		while (_flashedTimes < flashingTime)
		{
			for (int a=0; a<currentPuzzleCells.Count; a++)
			{
				coloredCell = !coloredCell;
				_tempProcessedCell = currentPuzzleCells[a].GetComponent<SingleCell>();
				if (_tempProcessedCell.defaultValue != currentPuzzle.puzzleSolid)
				{
					if (coloredCell)
					{
						_tempProcessedCell.theBackgroundObject.color = selectedSecondary;
					}
					else
					{
						_tempProcessedCell.theBackgroundObject.color = Color.white;
					}
				}
			}
			yield return new WaitForSeconds (flashingTimer);
			for (int b=0; b<currentPuzzleCells.Count; b++)
			{
				_tempProcessedCell = currentPuzzleCells[b].GetComponent<SingleCell>();
				if (_tempProcessedCell.defaultValue != currentPuzzle.puzzleSolid)
				{
					_tempProcessedCell.theBackgroundObject.color = Color.white;
				}
			}
			yield return new WaitForSeconds (flashingTimer);
			coloredCell = false;
			_flashedTimes++;
		}

		for (int a=0; a<currentPuzzleCells.Count; a++)
		{
			_tempProcessedCell = currentPuzzleCells[a].GetComponent<SingleCell>();
			if (_tempProcessedCell.defaultValue != currentPuzzle.puzzleSolid)
			{
				_tempProcessedCell.theBackgroundObject.color = selectedSecondary;
			}
		}

		//display the message
		for(int c=0; c<completionGroup.Count; c++)
		{
			completionGroup[c].gameObject.SetActive(true);
		}
	}

	public void OnGoBackToLibrary()
	{
		XMLParser.Instance.puzzles.Clear();
		Application.LoadLevel ("library");
	}
}
