using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeScreen : ScreenBase {
	public static PrizeScreen Ins;
	private GameObject prizeGO;
	public Transform prizeHolder;
	public Animation prizeBoxAnimation;
	public Button selectAndPlayBtn;
	public Button openBoxBtn;
	public Button backBtn;
	public Text nameText;
	public Text abilityText;
	public GUIAnim rayBG;
	public ParticleSystem confettiParticle;
	private bool isOpenedBox;
	private bool isCharacterShowed;
	private int charactedIndex = -1;

	public static int GetBoxPrise ()
	{
		return 150;
	}

	public override void Init ()
	{
		base.Init ();
		Ins = this;

		openBoxBtn.onClick.RemoveAllListeners ();

		openBoxBtn.onClick.AddListener (() => {
			if (User.BuyWithCoin (GetBoxPrise ()))
				OpenBox ();
		});

		selectAndPlayBtn.onClick.RemoveAllListeners ();
		selectAndPlayBtn.onClick.AddListener (() => {
			User.SetPlayerIndex (charactedIndex);
			SceneController.RestartLevel ();
		});

		/*backBtn.onClick.RemoveAllListeners ();
		backBtn.onClick.AddListener (() => {
			ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.Shop);
		});*/
	}

	void Update ()
	{
		if (isOpenedBox && !prizeBoxAnimation.isPlaying && !isCharacterShowed) {
			isCharacterShowed = true;
			CharacterShow ();
		}

		if (rayBG.gameObject.activeSelf) {
			rayBG.transform.Rotate (0, 0, 10f * Time.deltaTime);
		}

		if (isCharacterShowed) {
			prizeGO.transform.localEulerAngles = new Vector3 (0, prizeGO.transform.localEulerAngles.y + Time.deltaTime * 45, 0);
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
		User.SaveUserInfo ();
		rayBG.gameObject.SetActive (true);
		rayBG.MoveIn (GUIAnimSystem.eGUIMove.Self);
		backBtn.gameObject.SetActive (false);
		confettiParticle.Play ();
	}

	public override void OnActivate ()
	{
		base.OnActivate ();
		prizeBoxAnimation.Play ("PrizeBoxIdleAnim");
		openBoxBtn.gameObject.SetActive (true);
		backBtn.gameObject.SetActive (true);
		rayBG.gameObject.SetActive (false);
		selectAndPlayBtn.gameObject.SetActive (false);
		isOpenedBox = false;
		charactedIndex = -1;
		nameText.gameObject.SetActive (false);
		abilityText.gameObject.SetActive (false);
		isCharacterShowed = false;
		confettiParticle.Clear ();
		if (prizeGO != null)
			Destroy (prizeGO);
	}

	public override void OnDeactivate ()
	{
		base.OnDeactivate ();

	}

	public static int GetRandomCharacter ()
	{
		int index = GetRandomAvaibleCharacterIndex ();

		if (index > -1) {

			Ins.prizeGO = new GameObject ("Prize");
			Ins.prizeGO.transform.SetParent (Ins.prizeHolder);
			Ins.prizeGO.transform.localPosition = Vector3.zero;
			Ins.prizeGO.transform.localEulerAngles = Vector3.up * 90;
			Ins.prizeGO.transform.localScale = Vector3.one * 0.6f;
			Ins.prizeGO.layer = LayerMask.NameToLayer ("ShopItem");

			MeshFilter mf = Ins.prizeGO.AddComponent <MeshFilter> ();
			MeshRenderer mr = Ins.prizeGO.AddComponent <MeshRenderer> ();

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
