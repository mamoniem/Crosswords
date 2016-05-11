using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SingleCell : MonoBehaviour {

	public enum CellTypes {Empty, Black, Invisible, ContainLetter};
	public CellTypes CellType;

	public string defaultValue;
	public string inputValue;

	public int fontSize;

	public Text theValueObject;
	public Image theBackgroundObject;
	public Image theCornerTriangleObject;
	public Text cellNumberingObject;

	public Color normalColor;
	public Color wrongColor;
	public Color pencilColor;
	
	public bool isHint;
	public bool isWrong;
	public bool isPencil = false;

	public int cellNumeredWith;
	public bool numbered;

	public int cellAcrossOrder;
	public int cellDownOrder;

	public bool isDownStartWord;
	public bool isAcrossStartWord;

	public SingleCell reflectedCell;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnSetAsSelectedCell()
	{
		if (defaultValue != GridManager.Instance.currentPuzzle.puzzleSolid)
		{
			GridManager.Instance.OnSetSelectedCell (this.gameObject.GetComponent<SingleCell>());
		}
	}

	public void OnReflectSelectedCell()
	{
		if (defaultValue != GridManager.Instance.currentPuzzle.puzzleSolid)
		{
			GridManager.Instance.OnSetSelectedCell (reflectedCell);
			reflectedCell.theValueObject.color = theValueObject.color;
		}
	}
}
