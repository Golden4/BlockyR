using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour {
	public static ScreenController Ins;

	public enum GameScreen {
		Menu,
		Shop,
		UI,
		Continue,
		GameOver,
		Pause,
		Prize
	}

	public ScreenBase[] screensList;

	public static GameScreen curActiveScreen;

	public static System.Action OnChangeScreenEvent;

	void Awake ()
	{
		Ins = this;

		for (int i = 0; i < screensList.Length; i++) {
			screensList [i].Init ();
		}
	}

	void Start ()
	{
		if (System.Enum.GetNames (typeof(GameScreen)).Length != screensList.Length) {
			Debug.LogError ("GameScreen Count: " + System.Enum.GetNames (typeof(GameScreen)).Length + " != screensList: " + screensList.Length);
		}

		//ActivateScreen (GameScreen.Prize);
		ActivateScreen (GameScreen.Menu);

	}

	/*	public static void ActivateScreen<T> (T screen) where T:ScreenBase
	{
		for (int i = 0; i < Ins.screensList.Length; i++) {
			if (typeof(T) == Ins.screensList [i].GetType ()) {
				ActivateScreen (i);
			}
		}
	}*/

	public void ActivateScreen (GameScreen screen)
	{
		ActivateScreen ((int)screen);
	}

	public void ActivateScreen (int screen)
	{
		for (int i = 0; i < screensList.Length; i++) {
			if (i == screen) {
				screensList [i].Activate ();
			} else {
				screensList [i].Deactivate ();
			}
				
		}

		curActiveScreen = (GameScreen)screen;

		if (OnChangeScreenEvent != null) {
			OnChangeScreenEvent.Invoke ();
		}

	}

	void OnDestroy ()
	{
		for (int i = 0; i < screensList.Length; i++) {
			screensList [i].OnCleanUp ();
		}
	}

	void OnApplicationQuit ()
	{
		
	}

	void OnApplicationFocus (bool pause)
	{
		if (Game.isGameStarted && pause)
			ActivateScreen (GameScreen.Pause);
	}

	void OnApplicationPause (bool pause)
	{
		if (Game.isGameStarted && !pause)
			ActivateScreen (GameScreen.Pause);
	}

}
