using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

	public GameObject[] items;

	public Transform itemsHolder;

	ScrollRect sr;

	public event System.Action<int> OnChangeItemEvent;

	public float ContentSize {
		get {
			return 200 + 150 * items.Length + 87;
		}
	}

	public int GetCurItemIndex {
		get {
			return (-Mathf.FloorToInt (sr.content.localPosition.x) + 70) / 150;
		}
	}

	bool dragging;

	public void Init ()
	{		
		sr = GetComponent <ScrollRect> ();

		SpawnItemsFromData ();

		sr.content.sizeDelta = new Vector2 (ContentSize, sr.content.sizeDelta.y);
	}

	void SpawnItemsFromData ()
	{
		items = new GameObject[Database.Get.playersData.Length];
		for (int i = 0; i < Database.Get.playersData.Length; i++) {
			items [i] = new GameObject (i + "Player");
			items [i].transform.SetParent (itemsHolder);
			items [i].transform.localPosition = Vector3.forward * 1.5f * i;
			items [i].transform.localEulerAngles = Vector3.zero;
			items [i].layer = LayerMask.NameToLayer ("ShopItem");

			MeshFilter mf = items [i].AddComponent <MeshFilter> ();
			MeshRenderer mr = items [i].AddComponent <MeshRenderer> ();

			mf.sharedMesh = Database.Get.playersData [i].playerPrefab.GetComponent <MeshFilter> ().sharedMesh;
			mr.sharedMaterials = Database.Get.playersData [i].playerPrefab.GetComponent <MeshRenderer> ().sharedMaterials;
			mf.sharedMesh.RecalculateNormals ();
		}
	}

	public Color boughtColor;
	public Color notBoughtColor;

	public void SetItemState (int itemIndex, bool bought)
	{
		MeshRenderer mr = items [itemIndex].GetComponent <MeshRenderer> ();

		Material newMat = new Material (mr.material);

		newMat.color = (bought) ? boughtColor : notBoughtColor;

		mr.material = newMat;

	}

	int lastItemIndex = -10;

	void Update ()
	{

		FocusToObject (GetCurItemIndex);

		if (!dragging && sr.velocity.magnitude < 50) {
			SnapToObj (GetCurItemIndex);
		}


		if (lastItemIndex != GetCurItemIndex) {
			lastItemIndex = GetCurItemIndex;

			if (OnChangeItemEvent != null)
				OnChangeItemEvent (GetCurItemIndex);

		}

	}


	void FocusToObject (int index)
	{
		for (int i = 0; i < items.Length; i++) {

			float targetScale = (i == index) ? 1.6f : 1;

			items [i].transform.localScale = Vector3.Lerp (items [i].transform.localScale, Vector3.one * targetScale, Time.deltaTime * 8);

			if (User.GetInfo.userData [i].bought)
				items [i].transform.localEulerAngles = new Vector3 (0, items [i].transform.localEulerAngles.y + Time.deltaTime * 45, 0);
			else {
				items [i].transform.localEulerAngles = new Vector3 (0, 45, 0);

			}
		}
	}

	public void SnapToObj (int index, bool lerp = true)
	{
		Vector3 pos = sr.content.localPosition;
		pos.x = -index * 150;
		if (lerp)
			sr.content.localPosition = Vector3.Lerp (sr.content.localPosition, pos, Time.deltaTime * 20);
		else
			sr.content.localPosition = pos;

	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		dragging = true;

	}

	public void OnEndDrag (PointerEventData eventData)
	{
		dragging = false;
	}
}
