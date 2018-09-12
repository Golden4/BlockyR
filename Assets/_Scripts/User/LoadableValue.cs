using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable]
public struct LoadableValue {
	int offset;
	int valueWithOffset;
	bool loaded;

	public static int LoadValue (string key)
	{
		int loadedValue = 0;

		if (!loaded) {
			loaded = true;

			if (PlayerPrefs.HasKey (key))
				loadedValue = PlayerPrefs.GetInt (key);
		}

		return loadedValue;
	}

	public void SaveValue (string key)
	{
		PlayerPrefs.SetInt (key, value);
	}

	public LoadableValue (string key)
	{
		int loadedValue = LoadValue (key);

		offset = UnityEngine.Random.Range (-1000, 1000);

		this.valueWithOffset = loadedValue + offset;

	}

	public LoadableValue (int value)
	{
		offset = UnityEngine.Random.Range (-1000, 1000);

		this.valueWithOffset = value + offset;
	}

	public int value {
		get {
			return valueWithOffset - offset;
		}

		set {
			valueWithOffset = value + offset;
		}
	}

	public static LoadableValue operator+ (LoadableValue value1, LoadableValue value2)
	{
		value1.value = (value1.value + value2.value);
		return value1;
	}

	public static LoadableValue operator- (LoadableValue value1, LoadableValue value2)
	{
		value1.value = (value1.value - value2.value);
		return value1;
	}

	public static LoadableValue operator+ (LoadableValue value1, int value2)
	{
		value1.value = (value1.value + value2);
		return value1;
	}

	public static LoadableValue operator- (LoadableValue value1, int value2)
	{
		value1.value = (value1.value - value2);
		return value1;
	}

	public static implicit operator int (LoadableValue value)
	{
		return value.value;
	}

	public static implicit operator LoadableValue (int value)
	{
		return new LoadableValue (value);
	}

	public override string ToString ()
	{
		return string.Format ("Value = {0}", value);
	}
}*/
