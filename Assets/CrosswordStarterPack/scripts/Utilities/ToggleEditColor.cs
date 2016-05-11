using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleEditColor : MonoBehaviour {

	public Color pressedColor;
	public Color normalColor;

	public Image theImageComponent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPressToggleButton(bool status)
	{
		if (status)
		{
			theImageComponent.color = pressedColor;
		}
		else
		{
			theImageComponent.color = normalColor;
		}
	}
}
