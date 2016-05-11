using UnityEngine;
using UnityEditor;

using System.Collections;

public class BuildSet : MonoBehaviour {

	[MenuItem ("Window/Crossword-Tools/Set Build For/Debug")]
	static void SetAsDebug () {
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "DEBUG_BUILD");
	}

	[MenuItem ("Window/Crossword-Tools/Set Build For/Release")]
	static void SetAsRelease () {
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "RELEASE_BUILD");
	}
}
