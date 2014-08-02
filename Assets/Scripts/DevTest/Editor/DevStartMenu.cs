using UnityEngine;
using UnityEditor;
using System.Collections;

public class DevStartMenu : EditorWindow
{
	[MenuItem("Test/DevRun %#p")]
	static void Run()
	{
		EditorApplication.OpenScene("Assets/Scenes/DevTest.unity");
		EditorApplication.isPlaying = true;
	}
}
