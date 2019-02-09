using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeScreen : ScreenBase {
	public static PrizeScreen Ins;
	static GameObject prizeGO;
	public Transform prizeHolder;
	public Animation prizeBoxAnimation;
	public Button selectAndPlayBtn;
	public Button openBoxBtn;
	public Button backBtn;
	public Text nameText;
	public Text abilityText;
	private bool isOpenedBox;
	private bool isCharacterShowed;
	private int charactedIndex = -1;

	public override void Init ()
	{
		base.Init ();
		Ins = this;

		openBoxBtn.onClick.RemoveAllListeners ();
		openBoxBtn.onClick.AddListener (OpenBox);
		openBoxBtn.gameObject.SetActive (true);
		backBtn.gameObject.SetActive (false);

		selectAndPlayBtn.gameObject.SetActive (false);
		selectAndPlayBtn.onClick.RemoveAllListeners ();
		selectAndPlayBtn.onClick.AddListener (() => {
			User.SetPlayerIndex (charactedIndex);
			SceneController.RestartLevel ();
		});

		backBtn.onClick.RemoveAllListeners ();
		backBtn.onClick.AddListener (() => {
			SceneController.RestartLevel ();
			ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.Menu);

		});
	}

	public void GivePrize ()
	{
		
	}

	void Update ()
	{
		if (isOpenedBox && !prizeBoxAnimation.isPlaying && !isCharacterShowed) {
			isCharacterShowed = true;
			CharacterShow ();
		}
	}

	void CharacterShow ()
	{
		if (charactedIndex != -1) {
			nameText.gameObject.SetActive (true);
			abilityText.gameObject.SetActive (true);
			nameText.text = Database.Get.playersData [charactedIndex].name;
			abilityText.text = Database.Get.playersData [charactedIndex].ability;
			selectAndPlayBtn.gameObject.SetActive (true);
			backBtn.gameObject.SetActive (true);
		}
	}

	public void OpenBox ()
	{
		isOpenedBox = true;
		charactedIndex = GetRandomCharacter ();
		prizeBoxAnimation.Play ("PrizeBoxOpenAnim");
		openBoxBtn.gameObject.SetActive (false);
		User.GetInfo.userData [charactedIndex].bought = true;
	}

	public override void OnActivate ()
	{
		base.OnActivate ();
		prizeBoxAnimation.Play ("PrizeBoxIdleAnim");
	}

	public static int GetRandomCharacter ()
	{
		int index = GetRandomAvaibleCharacterIndex ();

		if (index > -1) {

			prizeGO = new GameObject ("Prize");
			prizeGO.transform.SetParent (Ins.prizeHolder);
			prizeGO.transform.localPosition = Vector3.zero;
			prizeGO.transform.localEulerAngles = Vector3.up * 90;
			prizeGO.transform.localScale = Vector3.one * 0.6f;
			prizeGO.layer = LayerMask.NameToLayer ("ShopItem");

			MeshFilter mf = prizeGO.AddComponent <MeshFilter> ();
			MeshRenderer mr = prizeGO.AddComponent <MeshRenderer> ();

			mf.sharedMesh = Database.Get.playersData [index].playerPrefab.GetComponent <MeshFilter> ().sharedMesh;
			mr.sharedMaterials = Database.Get.playersData [index].playerPrefab.GetComponent <MeshRenderer> ().sharedMaterials;
			mf.sharedMesh.RecalculateNormals ();

		} else {
			Debug.Log ("All characters bought");
		}
		return index;
	}

	static int GetRandomAvaibleCharacterIndex ()
	{
		int index = -1;

		List<int> avaibleIndexes = new List<int> ();

		for (int i = 0; i < User.GetInfo.userData.Length; i++) {
			if (!User.GetInfo.userData [i].bought) {
				avaibleIndexes.Add (i);
			}
		}

		if (avaibleIndexes.Count > 0)
			index = avaibleIndexes [Random.Range (0, avaibleIndexes.Count)];

		return index;
	}

}
