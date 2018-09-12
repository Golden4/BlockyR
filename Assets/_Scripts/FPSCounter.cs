using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {
	Text text;
	float deltaTime;

	void Start ()
	{
		text = GetComponent <Text> ();
	}

	void Update ()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * .1f;

		float fps = 1f / deltaTime;

		string textt = string.Format ("{0:0.} fps", fps);

		text.text = textt;

	}

}
