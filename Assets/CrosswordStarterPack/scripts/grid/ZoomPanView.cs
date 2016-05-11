using UnityEngine;
using System.Collections;

public class ZoomPanView : MonoBehaviour
{
	public static ZoomPanView Instance;

	public float perspectiveZoomSpeed = 0.01f;
	public float orthoZoomSpeed = 0.01f; 

	public const float TOUCH_FACTOR_VS_ZOOM = 0.01f;
	public float speed = 0.01f;

	void Awake ()
	{
		Instance = this;
	}

	void Update()
	{
		if (Input.touchCount == 2)
		{
			Touch firstTouch = Input.GetTouch(0);
			Touch secongTouch = Input.GetTouch(1);
			
			Vector2 firstTouchPastPosition = firstTouch.position - firstTouch.deltaPosition;
			Vector2 secondTouchPastPosition = secongTouch.position - secongTouch.deltaPosition;
			
			float pastTouchDeltaMagnitude = (firstTouchPastPosition - secondTouchPastPosition).magnitude;
			float touchDeltaMagnitude = (firstTouch.position - secongTouch.position).magnitude;
			
			float deltaMagnitudeDiff = pastTouchDeltaMagnitude - touchDeltaMagnitude;

			if (GetComponent<Camera>().orthographicSize <= 1.4f)
			{
				GetComponent<Camera>().orthographicSize = 1.42f;
			}

			if (GetComponent<Camera>().orthographicSize > 5.0f)
			{
				GetComponent<Camera>().orthographicSize = 5.0f;
			}

			if (GetComponent<Camera>().orthographicSize <= 5.0f && GetComponent<Camera>().orthographicSize > 1.4f)
			{
				GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
				GetComponent<Camera>().orthographicSize = Mathf.Max(GetComponent<Camera>().orthographicSize, 0.1f);
			}
			speed = TOUCH_FACTOR_VS_ZOOM/GetComponent<Camera>().orthographicSize;
		}

		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && GetComponent<Camera>().orthographicSize < 4.8f) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			if (GetComponent<Camera>().orthographicSize < 4.9f)
			{
				transform.Translate(-touchDeltaPosition.x * speed/2, -touchDeltaPosition.y * speed/2, 0);
			}
			else
			{
				Vector3 tempPos = transform.position;
				tempPos = new Vector3 (25, 0, 0);
				//transform.position = Vector3.Lerp(transform.position, tempPos, Time.deltaTime);
				transform.position = tempPos;
			}
		}
	}

	public void OnAutoPanTo(Vector3 panTarget)
	{
		if (GetComponent<Camera>().orthographicSize < 3.0f)
		{
			Vector3 tempPos = transform.position;
			tempPos = new Vector3 (panTarget.x, panTarget.y, -10);
			//transform.position = Vector3.Lerp(transform.position, tempPos, Time.deltaTime);
			transform.position = tempPos;
		}
	}
}