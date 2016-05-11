using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PuzzleButton : MonoBehaviour {
	
	public Text numberDisplay;
	public Text nameDisplay;
	public GameObject completedMark;
	
	public bool isCompleted;
	
	public int puzzleNumber;
	public string number;
	public string name;
	
	public int gridSize;
	public Color filledCell;
	
	public GameObject cellsParent;
	private GameObject cellSample;
	

	public SingleCell gridSampleCell;
	
	public XMLParser.aPuzzle currentPuzzle;
	
	public GameObject usedGridCell;
	public float usedGridCellSize;
	
	public List<GameObject> currentPuzzleCells = new List<GameObject>();
	
	public Color selectedSecondary;

	public bool isDeluxe = false;

	public GameObject deluxeLocked;
	public GameObject deluxeUnlocked;
	
	
	void Start () {
		
	}
	
	
	public void BuildToDisplay()
	{
		cellSample = PuzzleLibrary.Instance.cellSample;
		currentPuzzle = XMLParser.Instance.puzzles[puzzleNumber];
		
		int gridSizeee = int.Parse(XMLParser.Instance.puzzles[puzzleNumber].puzzleSize);
		
		usedGridCellSize = this.GetComponent<RectTransform>().sizeDelta.x/(int.Parse(XMLParser.Instance.puzzles[puzzleNumber].puzzleSize));
		
		GameObject _tempProcessedCell;
		SingleCell _tempProcessedCellComponent;
		RectTransform _tempProcessedCellRect;
		Vector3 _tempProcessedCellPosition = new Vector3();
		
		float _tempCellSizeNew = Mathf.Abs((this.gameObject.GetComponent<RectTransform>().sizeDelta.y)/gridSizeee);
		
		RectTransform theGridRect = this.gameObject.GetComponent<RectTransform>();
		
		for (int x=0; x<gridSizeee; x++)
		{
			for (int y=0; y<gridSizeee; y++)
			{
				if (y==0)
				{
					_tempProcessedCellPosition.x = this.transform.position.x;
				}
				else{
					_tempProcessedCellPosition.x += usedGridCellSize;
				}
				_tempProcessedCell = (GameObject) Instantiate(cellSample.gameObject, cellsParent.transform.position, cellsParent.transform.rotation);
				currentPuzzleCells.Add(_tempProcessedCell);
				
				_tempProcessedCellComponent = _tempProcessedCell.GetComponent<SingleCell>();
				_tempProcessedCellRect = _tempProcessedCell.GetComponent<RectTransform>();
				
				_tempProcessedCell.transform.SetParent(this.transform);
				
				_tempProcessedCellRect.localScale = new Vector3(1, 1, 1);
				_tempProcessedCellRect.sizeDelta = new Vector2 (usedGridCellSize, usedGridCellSize);
				_tempProcessedCell.name = (x+1).ToString() + "-" + (y+1).ToString();
				
				//_tempProcessedCellComponent.cellAcrossOrder = y+1;
				//_tempProcessedCellComponent.cellDownOrder = x+1;
				
				Vector3 tempPos;
				tempPos = _tempProcessedCellRect.localPosition;
				tempPos.x = usedGridCellSize*y;
				tempPos.y = usedGridCellSize*-x;
				_tempProcessedCellRect.localPosition = tempPos;
				
				_tempProcessedCellComponent.defaultValue = currentPuzzle.puzzleData[(gridSizeee*x)+y].ToString();
				
				if (currentPuzzle.puzzleData[(gridSizeee*x)+y].ToString() == currentPuzzle.puzzleSolid)
				{
					_tempProcessedCellComponent.theBackgroundObject.color = Color.black;
				}
				
				//load if it was saved
				if ((PlayerPrefs.GetString (currentPuzzle.tag + " ** " + _tempProcessedCellComponent.gameObject.name) != "" &&
				     PlayerPrefs.GetString (currentPuzzle.tag + " ** " + _tempProcessedCellComponent.gameObject.name) != null) &&
				    _tempProcessedCellComponent.defaultValue != currentPuzzle.puzzleSolid)
				{
					_tempProcessedCellComponent.theBackgroundObject.color = filledCell;
				}
				else
				{
					
				}
			}
			_tempProcessedCellPosition.x = this.transform.position.x;
		}
	}

	public void OnLoadPuzzle()
	{
		XMLParser.Instance.puzzleToLoad = puzzleNumber;

		if (isDeluxe)
		{
			deluxeUnlocked.GetComponent<Animator>().SetBool ("isAnimation", true);
		}
		else
		{
			Debug.Log("will load");
			Application.LoadLevel ("InGame");
			Debug.Log("load done");
		}
	}
}
