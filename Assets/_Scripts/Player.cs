using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof(Rigidbody))]
public class Player : MonoBehaviour {
	public static Player Ins;

	public static event System.Action OnPlayerDie;

	Vector2I curCoord = new Vector2I ();
	Rigidbody rb;

	//[SerializeField]
	float speed = 4;
	float speedChangeMultiply = 3;

	[HideInInspector]
	public bool isDead = false;

	public static bool isWaitingOnStart = true;
	float waitingTime = .8f;

	void Awake ()
	{
		Ins = this;
		rb = GetComponent <Rigidbody> ();
		rb.useGravity = false;
		rb.isKinematic = true;
		targetScale = transform.localScale;
		whiteMat = Resources.Load <Material> ("Materials/WhiteMaterial");
		Game.OnGameStarted += OnGameStart;
		gameObject.AddComponent <AudioListener> ();
	}

	void OnGameStart ()
	{
		isWaitingOnStart = true;
		ShowPlayerWarning (true);

		Utility.Invoke (this, waitingTime, () => {
			isWaitingOnStart = false;
			ShowPlayerWarning (false);
		});

	}

	float lastMoveTime = -1;

	Direction curDir;
	bool dirApply;

	void Update ()
	{

		if (isDead || !Game.isGameStarted || Game.isPause || isWaitingOnStart)
			return;

		if (blocksAround [0] == null) {
			CheckBlocksAndCollidersAround ((Direction)0);
		}

		for (int i = 0; i < 4; i++)
			if (MobileInputManager.GetKey ((Direction)i)) {
				
				Move ((Direction)i);
			}

		CheckDirection ();

		/*if (Input.GetKey (KeyCode.A))
			Move (Direction.Left);		
		if (Input.GetKey (KeyCode.W))
			Move (Direction.Up);
		if (Input.GetKey (KeyCode.S))
			Move (Direction.Down);*/
		//if (Input.GetKey (KeyCode.D))

		/*if (blocksAround [(int)curDir].GetType () == typeof(BlockBalk)) {
			isWaitingBalk = true;
		} else {
			isWaitingBalk = false;
		}*/

		if (!isSnaped /*&& !isWaitingBalk*/)
			Move (curDir);
		
		UIScreen.Ins.UpdateScore (curBlock.worldCoords.x);

	}

	void CheckDirection ()
	{
		if (Input.GetKey (KeyCode.A)) {
			dirApply = true;
			curDir = Direction.Left;
		} else if (Input.GetKey (KeyCode.W)) {
				dirApply = true;
				curDir = Direction.Up;
			} else if (Input.GetKey (KeyCode.S)) {
					dirApply = true;
					curDir = Direction.Down;
				} else {
					if (!dirApply && curDir != Direction.Right) {
						curDir = Direction.Right;
					}
				}
	}

	void FixedUpdate ()
	{
		MoveAnimate ();
	}

	float moveProgress;

	float jumpHeight = .5f;
	public float startHeight = .5f;

	Vector3 targetRotation;
	Vector3 targetScale;
	Vector3 targetPos;

	bool moving;

	void MoveAnimate ()
	{
		if (isSnaped && curBalk != null) {
			if (moving) {
				targetPos = curBalk.transform.position + Vector3.up * startHeight + Vector3.forward * snapedPos;
			} else {
				transform.position = curBalk.transform.position + Vector3.up * startHeight + Vector3.forward * snapedPos;
				curCoord = GetCurCoords ();
			}

		}
/*		if (moveProgress < 1) {
			
			Vector3 pos = Vector3.MoveTowards (transform.localPosition, targetPos, Time.fixedDeltaTime * 10);
			print (pos);
			transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (targetRotation), Time.fixedDeltaTime * 20);
			transform.localScale = Vector3.Lerp (transform.localScale, targetScale, Time.fixedDeltaTime * 5);

			float y = jumpHeight * Mathf.Sin (Mathf.PI * moveProgress) + startHeight;

			moveProgress += Time.fixedDeltaTime / .2f;

			pos.y = y;

			transform.localPosition = pos;
		}*/
	}

	/*void OnDrawGizmos ()
	{
		Gizmos.DrawCube ((WorldPositionToBlockCoords (transform.position) + CubeMeshData.offsets [(int)0]).ToVector3 (.7f), Vector3.one);
	}*/

