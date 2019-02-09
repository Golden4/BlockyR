using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class SwitchButton : MonoBehaviour {

	public enum Buttons {
		Audio,
		Input
	}

	public Buttons type;

	Button btn;

	public SwitchInfo[] switchInfos;

	public int curSwitchIndex;

	void Start ()
	{
		btn = GetComponent <Button> ();

		btn.onClick.AddListener (() => {
			SwitchNext ();
		});

		UpdateEvents ();
		Switch (curSwitchIndex);
	}

	void Switch (int index)
	{
		transform.GetChild (0).GetComponent <Image> ().sprite = switchInfos [index].sprite;

		if (switchInfos [index].Event != null)
			switchInfos [index].Event.Invoke ();
	}

	void SwitchNext ()
	{
		curSwitchIndex = (curSwitchIndex + 1) % switchInfos.Length;
		Switch (curSwitchIndex);
	}

	void UpdateEvents ()
	{
		if (type == Buttons.Audio) {
			
			curSwitchIndex = (!AudioManager.Ins.audioEnabled) ? 1 : 0;

			Action[] actions = {
				delegate {
					AudioManager.Ins.EnableAudio (true);
				}, delegate {
					AudioManager.Ins.EnableAudio (false);
				}
			};

			for (int i = 0; i < switchInfos.Length; i++) {
				switchInfos [i].Event = actions [i];
			}

		}

		if (type == Buttons.Input) {
			
		}
	}

	[System.Serializable]
	public class SwitchInfo {
		public Sprite sprite;
		public Action Event;
	}

}
