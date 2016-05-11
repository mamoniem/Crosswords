using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class PuzzleLibrary : MonoBehaviour {

	public static PuzzleLibrary Instance;

	public GameObject magazineSample;
	public GameObject puzzleSample;
	public GameObject cellSample;


	[Serializable]
	public class SingleMagazine{
		public TextAsset dataFile;
		public string displayName;
		public Sprite displayImage;
	}

	[Serializable]
	public class Magazine{
		public bool isStack;
		public bool isDeluxe;

		public List<SingleMagazine> stackContent = new List<SingleMagazine>();

		public TextAsset dataFile;
		public string displayName;
		public Sprite displayImage;

		public bool isAvaliable;
	}
	public List <Magazine> gameMagazinesLibrary = new List<Magazine>();

	public const float SPACE_BETWEEN_MAGAZINES_X = 25.0f;
	public const float SPACE_BETWEEN_MAGAZINES_Y = 35.0f;

	public const float SPACE_BETWEEN_PUZZLES_X = 1.5f;
	public const float SPACE_BETWEEN_PUZZLES_Y = 115.0f;
	public const int PUZZLES_PER_ROW = 3;

	public const int DELUXE_SIZE = 35;
	public const int DELUXE_PUZZLES = 30;
	public const int DELUXE_UNLOCKABLE = 5;
	public const int DELUXE_UNLOCK_AFTER = 6;

	public GameObject magazinesPanel;
	public GameObject stackPanel;
	public GameObject stackContentPanel;
	public GameObject puzzlesPanel;
	public GameObject deluxePanel;

	public GameObject magazinesParent;
	public GameObject puzzlesParent;
	public GameObject stacksParent;
	public GameObject stacksContentParent;
	public GameObject deluxeContentParent;
	public GameObject deluxeUnlockablesParent;

	public List<GameObject> CurrentMagazine = new List<GameObject>();
	public List<GameObject> CurrentStackContent = new List<GameObject>();

	void Awake()
	{
		Instance = this;
	}


	void Start () {
		OnBuildtheLibrary();
	}
	
	
	void Update () {
	
	}

	public void OnBuildtheLibrary()
	{
		GameObject _tempObject;
		MagazineUnit _processedMagazine;
		RectTransform _tempRect;

		for (int c=0; c<gameMagazinesLibrary.Count; c++)
		{
			_tempObject = (GameObject) Instantiate(magazineSample.gameObject, this.transform.position, this.transform.rotation);
			_tempObject.transform.SetParent(magazinesParent.transform);
			_tempObject.name = "mag-" + gameMagazinesLibrary[c].displayName;
			_tempRect = _tempObject.GetComponent<RectTransform>();
			_processedMagazine = _tempObject.GetComponent<MagazineUnit>();
			_tempRect.localScale = new Vector3(1, 1, 1);

			Vector3 tempPos;
			tempPos = _tempRect.localPosition;
			tempPos.x = (_tempRect.sizeDelta.x + SPACE_BETWEEN_MAGAZINES_X)*c;
			tempPos.y = magazinesParent.GetComponent<RectTransform>().pivot.y; //*-c
			_tempRect.localPosition = tempPos;

			_processedMagazine.indexedWith = c;
			_processedMagazine.name = gameMagazinesLibrary[c].displayName;
			_processedMagazine.logoSprite = gameMagazinesLibrary[c].displayImage;
			_processedMagazine.nameDisplay.text = _processedMagazine.name;
			_processedMagazine.logoDisplay.sprite = _processedMagazine.logoSprite;
		}
	}

	public void OnLoadMagazineContent(int index)
	{
		magazinesPanel.SetActive (false);
		//Debug.Log("Checking the type of " + gameMagazinesLibrary[index].displayName);

		if (gameMagazinesLibrary[index].isDeluxe)
		{
			//Debug.Log("Loading a Deluxe");
			deluxePanel.SetActive(true);
			OnLoadPuzzlesWithinDeluxe(index);
		}
		else
		{
			if (gameMagazinesLibrary[index].isStack)
			{
				//Debug.Log("Loading a Stack");
				stackPanel.SetActive(true);
				OnLoadMagazinesWithinStack(index);
			}
			else
			{
				//Debug.Log("Loading a Magazine");
				puzzlesPanel.SetActive(true);
				OnLoadPuzzlesWithinMagazine(index);
			}
		}
	}

	public void OnLoadLibraryMagazinesList()
	{
		puzzlesPanel.SetActive(false);
		stackPanel.SetActive(false);
		stackContentPanel.SetActive(false);
		deluxePanel.SetActive(false);

		magazinesPanel.SetActive (true);
		OnClearTheCurrentLoadedMagazine();
	}

	public void OnLoadStackMagazinesList()
	{
		puzzlesPanel.SetActive(false);
		deluxePanel.SetActive(false);
		magazinesPanel.SetActive (false);
		stackContentPanel.SetActive(false);
		
		stackPanel.SetActive(true);
		OnClearTheCurrentLoadedMagazine();
	}

	public void OnClearTheCurrentLoadedMagazine()
	{
		for (int c=0; c<CurrentMagazine.Count; c++)
		{
			Destroy (CurrentMagazine[c].gameObject);
		}
		XMLParser.Instance.puzzles.Clear();
		CurrentMagazine.Clear();
	}

	// Deluxe
	public void OnLoadPuzzlesWithinDeluxe(int index)
	{

		CurrentMagazine.Clear();
		XMLParser.Instance.xmlFile = gameMagazinesLibrary[index].dataFile;
		XMLParser.Instance.BreakdownXML();
		
		
		GameObject _tempObject;
		PuzzleButton _processedPuzzle;
		RectTransform _tempRect;
		
		
		int theTotalObjectsAmount = XMLParser.Instance.puzzles.Count;
		float theDistanceBetweenObjects = puzzleSample.GetComponent<RectTransform>().sizeDelta.x + SPACE_BETWEEN_PUZZLES_X;
		
		int instances = 0;
		int indexedWith = 0;
		
		
		int rows = theTotalObjectsAmount/PUZZLES_PER_ROW;
		Vector3 _tempPosition;
		int rowsProcessed = 0;

		int deluxUnlockablesProcessed = 0;;

		indexedWith = theTotalObjectsAmount;
		
		while (indexedWith > 0)
		{
			for (int v=0; v<PUZZLES_PER_ROW; v++)
			{
				if (instances < theTotalObjectsAmount)
				{
					//normal items
					if (instances < DELUXE_PUZZLES)
					{
						_tempObject = (GameObject) Instantiate(puzzleSample.gameObject, deluxeContentParent.transform.position, deluxeContentParent.transform.rotation);
						_tempObject.transform.SetParent(deluxeContentParent.transform);
						_tempObject.name = "puzzle-" + XMLParser.Instance.puzzles[instances].number;
						_tempRect = _tempObject.GetComponent<RectTransform>();
						_processedPuzzle = _tempObject.GetComponent<PuzzleButton>();
						_tempRect.localScale = new Vector3(1, 1, 1);
						
						CurrentMagazine.Add (_tempObject);
						Vector3 tempPos;
						
						//do the positioning chnages
						_tempPosition = _tempObject.transform.position;
						_tempPosition.x += SPACE_BETWEEN_PUZZLES_X*v;
						_tempPosition.y -= SPACE_BETWEEN_PUZZLES_X*rowsProcessed;
						_tempObject.transform.position = _tempPosition;
						
						
						_processedPuzzle.puzzleNumber = instances;
						_processedPuzzle.name = XMLParser.Instance.puzzles[instances].title;
						_processedPuzzle.number = "Puzzle " + XMLParser.Instance.puzzles[instances].number;

						_processedPuzzle.isDeluxe = false;


						_processedPuzzle.nameDisplay.text = _processedPuzzle.name;
						_processedPuzzle.numberDisplay.text = _processedPuzzle.number;
						
						_processedPuzzle.gridSize = int.Parse(XMLParser.Instance.puzzles[instances].puzzleSize);
						
						if (PlayerPrefs.GetInt (XMLParser.Instance.puzzles[instances].tag) == 1)
						{
							_processedPuzzle.isCompleted = true;
						}
						else
						{
							_processedPuzzle.isCompleted = false;
						}
						
						_processedPuzzle.completedMark.SetActive (_processedPuzzle.isCompleted);
						
						_processedPuzzle.BuildToDisplay();
						
						instances ++;
					}
					//the unlockables
					else
					{
						_tempObject = (GameObject) Instantiate(puzzleSample.gameObject, deluxeUnlockablesParent.transform.position, deluxeUnlockablesParent.transform.rotation);
						_tempObject.transform.SetParent(deluxeUnlockablesParent.transform);
						_tempObject.name = "puzzle-" + XMLParser.Instance.puzzles[instances].number;
						_tempRect = _tempObject.GetComponent<RectTransform>();
						_processedPuzzle = _tempObject.GetComponent<PuzzleButton>();
						_tempRect.localScale = new Vector3(1, 1, 1);
						
						CurrentMagazine.Add (_tempObject);
						Vector3 tempPos;
						
						//do the positioning chnages
						_tempPosition = _tempObject.transform.position;
						_tempPosition.x += SPACE_BETWEEN_PUZZLES_X*deluxUnlockablesProcessed;
						_tempPosition.y = deluxeUnlockablesParent.transform.position.y;
						_tempObject.transform.position = _tempPosition;
						
						
						_processedPuzzle.puzzleNumber = instances;
						_processedPuzzle.name = XMLParser.Instance.puzzles[instances].title;
						_processedPuzzle.number = "Puzzle " + XMLParser.Instance.puzzles[instances].number;

						_processedPuzzle.isDeluxe = true;
						_processedPuzzle.deluxeLocked.SetActive(true);

						_processedPuzzle.nameDisplay.text = _processedPuzzle.name;
						_processedPuzzle.numberDisplay.text = _processedPuzzle.number;
						
						_processedPuzzle.gridSize = int.Parse(XMLParser.Instance.puzzles[instances].puzzleSize);
						
						if (PlayerPrefs.GetInt (XMLParser.Instance.puzzles[instances].tag) == 1)
						{
							_processedPuzzle.isCompleted = true;

							_processedPuzzle.deluxeLocked.SetActive(false);
							_processedPuzzle.deluxeUnlocked.SetActive(true);
						}
						else
						{
							_processedPuzzle.isCompleted = false;

							_processedPuzzle.deluxeLocked.SetActive(true);
							_processedPuzzle.deluxeUnlocked.SetActive(false);

						}
						
						_processedPuzzle.completedMark.SetActive (_processedPuzzle.isCompleted);
						
						//_processedPuzzle.BuildToDisplay();
						
						instances ++;
						deluxUnlockablesProcessed ++;
					}

				}
				
			}

			Vector2 unlockableSize = deluxeUnlockablesParent.GetComponent<RectTransform>().sizeDelta;
			unlockableSize.x = (puzzleSample.GetComponent<RectTransform>().sizeDelta.x*DELUXE_UNLOCKABLE) + (50*DELUXE_UNLOCKABLE);
			deluxeUnlockablesParent.GetComponent<RectTransform>().sizeDelta = unlockableSize;

			Vector2 contentSize = deluxeContentParent.GetComponent<RectTransform>().sizeDelta;
			contentSize.y += puzzleSample.GetComponent<RectTransform>().sizeDelta.y + (SPACE_BETWEEN_PUZZLES_X*2);
			deluxeContentParent.GetComponent<RectTransform>().sizeDelta = contentSize;

			indexedWith = theTotalObjectsAmount-instances;
			rowsProcessed ++;
		}

		//check all unlocked items

		int _tempUnlockedDeluxeItems = 0;
		int _tempUnlock = 0;

		for (int c=0; c<CurrentMagazine.Count-DELUXE_UNLOCKABLE; c++)
		{
			if (CurrentMagazine[c].GetComponent<PuzzleButton>().isCompleted)
			{
				_tempUnlockedDeluxeItems ++;
			}
		}
		_tempUnlock = _tempUnlockedDeluxeItems/DELUXE_UNLOCK_AFTER;
		Debug.Log ("should unlock now" + _tempUnlock);

		for (int x=0; x<_tempUnlock; x++)
		{
			deluxeUnlockablesParent.transform.GetChild(x).GetComponent<PuzzleButton>().deluxeLocked.SetActive(false);
			deluxeUnlockablesParent.transform.GetChild(x).GetComponent<PuzzleButton>().deluxeUnlocked.SetActive(true);
		}

	}

	public void OnLoadMagazinesWithinStack (int index)
	{
		GameObject _tempObject;
		MagazineUnit _processedMagazine;
		RectTransform _tempRect;

		int theTotalObjectsAmount = gameMagazinesLibrary[index].stackContent.Count;
		float theDistanceBetweenObjects = magazineSample.GetComponent<RectTransform>().sizeDelta.x + SPACE_BETWEEN_PUZZLES_X;
		
		int instances = 0;
		int indexedWith = 0;
		
		
		int rows = theTotalObjectsAmount/PUZZLES_PER_ROW;
		Vector3 _tempPosition;
		int rowsProcessed = 0;
		
		indexedWith = theTotalObjectsAmount;


		while (indexedWith > 0)
		{

			for (int c=0; c<PUZZLES_PER_ROW; c++)
			{
				if (instances < theTotalObjectsAmount)
				{
					_tempObject = (GameObject) Instantiate(magazineSample.gameObject, this.transform.position, this.transform.rotation);
					_tempObject.transform.SetParent(stacksParent.transform);
					_tempObject.name = "stMag-" + gameMagazinesLibrary[index].stackContent[c].displayName;
					_tempRect = _tempObject.GetComponent<RectTransform>();
					_processedMagazine = _tempObject.GetComponent<MagazineUnit>();
					_tempRect.localScale = new Vector3(1, 1, 1);

					_tempPosition = stacksParent.transform.position;
					_tempPosition.x += SPACE_BETWEEN_PUZZLES_X*c;
					_tempPosition.y -= SPACE_BETWEEN_PUZZLES_X*rowsProcessed;
					_tempObject.transform.position = _tempPosition;
					
					_processedMagazine.indexedWith = instances;
					_processedMagazine.isStackElement = true;
					_processedMagazine.stackIndex = index;
					_processedMagazine.name = gameMagazinesLibrary[index].stackContent[c].displayName;
					_processedMagazine.logoSprite = gameMagazinesLibrary[index].stackContent[c].displayImage;
					_processedMagazine.nameDisplay.text = _processedMagazine.name;
					_processedMagazine.logoDisplay.sprite = _processedMagazine.logoSprite;

					instances ++;
				}
			}

			indexedWith = theTotalObjectsAmount-instances;
			rowsProcessed ++;
		}
	}

	//
	// STACK -> Magazine -> Puzzles
	//
	public void OnLoadPuzzlesWithinMagazine(int stackIndex, int index)
	{
		//Debug.Log ("<color=green> reading puzzles from stack content magazine! </color>");
		CurrentMagazine.Clear();
		XMLParser.Instance.xmlFile = gameMagazinesLibrary[stackIndex].stackContent[index].dataFile;
		//Debug.Log (XMLParser.Instance.xmlFile.name);
		XMLParser.Instance.BreakdownXML();

		stackContentPanel.SetActive(true);
		stackPanel.SetActive(false);
		
		GameObject _tempObject;
		PuzzleButton _processedPuzzle;
		RectTransform _tempRect;
		
		
		int theTotalObjectsAmount = XMLParser.Instance.puzzles.Count;
		float theDistanceBetweenObjects = puzzleSample.GetComponent<RectTransform>().sizeDelta.x + SPACE_BETWEEN_PUZZLES_X;
		
		int instances = 0;
		int indexedWith = 0;
		
		
		int rows = theTotalObjectsAmount/PUZZLES_PER_ROW;
		Vector3 _tempPosition;
		int rowsProcessed = 0;
		
		indexedWith = theTotalObjectsAmount;
		
		while (indexedWith > 0)
		{
			for (int v=0; v<PUZZLES_PER_ROW; v++)
			{
				if (instances < theTotalObjectsAmount)
				{
					_tempObject = (GameObject) Instantiate(puzzleSample.gameObject, stacksContentParent.transform.position, stacksContentParent.transform.rotation);
					_tempObject.transform.SetParent(stacksContentParent.transform);
					_tempObject.name = "puzzle-" + XMLParser.Instance.puzzles[instances].number;
					_tempRect = _tempObject.GetComponent<RectTransform>();
					_processedPuzzle = _tempObject.GetComponent<PuzzleButton>();
					_tempRect.localScale = new Vector3(1, 1, 1);
					
					CurrentMagazine.Add (_tempObject);
					Vector3 tempPos;
					
					//do the positioning chnages
					_tempPosition = _tempObject.transform.position;
					_tempPosition.x += SPACE_BETWEEN_PUZZLES_X*v;
					_tempPosition.y -= SPACE_BETWEEN_PUZZLES_X*rowsProcessed;
					_tempObject.transform.position = _tempPosition;
					
					
					_processedPuzzle.puzzleNumber = instances;
					_processedPuzzle.name = XMLParser.Instance.puzzles[instances].title;
					_processedPuzzle.number = "Puzzle " + XMLParser.Instance.puzzles[instances].number;
					
					_processedPuzzle.nameDisplay.text = _processedPuzzle.name;
					_processedPuzzle.numberDisplay.text = _processedPuzzle.number;
					
					_processedPuzzle.gridSize = int.Parse(XMLParser.Instance.puzzles[instances].puzzleSize);
					
					if (PlayerPrefs.GetInt (XMLParser.Instance.puzzles[instances].tag) == 1)
					{
						_processedPuzzle.isCompleted = true;
					}
					else
					{
						_processedPuzzle.isCompleted = false;
					}
					
					_processedPuzzle.completedMark.SetActive (_processedPuzzle.isCompleted);
					
					_processedPuzzle.BuildToDisplay();
					
					instances ++;
				}
				
			}
			Vector2 contentSize = stacksContentParent.GetComponent<RectTransform>().sizeDelta;
			contentSize.y += puzzleSample.GetComponent<RectTransform>().sizeDelta.y + (SPACE_BETWEEN_PUZZLES_X*2);
			stacksContentParent.GetComponent<RectTransform>().sizeDelta = contentSize;

			indexedWith = theTotalObjectsAmount-instances;
			rowsProcessed ++;
		}
	}

	public void OnLoadPuzzlesWithinMagazine(int index)
	{
		CurrentMagazine.Clear();
		XMLParser.Instance.xmlFile = gameMagazinesLibrary[index].dataFile;
		XMLParser.Instance.BreakdownXML();


		GameObject _tempObject;
		PuzzleButton _processedPuzzle;
		RectTransform _tempRect;


		int theTotalObjectsAmount = XMLParser.Instance.puzzles.Count;
		float theDistanceBetweenObjects = puzzleSample.GetComponent<RectTransform>().sizeDelta.x + SPACE_BETWEEN_PUZZLES_X;
		
		int instances = 0;
		int indexedWith = 0;


		int rows = theTotalObjectsAmount/PUZZLES_PER_ROW;
		Vector3 _tempPosition;
		int rowsProcessed = 0;
		
		indexedWith = theTotalObjectsAmount;
		
		while (indexedWith > 0)
		{
			for (int v=0; v<PUZZLES_PER_ROW; v++)
			{
				if (instances < theTotalObjectsAmount)
				{
					_tempObject = (GameObject) Instantiate(puzzleSample.gameObject, puzzlesParent.transform.position, puzzlesParent.transform.rotation);
					_tempObject.transform.SetParent(puzzlesParent.transform);
					_tempObject.name = "puzzle-" + XMLParser.Instance.puzzles[instances].number;
					_tempRect = _tempObject.GetComponent<RectTransform>();
					_processedPuzzle = _tempObject.GetComponent<PuzzleButton>();
					_tempRect.localScale = new Vector3(1, 1, 1);

					CurrentMagazine.Add (_tempObject);
					Vector3 tempPos;

					//do the positioning chnages
					_tempPosition = _tempObject.transform.position;
					_tempPosition.x += SPACE_BETWEEN_PUZZLES_X*v;
					_tempPosition.y -= SPACE_BETWEEN_PUZZLES_X*rowsProcessed;
					_tempObject.transform.position = _tempPosition;


					_processedPuzzle.puzzleNumber = instances;
					_processedPuzzle.name = XMLParser.Instance.puzzles[instances].title;
					_processedPuzzle.number = "Puzzle " + XMLParser.Instance.puzzles[instances].number;
					
					_processedPuzzle.nameDisplay.text = _processedPuzzle.name;
					_processedPuzzle.numberDisplay.text = _processedPuzzle.number;
					
					_processedPuzzle.gridSize = int.Parse(XMLParser.Instance.puzzles[instances].puzzleSize);
					
					if (PlayerPrefs.GetInt (XMLParser.Instance.puzzles[instances].tag) == 1)
					{
						_processedPuzzle.isCompleted = true;
					}
					else
					{
						_processedPuzzle.isCompleted = false;
					}
					
					_processedPuzzle.completedMark.SetActive (_processedPuzzle.isCompleted);
					
					_processedPuzzle.BuildToDisplay();

					instances ++;
				}
				
			}
			indexedWith = theTotalObjectsAmount-instances;
			rowsProcessed ++;
		}
	}

	public void OnLoadHome()
	{
		XMLParser.Instance.puzzles.Clear();
		Application.LoadLevel("home");
	}
}
