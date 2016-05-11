using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MagazineUnit : MonoBehaviour {

	public int indexedWith;
	public Text nameDisplay;
	public Image logoDisplay;
	
	public string name;
	public Sprite logoSprite;

	public bool isStackElement;
	public int stackIndex;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnLoadMagazineContent()
	{
		//Debug.Log ("Loading [" + this.gameObject.name + "]");
		if (isStackElement)
		{
			PuzzleLibrary.Instance.OnLoadPuzzlesWithinMagazine(stackIndex, indexedWith);
		}
		else
		{
			PuzzleLibrary.Instance.OnLoadMagazineContent(indexedWith);
		}
	}
}
