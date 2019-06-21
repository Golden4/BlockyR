using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpoilerButton : MonoBehaviour {

	public GameObject spoilerParent;
	public GameObject tapToStart;

	public bool isShow = false;

	void Start ()
	{
		Show ();

		GetComponent <Button> ().onClick.AddListener (() => {

			if (isShow) {
				Close ();
			} else {
				Show ();
			}

		});

		Close ();
	}

	public void Show ()
	{
		isShow = true;
		tapToStart.SetActive (false);
		spoilerParent.gameObject.SetActive (true);
		GUIAnimSystem.Instance.MoveIn (spoilerParent.transform, true);
	}

	public void Close ()
	{

		tapToStart.SetActive (true);
		isShow = false;
		spoilerParent.gameObject.SetActive (false);
	}

}
