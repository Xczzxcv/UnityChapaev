using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManagerScript : MonoBehaviour
{
	public enum Side { player, opponent };
	[SerializeField] private GameObject whiteParent;
	[SerializeField] private GameObject blackParent;
	[SerializeField] private GameEvent newTurnEvent;
	[SerializeField] private BoolRef isPlayersTurn;
	[SerializeField] private GameEvent onGameEnded;
	[SerializeField] private BoolRef isGameEnded;
	[SerializeField] private float timeBeforeNextTurn = 2f;
	[Header("Player Camera")]
	[SerializeField] private Vector3 playerCameraPos;
	[SerializeField] private Quaternion playerCameraRotation;
	[Header("Opponent Camera")]
	[SerializeField] private Vector3 opponentCameraPos;
	[SerializeField] private Quaternion opponentCameraRotation;

	private float minVelocity = 0.01f;
	private bool isMoveDone = false;
	private bool isProcessing = false;

	private void Update()
	{
		CheckForNewTurn();
	}

	public void MoveIsDone()
	{
		isMoveDone = true;
	}

	private bool IsActivePhase()
	{
		foreach (Transform draughtT in whiteParent.transform)
		{
			if (draughtT.gameObject.GetComponent<Rigidbody>().velocity.x > minVelocity
				|| draughtT.gameObject.GetComponent<Rigidbody>().velocity.y > minVelocity
				|| draughtT.gameObject.GetComponent<Rigidbody>().velocity.z > minVelocity)
			{ return true; }
		}

		foreach (Transform draughtT in blackParent.transform)
		{
			if (draughtT.gameObject.GetComponent<Rigidbody>().velocity.x > minVelocity
				|| draughtT.gameObject.GetComponent<Rigidbody>().velocity.y > minVelocity
				|| draughtT.gameObject.GetComponent<Rigidbody>().velocity.z > minVelocity)
			{ return true; }
		}

		return false;
	}

	private void CheckForNewTurn()
	{
		if (isMoveDone && !isProcessing && !IsActivePhase())
		{
			isProcessing = true;
			StartCoroutine(ProcessNewEvent());
		}
	}
	
	private IEnumerator ProcessNewEvent()
	{
		yield return new WaitForSeconds(timeBeforeNextTurn);

		isMoveDone = false;
		Debug.Log($"bef invert {isPlayersTurn}");
		isPlayersTurn.Variable.Invert();
		Debug.Log($"aft invert {isPlayersTurn}");
		if (isPlayersTurn.Value)
		{
			Debug.Log("PLAYER TURN");
			Camera.main.transform.position = playerCameraPos;
			Camera.main.transform.rotation = playerCameraRotation;
		}
		else
		{
			Debug.Log("OPPONENT TURN");
			Camera.main.transform.position = opponentCameraPos;
			Camera.main.transform.rotation = opponentCameraRotation;
		}

		newTurnEvent.Raise();
		isProcessing = false;
	}
}
