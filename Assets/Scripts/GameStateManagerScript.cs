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
	[SerializeField] private float timeBeforeNextTurn = 2f;
	[Header("Player Camera")]
	[SerializeField] private Vector3 playerCameraPos;
	[SerializeField] private Quaternion playerCameraRotation;
	[Header("Opponent Camera")]
	[SerializeField] private Vector3 opponentCameraPos;
	[SerializeField] private Quaternion opponentCameraRotation;
	[SerializeField] private BoolRef isMoveDone;

	private float minVelocity = 0.01f;
	private bool isProcessing = false;
	private GameObject winner = null;

	private void Update()
	{
		winner = GetWinner();
		if (winner != null)
		{
			ProcessEndGame();
		}
		CheckForNewTurn();
	}

	private bool IsActivePhase()
	{
		Rigidbody tempBody;
		foreach (Transform draughtT in whiteParent.transform)
		{
			tempBody = draughtT.gameObject.GetComponent<Rigidbody>();
			if (tempBody.velocity.x > minVelocity
				|| tempBody.velocity.y > minVelocity
				|| tempBody.velocity.z > minVelocity)
			{ return true; }
		}

		foreach (Transform draughtT in blackParent.transform)
		{
			tempBody = draughtT.gameObject.GetComponent<Rigidbody>();
			if (tempBody.velocity.x > minVelocity
				|| tempBody.velocity.y > minVelocity
				|| tempBody.velocity.z > minVelocity)
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

		isMoveDone.Variable.SetValue(false);
		isPlayersTurn.Variable.Invert();
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

	private GameObject GetWinner()
	{
		int playerCnt = 0,
			opponentCnt = 0;
		foreach (Transform draughtT in whiteParent.transform)
		{
			if (draughtT.gameObject.GetComponent<DraughtController>().isActive) ++playerCnt;
		}

		foreach (Transform draughtT in blackParent.transform)
		{
			if (draughtT.gameObject.GetComponent<DraughtController>().isActive) ++opponentCnt;
		}

		if (playerCnt == 0) return blackParent;
		else if (opponentCnt == 0) return whiteParent;
		else return null;
	}

	private void ProcessEndGame()
	{
		onGameEnded.Raise();
		Debug.Log("GAME ENDED!!!!!");
	}
}
