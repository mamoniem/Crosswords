using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputKeyboard : MonoBehaviour {

	public static InputKeyboard Instance;

	public bool isInput = false;

	public float keyPressCoolDown = 0.3f;

	public Text clueDisplay;

	public bool canPressClearButton = false;
	public Image clearButton;
	public Sprite clearButtonActive;
	public Sprite clearButtonInactive;

	public List<SingleCell> keyboardInputCells = new List<SingleCell>();

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnUpdateTheClearButtonStatus(bool status)
	{
		canPressClearButton = status;

		if (status)
		{
			clearButton.sprite = clearButtonActive;
		}
		else
		{
			clearButton.sprite = clearButtonInactive;
		}
	}
}
