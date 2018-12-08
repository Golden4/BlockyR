using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdController : MonoBehaviour {

	public string AdID;

	public static AdController Ins;

	void Start ()
	{
		Ins = this;
	}

	void Update ()
	{
		
	}
}
