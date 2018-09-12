using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpoilerButton : MonoBehaviour {

	public GameObject spoilerParent;
	public GameObject tapToStart;

	public bool isShow = false;

	void Awake ()
	{
		Show ();
	}

	void Start ()
	{
		GetComponent <Button> ().onClick.AddListener (() => {

			if (isShow) {
				Close ();
			} else {
				Show ();
			}

		});

		Close ();
	}

	void Show ()
	{
		isShow = true;
		tapToStart.SetActive (false);
		spoilerParent.gameObject.SetActive (true);
	}

	void Close ()
	{

		tapToStart.SetActive (true);
		isShow = false;
		spoilerParent.gameObject.SetActive (false);
	}

}
