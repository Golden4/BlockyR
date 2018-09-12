using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class DatabaseWindow : EditorWindow {

	void OnGUI ()
	{
		
	}

	[MenuItem ("Window/Database")]
	public static void ShowWindow ()
	{
		GetWindow <DatabaseWindow> ("Database");
	}

}
#endif
