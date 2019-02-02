using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuScreen : ScreenBase {

	public static event System.Action OnStartGame;

	void Start ()
	{
		SceneController.Init ();
	}

	void Update ()
	{
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN

		if (Input.anyKeyDown && !EventSystem.current.IsPointerOverGameObject ()) {
			
			StartGame ();
		}
		#else
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
			if (!EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId))
				StartGame ();
		}

		#endif
	}

	public void StartGame ()
	{
		ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.UI);

		SceneController.ShowGameTitle (false, true);

		if (OnStartGame != null)
			OnStartGame.Invoke ();
	}

	public void ShowShopScreen ()
	{
		ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.Shop);
	}
}
