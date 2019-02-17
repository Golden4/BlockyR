using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof(Rigidbody))]
public class Player : MonoBehaviour {
	public static Player Ins;

	public static event System.Action OnPlayerDie;
	public static event System.Action OnPlayerRetry;

	protected Vector2I curCoord = new Vector2I ();
	protected Rigidbody rb;
	protected float speed = 4;
	protected float speedChangeMultiply = 3;

	[HideInInspector]
	public bool isDead = false;

	public static bool isWaitingOnStart = true;
	protected float waitingTime = .8f;

	protected Material origMaterial;

	public PlayerAbility ability;

	protected virtual void Awake ()
	{
		Ins = this;
		Game.OnGameStarted += OnGameStart;
	}

	protected virtual void Start ()
	{
		rb = GetComponent <Rigidbody> ();
		rb.useGravity = false;
		rb.isKinematic = true;
		targetScale = transform.localScale;
		whiteMat = Resources.Load <Material> ("Materials/WhiteMaterial");
		gameObject.AddComponent <AudioListener> ();
		origMaterial = GetComponent <MeshRenderer> ().material;
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

	protected float lastMoveTime = -1;

	protected Direction curDir;
	protected bool dirApply;

	protected virtual void Update ()
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
		
		if (ability != null)
			ability.Update ();

		//CheckDirection ();

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

		//if (!isSnaped /*&& !isWaitingBalk*/)
		//	Move (curDir);
		
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

	protected virtual void FixedUpdate ()
	{
		if (isDead || !Game.isGameStarted || Game.isPause || isWaitingOnStart)
			return;
		
		MoveAnimate ();
	}

	protected float moveProgress;

	protected float jumpHeight = .5f;
	public float startHeight = .5f;

	protected Vector3 targetRotation;
	protected Vector3 targetScale;
	protected Vector3 targetPos;

	protected bool moving;

	protected void MoveAnimate ()
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
		Vector3 rotTemp = new Vector3 (CubeMeshData.offsets [(int)dir].x, 0, CubeMeshData.offsets [(int)dir].y);

		if (rotTemp != Vector3.zero)
			targetRotation = Quaternion.LookRotation (rotTemp).eulerAngles;
		
		transform.localScale = new Vector3 (1.1f, .8f, 1.1f);
		targetScale = Vector3.one;
		this.targetPos = targetPos; //= new Vector3 (curCoord.x, startHeight + curBlock.GetBlockHeight (), curCoord.y);
		StartCoroutine (MoveCoroutine ());

		//if (curDir != Direction.Right && dirApply)
		dirApply = false;
	}

	protected int nextMoveDir;
	protected bool isWaitingBalk;

	public virtual void Move (Direction dir)
	{
		if (Time.time > lastMoveTime + (1f / speed) + .01f) {
			CheckBlocksAndCollidersAround (dir);

			Block block;

			if (dir != Direction.Top) {
				block = blocksAround [(int)dir];
			} else {
				block = curBlock;
			}

			if (block.isWalkable ()) {
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

	void SetCoords (Vector2I coords)
	{
		curCoord = coords;
		curBlock = World.Ins.GetBlock (curCoord);
		moveProgress = 10;

		targetPos = new Vector3 (curCoord.x, startHeight + curBlock.GetBlockHeight (), curCoord.y);
		transform.position = targetPos;
		isSnaped = false;
	}

	protected bool isSnaped;
	protected Balk curBalk;
	protected int snapedPos = 0;

	protected virtual void OnPlayerStepOnBlock ()
	{
		
		if (!isSnaped) {

			curBlock.OnPlayerContact ();

			if (curBlock.CanDie ()) {
				Type blockType = curBlock.GetType ();
				DieInfo dieInfo;

				if (blockType.IsSubclassOf (typeof(BlockWater)) || blockType == typeof(BlockWater)) {
					dieInfo = DieInfo.Water;
				} else {
					dieInfo = DieInfo.Trap;
				}

				Die (dieInfo);
			}
		} else if (curBlock.biome == Biome.Snowy) {
				AudioManager.PlaySoundFromLibrary ("SnowJump");
			} else {
				AudioManager.PlaySoundFromLibrary ("WoodJump");
			}
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
				TrySnap (curBalk, dir);
			} else if (curBalk.transform != curCollider [0].transform) {
					curBalk.OnPlayerUnsnap ();
					curBalk = curCollider [0].transform.GetComponent <Balk> ();
					TrySnap (curBalk, dir);
				} else if (!MoveBalk (dir)) {
						Unsnap (dir);
					}

		} else if (isSnaped)
				Unsnap (dir);
	}

	void TrySnap (Balk balk, Direction dir)
	{
		if (balk.canSnap) {
			isSnaped = true;

			snapedPos = GetCloseBalkPoint (balk);

			targetPos = balk.transform.position + new Vector3 (0, startHeight, GetCloseBalkPoint (balk));
			balk.OnPlayerSnap ();
		} else {
			Unsnap (dir);
		}
	}

	int GetCloseBalkPoint (Balk balk)
	{
		int maxSize = balk.size;

		Vector3 localPos = balk.transform.InverseTransformPoint (transform.position);
		int point = Mathf.RoundToInt (localPos.z);

		point = Mathf.Clamp (point, -maxSize, maxSize);

		return point;
	}

	public void Unsnap (Direction dir)
	{
		isSnaped = false;

		Vector2I vec = WorldPositionToBlockCoords (transform.position);

		targetPos = new Vector3 (vec.x, .5f, vec.y);
		if (dir == Direction.Top)
			curCoord = World.Ins.GetBlock (WorldPositionToBlockCoords (transform.position)).worldCoords;
		else
			curCoord = World.Ins.GetBlock (WorldPositionToBlockCoords (transform.position)).worldCoords + CubeMeshData.offsets [(int)dir];

		targetPos = new Vector3 (curCoord.x, startHeight + curBlock.GetBlockHeight (), curCoord.y);

		if (curBalk != null)
			curBalk.OnPlayerUnsnap ();
	}

	bool MoveBalk (Direction dir)
	{
		if (Direction.Up == dir)
			snapedPos++;

		if (Direction.Down == dir)
			snapedPos--;

		if (snapedPos > curBalk.size || snapedPos < -curBalk.size)
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

	protected bool retry = false;

	public virtual void Die (DieInfo dieInfo)
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
		
		if (!retry)
			ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.GameOver);

		StartCoroutine (FadeMat ());

		if (ability != null && ability.isUsingAbility)
			ability.OnAbilityEnd ();

		if (OnPlayerDie != null)
			OnPlayerDie ();

		if (retry)
			Utility.Invoke (this, 1, Retry);

	}

	protected GameObject warningPlayer;

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

	protected Collider[] curCollider;
	protected Block[] blocksAround = new Block[4];
	protected Block curBlock;

	protected void CheckBlocksAndCollidersAround (Direction dir)
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

	public void Retry ()
	{
		GetComponent <MeshRenderer> ().material = origMaterial;
		isDead = false;
		dirApply = false;
		curDir = Direction.Right;

		SetCoords (World.FindSpawnPos (curCoord));

		if (OnPlayerRetry != null)
			OnPlayerRetry ();
	}
}