using UnityEngine;
using System.Collections;

public class DeluxeButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void OnFinishUnlockingAnimation ()
	{
		Application.LoadLevel ("InGame");
	}
}
