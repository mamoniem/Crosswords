using UnityEngine;
using System.Collections;

public class LogManager : MonoBehaviour {

	public static LogManager Instance;
	public bool isLogging;

	void Awake()
	{
		Instance = this;

#if DEBUG_BUILD
		isLogging = true;
#else
		isLogging = false;
#endif
	}

	public void LogMessage(string message)
	{
		if (isLogging)
		{
			Debug.Log (message);
		}
	}

	public void LogWarning(string message)
	{
		if (isLogging)
		{
			Debug.LogWarning (message);
		}
	}

	public void LogError(string message)
	{
		if (isLogging)
		{
			Debug.LogError (message);
		}
	}
}
