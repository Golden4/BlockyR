using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public Direction direction;

	public enum State {
		None,
		Down,
		Holding,
		Up
	}

	[HideInInspector]
	public State curState = State.None;

	bool active = false;

	void OnEnable ()
	{
		active = true;
		MobileInputManager.RegisterInput (this, direction);
	}

	void OnDisable ()
	{
		active = false;
		MobileInputManager.UnRegisterInput (direction);
	}

	bool btnDown = false;
	bool isHolding = false;

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

	public void OnPointerDown (PointerEventData eventData)
	{
		isHolding = true;
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		isHolding = false;
	}

}
