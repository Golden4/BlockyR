using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MobileInputManager {

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

	public static bool GetKey (Direction dir)
	{
		if (!ContainsKey (dir)) {
			return false;
		}

		return inputs [dir].curState == VInputButton.State.Holding;
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
