using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateManagerScript : MonoBehaviour
{
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
	private bool isProcessingTurn = false;
	private bool isGameEnded = false;
	private bool isEndGameProcessed = false;
	public GameObject Winner { get; set; } = null;

	private void Update()
	{
		if (isGameEnded && !isEndGameProcessed)
		{
			ProcessEndGame();
			isEndGameProcessed = true;
		}
		else if (!isGameEnded)
		{
			(isGameEnded, Winner) = GetResults();
			CheckForNewTurn();
		}
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
		if (isMoveDone && !isProcessingTurn && !IsActivePhase())
		{
			isProcessingTurn = true;
			StartCoroutine(ProcessNewTurnEvent());
		}
	}
	
	private IEnumerator ProcessNewTurnEvent()
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
		isProcessingTurn = false;
	}

	private Tuple<bool, GameObject> GetResults()
	{
		int playerAlive = 0,
			playerDead = 0,
			opponentAlive = 0,
			opponentrDead = 0;
		foreach (Transform draughtT in whiteParent.transform)
		{
			if (draughtT.gameObject.GetComponent<DraughtController>().isActive) ++playerAlive;
			else ++playerDead;
		}

		foreach (Transform draughtT in blackParent.transform)
		{
			if (draughtT.gameObject.GetComponent<DraughtController>().isActive) ++opponentAlive;
			else ++opponentrDead;
		}
		
		if (playerAlive + opponentAlive == 0)
		{
			if (playerDead > opponentrDead) return Tuple.Create(true, whiteParent);
			else if (opponentrDead > playerDead) return Tuple.Create(true, blackParent);
			else return Tuple.Create<bool, GameObject>(true, null);
		}
		else if (playerAlive == 0) return Tuple.Create(true, blackParent);
		else if (opponentAlive == 0) return Tuple.Create(true, whiteParent);
		else return Tuple.Create<bool, GameObject>(false, null);
	}

	private void ProcessEndGame()
	{
		Debug.Log("GAME ENDED!!!!!");
		onGameEnded.Raise();
	}
}
