using UnityEngine;
using System.Collections;

public class DoubleTapableUIElement : MonoBehaviour {

	private bool _isOneClick = false;
	private bool _isTimer;
	private float _timer;
	
	private float _clicksDelay = 0.5f;

	void Update(){
		if(Input.GetMouseButtonDown(0))
		{
			if(!_isOneClick)
			{
				_timer = Time.time;
				_isOneClick = true;
			} 
			else
			{
				_isOneClick = false;
				GridManager.Instance.acrossSelectionToggle.isOn = !GridManager.Instance.acrossSelectionToggle.isOn;
				GridManager.Instance.OnChangeSelectionType();
			}
		}

		if(_isOneClick)
		{
			if((Time.time - _timer) > _clicksDelay)
			{
				_isOneClick = false;
			}
			else
			{

			}
		}
	}

}
