using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class FileReader : EditorWindow {

	static FileReader AssetsUpdaterWindow;
	const int WINDOW_MIN_SIZE_X = 300;
	const int WINDOW_MIN_SIZE_Y = 400;
	const int WINDOW_MAX_SIZE_X = 300;
	const int WINDOW_MAX_SIZE_Y = 400;
	const int WINDOW_SIZE_X = 300;
	const int WINDOW_SIZE_Y = 400;
	const string WINDOW_NAME = "File Data";

	Vector2 scrollPos;

	public class clue
	{
		public string type;
		public string number;
		public string primary;
		public string alternate;
	}
	
	public class aPuzzle
	{
		public string number;
		public string difficulty;
		public string tag;
		public string title;
		public string puzzleSize;
		public string puzzleCutout;
		public string puzzleSolid;
		public string puzzleData;
		
		public List<clue> acrossClues = new List<clue>();
		public List<clue> downClues = new List<clue>();
	}
	
	public List<aPuzzle> puzzles = new List<aPuzzle>();
	
	TextAsset theXMLFile;
	string Output;

	[MenuItem ("Window/Crossword-Tools/File Data Checker")]
	
	static void Init () {
		AssetsUpdaterWindow = (FileReader)EditorWindow.GetWindow (typeof(FileReader));
		AssetsUpdaterWindow.autoRepaintOnSceneChange = true;
		//AssetsUpdaterWindow.minSize = new Vector2(WINDOW_MIN_SIZE_X, WINDOW_MIN_SIZE_Y);
		AssetsUpdaterWindow.maxSize = new Vector2(WINDOW_MAX_SIZE_X, WINDOW_MAX_SIZE_Y);
		AssetsUpdaterWindow.position = new Rect(Screen.width/2,300.0f, WINDOW_SIZE_X,WINDOW_SIZE_Y);
		AssetsUpdaterWindow.title = WINDOW_NAME;
		AssetsUpdaterWindow.Show();
		AssetsUpdaterWindow.Focus();
	}
	
	void OnGUI () {

		theXMLFile = (TextAsset)EditorGUILayout.ObjectField("XML File :", theXMLFile, typeof (TextAsset), true);

		EditorGUILayout.BeginHorizontal();
		GUI.color = Color.green;
		if (GUILayout.Button("Load"))
		{
			if (theXMLFile != null)
			{
				puzzles.Clear();
				puzzles = new List<aPuzzle>();
				Output = "";
				//Output = theXMLFile.text;
				GetAllData();
			}
			else
			{
				Output = "Please input a text";
			}
		}

		GUI.color = Color.red;
		if (GUILayout.Button("Clear"))
		{
			theXMLFile = null;
			Output = "";
		}
		EditorGUILayout.EndHorizontal();

		GUI.color = Color.white;
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width (WINDOW_MAX_SIZE_X), GUILayout.Height (300));
		GUILayout.Label(Output);
		//Output = EditorGUILayout.TextArea(Output, GUILayout.Height(position.height - 45));
		EditorGUILayout.EndScrollView();
	}

	void GetAllData()
	{
		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		xmlDoc.LoadXml(theXMLFile.text); // load the file.
		XmlNodeList puzzlesList = xmlDoc.GetElementsByTagName("crossword_puzzle"); // array of the level nodes.
		
		Output += "Total Puzzles inside the file is[" + puzzlesList.Count + "]" ;
		Output += "\n******************************************";

		foreach (XmlNode puzzleInfo in puzzlesList)
		{
			aPuzzle _tempPuzzle = new aPuzzle();
			
			_tempPuzzle.number = puzzleInfo.Attributes["number"].Value;
			_tempPuzzle.difficulty = puzzleInfo.Attributes["difficulty"].Value;
			_tempPuzzle.tag = puzzleInfo.Attributes["tag"].Value;
			
			//Debug.Log (puzzleInfo.Attributes["number"].Value);
			//Debug.Log (puzzleInfo.Attributes["difficulty"].Value);
			//Debug.Log (puzzleInfo.Attributes["tag"].Value);
			Output += "\nPuzzle Number[" + _tempPuzzle.number + "]" ;
			Output += "\nPuzzle Difficulty[" + _tempPuzzle.difficulty + "]" ;
			Output += "\nPuzzle Tag[" + _tempPuzzle.tag + "]" ;
			
			XmlNodeList puzzlecontent = puzzleInfo.ChildNodes;
			
			foreach (XmlNode puzzleNode in puzzlecontent)
			{
				if(puzzleNode.Name == "title")
				{
					_tempPuzzle.title = puzzleNode.InnerText;
					Output += "\nPuzzle Title[" + _tempPuzzle.title + "]" ;
					//Debug.Log (puzzleNode.InnerText);
				}
				
				if(puzzleNode.Name == "grid")
				{
					_tempPuzzle.puzzleSize = puzzleNode.Attributes["size"].Value;
					_tempPuzzle.puzzleCutout = puzzleNode.Attributes["cutout"].Value;
					_tempPuzzle.puzzleSolid = puzzleNode.Attributes["solid"].Value;
					_tempPuzzle.puzzleData = puzzleNode.InnerText;
					
					//Debug.Log (puzzleNode.Attributes["size"].Value);
					//Debug.Log (puzzleNode.Attributes["cutout"].Value);
					//Debug.Log (puzzleNode.Attributes["solid"].Value);
					//Debug.Log (puzzleNode.InnerText);
					Output += "\nPuzzle Size[" + _tempPuzzle.puzzleSize + "]" ;
					Output += "\nPuzzle Cutout[" + _tempPuzzle.puzzleCutout + "]" ;
					Output += "\nPuzzle Solid[" + _tempPuzzle.puzzleSolid + "]" ;
					Output += "\nPuzzle Data[" + _tempPuzzle.puzzleData + "]" ;
					Output += "\n******************************************";
				}

				//Output += "\n******************************************";
				//Output += "\n***********[ACROSS CLUES]*****************";
				//Output += "\n******************************************";
				if(puzzleNode.Name == "across")
				{
					XmlNodeList clues = puzzleNode.ChildNodes;
					foreach (XmlNode clue in clues)
					{
						clue _tempClue = new clue();
						_tempClue.type = "across";
						_tempClue.number = clue.Attributes["number"].Value;
						
						//Debug.Log ("The across Clue number [" + clue.Attributes["number"].Value + "]");
						
						XmlNodeList clueTypes = clue.ChildNodes;
						foreach (XmlNode clueType in clueTypes)
						{
							if(clueType.Name == "primary")
							{
								_tempClue.primary = clueType.InnerText;
								//Debug.Log ("Primary Clue is " + clueType.InnerText);
							}
							
							if(clueType.Name == "alternate")
							{
								_tempClue.alternate = clueType.InnerText;
								//Debug.Log ("Alternate Clue is " + clueType.InnerText);
							}
						}
						_tempPuzzle.acrossClues.Add(_tempClue);
						//Output += "\n[" + _tempClue.number + "] ---- Primary[" + _tempClue.primary +  "] ---- Alt[" + _tempClue.alternate + "]";
					}
				}

				//Output += "\n******************************************";
				//Output += "\n***********[DOWN CLUES]*******************";
				//Output += "\n******************************************";
				if(puzzleNode.Name == "down")
				{
					XmlNodeList clues = puzzleNode.ChildNodes;
					foreach (XmlNode clue in clues)
					{
						clue _tempClue = new clue();
						_tempClue.type = "down";
						_tempClue.number = clue.Attributes["number"].Value;
						
						//Debug.Log ("The across Clue number [" + clue.Attributes["number"].Value + "]");
						
						XmlNodeList clueTypes = clue.ChildNodes;
						foreach (XmlNode clueType in clueTypes)
						{
							if(clueType.Name == "primary")
							{
								_tempClue.primary = clueType.InnerText;
								//Debug.Log ("Primary Clue is " + clueType.InnerText);
							}
							
							if(clueType.Name == "alternate")
							{
								_tempClue.alternate = clueType.InnerText;
								//Debug.Log ("Alternate Clue is " + clueType.InnerText);
							}
						}
						_tempPuzzle.downClues.Add(_tempClue);
						//Output += "\n[" + _tempClue.number + "] ---- Primary[" + _tempClue.primary +  "] ---- Alt[" + _tempClue.alternate + "]";
					}
				}
			}
			puzzles.Add(_tempPuzzle);
		}
		Debug.Log (puzzles.Count);
	}
}
