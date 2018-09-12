using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {
	public static AudioManager Ins;

	AudioSource source;

	public SoundLibrary soundLibrary;

	void Awake ()
	{
		if (Ins == null) {
			Ins = this;
			DontDestroyOnLoad (gameObject);
		} else if (Ins != this) {
			Destroy (gameObject);
			return;
		}

		source = gameObject.AddComponent<AudioSource> ();

		soundLibrary = GetComponent <SoundLibrary> ();
	}

	public void PlaySound (Sound sound)
	{
		PlaySound (sound.clip);
	}

	public void PlaySound (AudioClip sound)
	{
		if (!audioEnabled)
			return;
		
		source.PlayOneShot (sound);
	}

	public bool audioEnabled = true;

	public void EnableAudio (bool enable)
	{
		audioEnabled = enable;
		print ("Audio " + enable);
	}

	public static void PlaySoundFromLibrary (string name)
	{
		Ins.PlaySound (Ins.soundLibrary.GetSoundByName (name));
	}

}

[System.Serializable]
public class Sound {
	
	public AudioClip clip;

	public Sound (AudioClip clip)
	{
		this.clip = clip;
	}
	
}
