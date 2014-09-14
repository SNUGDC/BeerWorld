using UnityEngine;
using UnityEditor;
using System.Collections;

public class DevStartMenu : EditorWindow
{
	[MenuItem("Test/DevRun %#p")]
	static void RunDevRun()
	{
		EditorApplication.OpenScene("Assets/Scenes/Util/DevTest.unity");
		EditorApplication.isPlaying = true;
	}

	[MenuItem("Test/DevRun(BossBattle) %#l")]
	static void RunBossBattle()
	{
		EditorApplication.OpenScene("Assets/Scenes/Util/DevTestBossBattle.unity");
		EditorApplication.isPlaying = true;
	}
}
