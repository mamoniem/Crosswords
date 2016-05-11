using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;

public class LettersCounter : EditorWindow {

	static LettersCounter AssetsUpdaterWindow;
	const int WINDOW_MIN_SIZE_X = 300;
	const int WINDOW_MIN_SIZE_Y = 120;
	const int WINDOW_MAX_SIZE_X = 300;
	const int WINDOW_MAX_SIZE_Y = 120;
	const int WINDOW_SIZE_X = 300;
	const int WINDOW_SIZE_Y = 120;
	const string WINDOW_NAME = "Letters Counter";
	
	string textToCount;
	string lettersCountingResult = "******";

	[MenuItem ("Window/Crossword-Tools/Letters Counter")]
	
	static void Init () {
		AssetsUpdaterWindow = (LettersCounter)EditorWindow.GetWindow (typeof(LettersCounter));
		AssetsUpdaterWindow.autoRepaintOnSceneChange = true;
		//AssetsUpdaterWindow.minSize = new Vector2(WINDOW_MIN_SIZE_X, WINDOW_MIN_SIZE_Y);
		AssetsUpdaterWindow.maxSize = new Vector2(WINDOW_MAX_SIZE_X, WINDOW_MAX_SIZE_Y);
		AssetsUpdaterWindow.position = new Rect(Screen.width/2,300.0f, WINDOW_SIZE_X,WINDOW_SIZE_Y);
		AssetsUpdaterWindow.title = WINDOW_NAME;
		AssetsUpdaterWindow.Show();
		AssetsUpdaterWindow.Focus();
	}
	
	void OnGUI () {

		textToCount = EditorGUILayout.TextArea(textToCount, GUILayout.Height(position.height - 45));

		EditorGUILayout.PrefixLabel(lettersCountingResult);

		if (GUILayout.Button("-=<Count>=-"))
		{
			if (textToCount != null)
			{
				lettersCountingResult = "There is : [" + textToCount.Length.ToString() + "] letter";
			}
			else
			{
				lettersCountingResult = "Please input a text";
			}
		}
	}
}
