using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class EmptyGridRepresentation : MonoBehaviour {

	//public static EmptyGridRepresentation Instance;

	public SingleCell gridSampleCell;
	
	public XMLParser.aPuzzle currentPuzzle;
	
	public GameObject usedGridCell;
	public float usedGridCellSize;

	public List<GameObject> currentPuzzleCells = new List<GameObject>();

	public Color selectedSecondary;


	void Start () {

	}


	public void BuildTheEmptyGrid()
	{
		currentPuzzle = GridManager.Instance.currentPuzzle;

		int gridSize = int.Parse(currentPuzzle.puzzleSize);

		usedGridCellSize = this.GetComponent<RectTransform>().sizeDelta.x/(int.Parse(currentPuzzle.puzzleSize));

		GameObject _tempProcessedCell;
		SingleCell _tempProcessedCellComponent;
		RectTransform _tempProcessedCellRect;
		Vector3 _tempProcessedCellPosition = new Vector3();
		
		float _tempCellSizeNew = Mathf.Abs((this.gameObject.GetComponent<RectTransform>().sizeDelta.y)/gridSize);
		
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

				_tempProcessedCellRect.localScale = new Vector3(1, 1, 1);
				_tempProcessedCellRect.sizeDelta = new Vector2 (usedGridCellSize, usedGridCellSize);
				_tempProcessedCell.name = (x+1).ToString() + "-" + (y+1).ToString();
				
				_tempProcessedCellComponent.cellAcrossOrder = y+1;
				_tempProcessedCellComponent.cellDownOrder = x+1;

				Vector3 tempPos;
				tempPos = _tempProcessedCellRect.localPosition;
				tempPos.x = usedGridCellSize*y;
				tempPos.y = usedGridCellSize*-x;
				_tempProcessedCellRect.localPosition = tempPos;
				
				_tempProcessedCellComponent.defaultValue = currentPuzzle.puzzleData[(gridSize*x)+y].ToString();
				
				if (currentPuzzle.puzzleData[(gridSize*x)+y].ToString() == currentPuzzle.puzzleSolid)
				{
					_tempProcessedCellComponent.theBackgroundObject.color = Color.black;
				}

				//load if it was saved
				if ((PlayerPrefs.GetString (currentPuzzle.tag + " ** " + _tempProcessedCellComponent.gameObject.name) != "" &&
				     PlayerPrefs.GetString (currentPuzzle.tag + " ** " + _tempProcessedCellComponent.gameObject.name) != null) &&
				    _tempProcessedCellComponent.defaultValue != currentPuzzle.puzzleSolid)
				{
					_tempProcessedCellComponent.theBackgroundObject.color = selectedSecondary;
				}
				else
				{

				}
			}
			_tempProcessedCellPosition.x = this.transform.position.x;
		}
	}
}
