using UnityEngine;
using System.Collections;

public class HomeScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnLoadLibrary()
	{
		Application.LoadLevel ("library");
	}
}
