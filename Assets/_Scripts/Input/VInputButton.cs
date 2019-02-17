using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, IDragHandler, IEndDragHandler, IBeginDragHandler {

	public Direction direction;

	public enum State
	{
		None,
		Down,
		Holding,
		Up
	}

	//[HideInInspector]
	public State curState = State.None;

	bool active = false;

	void OnEnable ()
	{
		active = true;
		isHolding = false;
		MobileInputTouchManager.RegisterInput (this, direction);
		curState = State.None;
	}

	void OnDisable ()
	{
		active = false;
		isHolding = false;
		MobileInputTouchManager.UnRegisterInput (direction);
		curState = State.None;
	}

	bool btnDown = false;
	bool isHolding = false;
	public bool isEnter = false;

	void Update ()
	{
		if (!active)
			return;

		if (isHolding) {
			
			if (!btnDown) {
				btnDown = true;
				curState = State.Down;

			} else
				curState = State.Holding;

		} else if (btnDown) {
			btnDown = false;
			curState = State.Up;
		} else {
			curState = State.None;
		}

	}

	public void OnPointerExit (PointerEventData eventData)
	{
		isEnter = false;
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		isEnter = true;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		isHolding = true;
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		isHolding = false;
	}

	public void OnDrag (PointerEventData eventData)
	{
		isHolding = true;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		isHolding = false;
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		isHolding = true;
	}


}
