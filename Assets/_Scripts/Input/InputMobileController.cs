using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMobileController : MonoBehaviour {

	public static InputMobileController Ins;

	public static InputType curInputType;

	public enum InputType
	{
		Touch,
		Swipe
	}

	void Awake ()
	{
		Ins = this;
	}

	void Start ()
	{
        
	}

	void Update ()
	{
        
	}

	public static void ChangeInputType (InputType type)
	{
		Debug.Log ("CurInputType: " + type);

	}
}
