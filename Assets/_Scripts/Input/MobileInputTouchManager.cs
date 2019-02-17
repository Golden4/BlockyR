using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MobileInputTouchManager {

	static Dictionary<Direction, VInputButton> inputs = new Dictionary<Direction, VInputButton> ();

	public static bool GetKeyUp (Direction dir)
	{
		if (!ContainsKey (dir)) {
			return false;
		}

		return inputs [dir].curState == VInputButton.State.Up;
	}

	public static bool GetKeyDown (Direction dir)
	{
		if (!ContainsKey (dir)) {
			return false;
		}

		return inputs [dir].curState == VInputButton.State.Down;
	}

	public static bool GetKey (Direction dir, bool exitCheck = false)
	{
		if (!ContainsKey (dir)) {
			return false;
		}

		bool key;

		if (exitCheck) {
			key = inputs [dir].curState == VInputButton.State.Holding && inputs [dir].isEnter;
		} else {
			key = inputs [dir].curState == VInputButton.State.Holding && (exitCheck);
		}

		return key;
	}

	static bool ContainsKey (Direction dir)
	{
		if (!inputs.ContainsKey (dir)) {
			//Debug.LogError ("Not found input: " + dir);

			return false;
		}

		return true;
	}

	public static void RegisterInput (VInputButton input, Direction dir)
	{
		inputs [dir] = input;
	}

	public static void UnRegisterInput (Direction dir)
	{
		inputs.Remove (dir);
	}


}
