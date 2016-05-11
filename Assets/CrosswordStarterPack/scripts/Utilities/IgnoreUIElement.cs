using UnityEngine;
using System.Collections;

public class IgnoreUIElement : MonoBehaviour, ICanvasRaycastFilter{

	public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		return false;
	}
}
