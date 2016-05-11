using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {

	public static DebugManager Instance;

	public string versionNumber;
	public Text versionObject;

	public bool isAutoSolveGridOnGeneration;
	public GameObject debugButton;
	public GameObject debugMenu;

	public Text screenResolution;
	public string aspectRatio;


	public Text displayUnityVersion;
	public Text displayOS;
	public Text displayProcessor;
	public Text displayMemory;
	public Text displayGFXMemory;
	public Text displayGFXDeviceID;
	public Text displayGFXDeviceName;
	public Text displayGFXVendor;
	public Text displayFPS;

	public  float frequency = 0.5F;
	public int nbDecimal = 1;
	
	private float accum   = 0f;
	private int   frames  = 0;
	private Color fpsColor = Color.white;
	private string sFPS = "";

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		float screenRatio = (((float)Screen.height)/((float)Screen.width)) ;
		if (screenRatio >= 1.7)
		{
			aspectRatio = "16:9";
		}
		else if (screenRatio >= 1.5)
		{
			aspectRatio = "3:2";
		}
		else
		{
			aspectRatio = "4:3";
		}

#if DEBUG_BUILD
		debugButton.SetActive(true);
		screenResolution.text = "Secreen: " + Screen.currentResolution.width.ToString() +"*" + Screen.currentResolution.height.ToString() + "Ratio of: " + aspectRatio;

		versionObject.text = "Version: " + versionNumber;
		UpdateDeviceInfo();
		StartCoroutine(FPS());
#else
		debugButton.SetActive(false);
#endif

	}

	void Update()
	{
#if DEBUG_BUILD
		accum += Time.timeScale/ Time.deltaTime;
		++frames;
#endif
	}

	IEnumerator FPS()
	{
		while( true )
		{
			// Update the FPS
			float fps = accum/frames;
			sFPS = fps.ToString( "f" + Mathf.Clamp( nbDecimal, 0, 10 ) );
			
			//Update the color
			fpsColor = (fps >= 30) ? Color.green : ((fps > 10) ? Color.red : Color.yellow);
			
			accum = 0.0F;
			frames = 0;

			displayFPS.text = sFPS;
			displayFPS.color = fpsColor;
			
			yield return new WaitForSeconds( frequency );
		}
	}

	private void UpdateDeviceInfo()
	{
		displayUnityVersion.text = "Unity Version: " + Application.unityVersion.ToString();
		displayOS.text = "OS: " + SystemInfo.operatingSystem.ToString();
		displayProcessor.text = "Processor: " + SystemInfo.processorType.ToString() + "-" + SystemInfo.processorCount.ToString();
		displayMemory.text = "Memory: " + SystemInfo.systemMemorySize.ToString();
		displayGFXMemory.text = "GFX Memory: " + SystemInfo.graphicsMemorySize.ToString();
		displayGFXDeviceID.text = "GFX Device ID: " + SystemInfo.graphicsDeviceID.ToString();
		displayGFXDeviceName.text = "GFX Device Name: " + SystemInfo.graphicsDeviceName.ToString();
		displayGFXVendor.text = "GFX Device Vendor: " + SystemInfo.graphicsDeviceVendor.ToString();
	}

	public void OnClickDebugButton()
	{
		debugMenu.SetActive(!debugMenu.activeInHierarchy);
	}

	public void OnShowAllWords()
	{
		List<GameObject> _tempAllCurrentCells;
		_tempAllCurrentCells = GridManager.Instance.currentPuzzleCells;

		for (int c=0; c< _tempAllCurrentCells.Count; c++)
		{
			SingleCell _tempProcessedCell;
			_tempProcessedCell = _tempAllCurrentCells[c].GetComponent<SingleCell>();
			_tempProcessedCell.theValueObject.text = _tempProcessedCell.defaultValue;
		}
	}

	public void OnHideAllWords()
	{
		List<GameObject> _tempAllCurrentCells;
		_tempAllCurrentCells = GridManager.Instance.currentPuzzleCells;
		
		for (int c=0; c< _tempAllCurrentCells.Count; c++)
		{
			SingleCell _tempProcessedCell;
			_tempProcessedCell = _tempAllCurrentCells[c].GetComponent<SingleCell>();
			_tempProcessedCell.theValueObject.text = _tempProcessedCell.inputValue;
		}
	}

	public void OnFillAllWords()
	{
		List<GameObject> _tempAllCurrentCells;
		_tempAllCurrentCells = GridManager.Instance.currentPuzzleCells;
		
		for (int c=0; c< _tempAllCurrentCells.Count; c++)
		{
			SingleCell _tempProcessedCell;
			_tempProcessedCell = _tempAllCurrentCells[c].GetComponent<SingleCell>();
			_tempProcessedCell.inputValue = _tempProcessedCell.defaultValue;
			_tempProcessedCell.theValueObject.text = _tempProcessedCell.inputValue;
			if (_tempProcessedCell.defaultValue != GridManager.Instance.currentPuzzle.puzzleSolid)
			{
				_tempProcessedCell.isHint = true;
				_tempProcessedCell.theCornerTriangleObject.gameObject.SetActive(true);
			}
			PlayerPrefs.SetString ((GridManager.Instance.currentPuzzle.tag + " ** " + _tempProcessedCell.gameObject.name), _tempProcessedCell.inputValue);
			Debug.Log(PlayerPrefs.GetString (GridManager.Instance.currentPuzzle.tag + " ** " + _tempProcessedCell.gameObject.name));
		}

		GridManager.Instance.OnComplete();
	}

	public void OnClearAllCells()
	{
		List<GameObject> _tempAllCurrentCells;
		_tempAllCurrentCells = GridManager.Instance.currentPuzzleCells;
		
		for (int c=0; c< _tempAllCurrentCells.Count; c++)
		{
			SingleCell _tempProcessedCell;
			_tempProcessedCell = _tempAllCurrentCells[c].GetComponent<SingleCell>();
			_tempProcessedCell.theValueObject.text = "";
			_tempProcessedCell.isHint = false;
			_tempProcessedCell.theCornerTriangleObject.gameObject.SetActive(false);
		}
	}

	public void OnDeleteSaves()
	{
		PlayerPrefs.DeleteAll();
	}
}
