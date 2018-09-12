using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	Vector3 prevPos = Vector3.zero;

	public void OnPointerDown (PointerEventData eventData)
	{
		if (prevPos == Vector3.zero)
			prevPos = transform.GetChild (0).localPosition;
		
		transform.GetChild (0).localPosition = prevPos - Vector3.up * 5;
	}


	public void OnPointerUp (PointerEventData eventData)
	{
		transform.GetChild (0).localPosition = prevPos;
	}

}
