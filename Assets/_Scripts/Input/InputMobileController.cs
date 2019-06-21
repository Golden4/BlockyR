using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMobileController : MonoBehaviour {

	public static InputMobileController Ins;

	public static InputType curInputType;

	public GameObject[] inputsGO;
	public Button inputInfoBtn;

	public enum InputType
	{
		Button,
		Touch
	}

	void Awake ()
	{
		Ins = this;
	}

	void Start ()
	{
		Game.OnGameStarted += InitInput;
	}

	void InitInput ()
	{
		for (int i = 0; i < inputsGO.Length; i++) {
			inputsGO [i].SetActive ((int)curInputType == i);
		}

		if (curInputType == InputType.Touch) {
			UIScreen.Ins.ShowInputsMap (true);
			inputInfoBtn.gameObject.SetActive (true);

		} else {
			inputInfoBtn.gameObject.SetActive (false);
		}

	}

	void OnDestroy ()
	{
		Game.OnGameStarted -= InitInput;
	}

	public static void ChangeInputType (InputType type)
	{
		curInputType = type;
		Debug.Log ("CurInputType: " + type);

	}
}
