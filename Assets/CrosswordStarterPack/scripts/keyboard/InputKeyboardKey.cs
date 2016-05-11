using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputKeyboardKey : MonoBehaviour {

	public string keyValue;

	private float m_keyPressCoolDown;

	public Image keyDefault;
	public Image keyPressed;

	void Awake()
	{
		keyValue = keyDefault.gameObject.name.Remove(0, 3);
	}
	// Use this for initialization
	void Start () {
		m_keyPressCoolDown = InputKeyboard.Instance.keyPressCoolDown;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnKeyGetPressed()
	{
		//Debug.Log("first call");
		//StartCoroutine(OnKeyGetPressedSequence());
		OnKeyGetPressedSequence();
	}

	public void OnKeyGetPressedSequence()
	{
		//Debug.Log("Input started");
		if (!InputKeyboard.Instance.isInput)
		{
			//Debug.Log("Input PASSED");
			//LogManager.Instance.LogMessage ("<color=yellow>Pressed the virtual key </color> [" + keyValue + "]");

			InputKeyboard.Instance.isInput = true;
			keyDefault.enabled = false;
			keyPressed.enabled = true;

			//GridManager.Instance.OnKeyboardInputValue(keyValue);

			//[3]
			//yield return new WaitForSeconds (m_keyPressCoolDown);

			//keyPressed.enabled = false;
			//keyDefault.enabled = true;
			//InputKeyboard.Instance.isInput = false;
		}
	}

	public void OnKeyHoldsEnd()
	{
		GridManager.Instance.OnKeyboardInputValue(keyValue);

		keyPressed.enabled = false;
		keyDefault.enabled = true;
		InputKeyboard.Instance.isInput = false;
	}

	public void OnKeyDragged()
	{
		keyPressed.enabled = false;
		keyDefault.enabled = true;
		InputKeyboard.Instance.isInput = false;
	}
}
