using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
	static SceneController Ins;

	public Image image;

	void Awake ()
	{
		if (Ins == null)
			Ins = this;
		else if (Ins != this) {
			DestroyImmediate (gameObject);
		}

		image.raycastTarget = false;

	}

	void Start ()
	{
		DontDestroyOnLoad (gameObject);
	}

	public static void LoadSceneWithFade (int index)
	{
		Ins.StartCoroutine (LoadSceneCoroutine (index));
	}

	public static void RestartLevel ()
	{
		LoadSceneWithFade (SceneManager.GetActiveScene ().buildIndex);
	}

	static IEnumerator LoadSceneCoroutine (int index)
	{
		
		Ins.image.raycastTarget = true;

		yield return FadeImage (Ins.image, true, .2f);

		AsyncOperation ao = SceneManager.LoadSceneAsync (index);
		ao.allowSceneActivation = false;

		while (ao.progress < .89f) {
			yield return null;
		}

		ao.allowSceneActivation = true;

		Ins.image.raycastTarget = false;

		yield return FadeImage (Ins.image, false, .2f);


	}

	static IEnumerator FadeImage (Image image, bool fadeIn, float time)
	{
		float alpha = 0;

		Color color = image.color;

		while (alpha < 1) {
			alpha += Time.deltaTime / time;
			color.a = (fadeIn) ? alpha : 1 - alpha;
			image.color = color;
			yield return null;
		}

		color.a = (fadeIn) ? 1 : 0;
		image.color = color;

	}



}