	IEnumerator MoveCoroutine ()
	{
		moving = true;
		while (moveProgress < 1) {
			Vector3 pos = Vector3.Lerp (transform.position, targetPos, moveProgress);
			transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (targetRotation), Time.fixedDeltaTime * 20);
			transform.localScale = Vector3.Lerp (transform.localScale, targetScale, Time.fixedDeltaTime * 5);

			float y = 0;

			/*if (moveProgress < .5f) {*/
			y = startHeight + jumpHeight * Mathf.Sin (Mathf.PI * moveProgress);
			/*} else {
				y = startHeight + (Mathf.Sin (Mathf.PI * moveProgress) * (targetPos.y));
			}*/

			moveProgress += Time.fixedDeltaTime / (1f / speed);

			pos.y = y;

			transform.position = pos;

			yield return new WaitForFixedUpdate ();
		}
		transform.position = targetPos;

		moving = false;

		OnEndMove ();

	}

	void StartMove (Direction dir, Vector3 targetPos)
	{
		moveProgress = 0;
		targetRotation = Quaternion.LookRotation (new Vector3 (CubeMeshData.offsets [(int)dir].x, 0, CubeMeshData.offsets [(int)dir].y)).eulerAngles;
		transform.localScale = new Vector3 (1.1f, .8f, 1.1f);
		targetScale = Vector3.one;
		this.targetPos = targetPos; //= new Vector3 (curCoord.x, startHeight + curBlock.GetBlockHeight (), curCoord.y);
		StartCoroutine (MoveCoroutine ());

		//if (curDir != Direction.Right && dirApply)
		dirApply = false;
	}

	int nextMoveDir;
	bool isWaitingBalk;

	void Move (Direction dir)
	{
		if (Time.time > lastMoveTime + (1f / speed) + .01f) {


			CheckBlocksAndCollidersAround (dir);

			if (blocksAround [(int)dir].isWalkable ()) {
				PlayStepSound ();

				lastMoveTime = Time.time;

				curCoord += CubeMeshData.offsets [(int)dir];

				CheckBlocksAndCollidersAround (dir);
				targetPos = new Vector3 (curCoord.x, startHeight + curBlock.GetBlockHeight (), curCoord.y);
				CheckBalk (dir);
				StartMove (dir, targetPos);
			}
		}
	}

	void OnEndMove ()
	{
		
		speed = Mathf.Clamp ((float)Math.Sqrt ((double)(UIScreen.Ins.score / 100f * speedChangeMultiply)) + 4, 4, 12);
		OnPlayerStepOnBlock ();
		//print (speed);
	}

	bool isSnaped;
	Balk curBalk;
	int snapedPos = 0;

	void OnPlayerStepOnBlock ()
	{
		
		if (!isSnaped) {

			curBlock.OnPlayerContact ();

			if (curBlock.CanDie ()) {
				Type blockType = curBlock.GetType ();
				DieInfo dieInfo;

				if (blockType == typeof(BlockWater) || blockType == typeof(BlockBalk)) {
					dieInfo = DieInfo.Water;
				} else {
					dieInfo = DieInfo.Trap;
				}

				Die (dieInfo);
			}
		}

		if (isSnaped)
			AudioManager.PlaySoundFromLibrary ("WoodJump");
		else if (curBlock.biome == Biome.Snowy)
				AudioManager.PlaySoundFromLibrary ("SnowJump");
	}

	void PlayStepSound ()
	{
		AudioManager.PlaySoundFromLibrary ("Jump");

	}

	void CheckBalk (Direction dir)
	{
		if (curCollider.Length > 0) {
			if (!isSnaped) {
				curBalk = curCollider [0].transform.GetComponent <Balk> ();
				Snap (curBalk, dir);
			} else if (curBalk.transform != curCollider [0].transform) {
					curBalk = curCollider [0].transform.GetComponent <Balk> ();
					Snap (curBalk, dir);
				} else if (!MoveBalk (dir)) {
						Unsnap (dir);
					}
		} else if (isSnaped)
				Unsnap (dir);
	}

	void Snap (Balk balk, Direction dir)
	{
		isSnaped = true;

		snapedPos = GetCloseBalkPoint (balk);

		targetPos = balk.transform.position + new Vector3 (0, startHeight, GetCloseBalkPoint (balk));
	}

	int GetCloseBalkPoint (Balk balk)
	{
		int maxSize = balk.curBalkLine.size;

		Vector3 localPos = balk.transform.InverseTransformPoint (transform.position);
		int point = Mathf.RoundToInt (localPos.z);

		point = Mathf.Clamp (point, -maxSize, maxSize);

		return point;
		
	}

	void Unsnap (Direction dir)
	{
		isSnaped = false;

		Vector2I vec = WorldPositionToBlockCoords (transform.position);

		targetPos = new Vector3 (vec.x, .5f, vec.y);

		curCoord = World.Ins.GetBlock (WorldPositionToBlockCoords (transform.position)).worldCoords + CubeMeshData.offsets [(int)dir];
		targetPos = new Vector3 (curCoord.x, startHeight + curBlock.GetBlockHeight (), curCoord.y);

	}

	bool MoveBalk (Direction dir)
	{
		if (Direction.Up == dir)
			snapedPos++;

		if (Direction.Down == dir)
			snapedPos--;

		if (snapedPos > curBalk.curBalkLine.size || snapedPos < -curBalk.curBalkLine.size)
			return false;

		curCoord = World.Ins.GetBlock (WorldPositionToBlockCoords (transform.position)).worldCoords + CubeMeshData.offsets [(int)dir];

		targetPos = curBalk.transform.position + Vector3.up * (startHeight + 0.4f) + Vector3.forward * snapedPos;
		return true;
	}

	public Vector2I WorldPositionToBlockCoords (Vector3 pos)
	{
		int x = Mathf.RoundToInt (pos.x);
		int y = Mathf.RoundToInt (pos.z);
		return new Vector2I (x, y);
	}

	Material whiteMat;

	public void Die (DieInfo dieInfo)
	{
		if (isDead)
			return;
		
		if (dieInfo.soundName == "") {
			AudioManager.PlaySoundFromLibrary ("Dead");
		} else {
			AudioManager.PlaySoundFromLibrary (dieInfo.soundName);
		}
		
		isDead = true;

		GetComponent <MeshRenderer> ().material = whiteMat;

		Utility.LoadFromResources (dieInfo.pathToParticle, new Vector3 (curBlock.worldCoords.x, 1, curBlock.worldCoords.y), null, 2);

		/*if (dieInfo.hidePlayer)
			gameObject.SetActive (false);*/

		ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.GameOver);

		StartCoroutine (FadeMat ());

		if (OnPlayerDie != null)
			OnPlayerDie ();
	}

	GameObject warningPlayer;

	void ShowPlayerWarning (bool activate)
	{
		if (activate) {
			if (warningPlayer == null) {
				GameObject gp = (GameObject)Resources.Load ("Prefabs/Warning");

				warningPlayer = Instantiate (gp);
			}

			warningPlayer.SetActive (true);

		} else {
			if (warningPlayer != null) {
				
				warningPlayer.SetActive (false);
			}

		}
	}

	IEnumerator FadeMat ()
	{
		
		Color col = whiteMat.color;

		float t = 1;

		while (t > 0) {
			
			col.a = t;

			whiteMat.color = col;

			yield return null;

			t -= Time.deltaTime / 2f;
		}


	}

	Collider[] curCollider;
	Block[] blocksAround = new Block[4];
	Block curBlock;

	void CheckBlocksAndCollidersAround (Direction dir)
	{
		for (int i = 0; i < blocksAround.Length; i++) {
			blocksAround [i] = World.Ins.GetBlock (curCoord + CubeMeshData.offsets [i]);
		}

		curBlock = World.Ins.GetBlock (curCoord);

		curCollider = Physics.OverlapBox ((/*WorldPositionToBlockCoords (*/transform.position + CubeMeshData.offsets [(int)dir].ToVector3 ())/*.ToVector3 (.7f)*/, Vector3.one * .4f, Quaternion.identity, MovingObjectsManager.Ins.balkMask);
	}

	public Vector2I GetCurCoords ()
	{
		return WorldPositionToBlockCoords (transform.position);
	}

	void OnDestroy ()
	{
		Game.OnGameStarted -= OnGameStart;
	}
}