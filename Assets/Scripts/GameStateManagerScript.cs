using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameResult = System.Tuple<bool, string>;

public class GameStateManagerScript : MonoBehaviour
{
	[SerializeField] private GameObject whiteParent;
	[SerializeField] private GameObject blackParent;
	[Space]
	[SerializeField] private float timeBeforeNextTurn = 2f;
	[SerializeField] private GameEvent onGameEnded;
	[Space]
	[SerializeField] private BoolRef isPlayersTurn;
	[SerializeField] private GameEvent newTurnEvent;
	[SerializeField] private IntRef playMode;
	[SerializeField] private BoolVar isAIThinking;
	[Space]
	[Header("Player Camera")]
	[SerializeField] private Vector3 playerCameraPos;
	[SerializeField] private Quaternion playerCameraRotation;
	[Header("Opponent Camera")]
	[SerializeField] private Vector3 opponentCameraPos;
	[SerializeField] private Quaternion opponentCameraRotation;
	[Space]
	[SerializeField] private BoolRef isMoveDone;
	[SerializeField] private StringRef ResultsText;
	[Space]
	[SerializeField] private string playerWinText;
	[SerializeField] private string opponentWinText;
	[SerializeField] private string drawText;
	[Space]
	[SerializeField] private FloatRef minVelocity;

	private bool isProcessingTurn = false;
	private bool isGameEnded = false;
	private bool isEndGameProcessed = false;

	private GameResult playerWinResult;
	private GameResult opponentWinResult;
	private GameResult drawResult;
	private GameResult gameContinueResult;

	private void Start()
	{
		playerWinResult = Tuple.Create(true, playerWinText);
		drawResult = Tuple.Create<bool, string>(false, null);
		opponentWinResult = Tuple.Create(true, opponentWinText);
		gameContinueResult = Tuple.Create<bool, string>(false, null);
	}

	private void Update()
	{
		if (IsNewTurn())
		{
			if (!isGameEnded) (isGameEnded, ResultsText.Variable.Value) = GetResults();
 
			if (isGameEnded && !isEndGameProcessed)
			{
				ProcessEndGame();
				isEndGameProcessed = true;
			}
			else if (!isGameEnded)
			{
				isProcessingTurn = true;
				StartCoroutine(ProcessNewTurnEvent());
			}
		}
	}

	private bool IsActivePhase()
	{
		Vector3 tempVector;

		foreach (Transform draughtT in whiteParent.transform)
		{
			tempVector = draughtT.GetComponent<Rigidbody>().velocity;
			if (Mathf.Abs(tempVector.x) > minVelocity
				|| Mathf.Abs(tempVector.y) > minVelocity
				|| Mathf.Abs(tempVector.z) > minVelocity)
			{ return true; }
		}

		foreach (Transform draughtT in blackParent.transform)
		{
			tempVector = draughtT.GetComponent<Rigidbody>().velocity;
			if (Mathf.Abs(tempVector.x) > minVelocity
				|| Mathf.Abs(tempVector.y) > minVelocity
				|| Mathf.Abs(tempVector.z) > minVelocity)
			{ return true; }
		}

		return false;
	}

	private bool IsNewTurn()
	{
		return isMoveDone && !isProcessingTurn && !IsActivePhase();
	}
	
	private IEnumerator ProcessNewTurnEvent()
	{
		yield return new WaitForSeconds(timeBeforeNextTurn);

		isMoveDone.Variable.SetValue(false);
		isPlayersTurn.Variable.Invert();

		if (playMode.Value == 1 && !isPlayersTurn.Value ) isAIThinking.SetValue(true);
		else isAIThinking.SetValue(false);

		newTurnEvent.Raise();

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

		isProcessingTurn = false;
	}

	private GameResult GetResults()
	{
		// call happens before processing new turn, so 'isPlayerTurn' not changed yet

		Debug.Log($"get results call {isPlayersTurn.Value}");

		int playerAlive = 0,
			playerDeactivated = 0,
			opponentAlive = 0,
			opponentrDeactivated = 0;
		foreach (Transform draughtT in whiteParent.transform)
		{
			if (draughtT.gameObject.GetComponent<DraughtController>().isActive) ++playerAlive;
			else ++playerDeactivated;
		}

		foreach (Transform draughtT in blackParent.transform)
		{
			if (draughtT.gameObject.GetComponent<DraughtController>().isActive) ++opponentAlive;
			else ++opponentrDeactivated;
		}

		Debug.Log($"Player: A {playerAlive} | D {playerDeactivated}");
		Debug.Log($"Opponent: A {opponentAlive} | D {opponentrDeactivated}");
		
		if (playerAlive > 0 && opponentAlive > 0) return gameContinueResult;
		else if (playerAlive == 0 && opponentAlive > 0)
		{
			if (!isPlayersTurn) return opponentWinResult;
			else {
				if (opponentAlive == 1) return gameContinueResult;
				else return opponentWinResult;
			} 
		}
		else if (opponentAlive == 0 && playerAlive > 0)
		{
			if (isPlayersTurn) return playerWinResult;
			else
			{
				if (playerAlive == 1) return gameContinueResult;
				else return playerWinResult;
			}
		}
		else
		{
			if (playerDeactivated > opponentrDeactivated) return playerWinResult;
			else if (opponentrDeactivated > playerDeactivated) return opponentWinResult;
			else return drawResult;
		}
	}

	private void ProcessEndGame()
	{
		Debug.Log("GAME ENDED!!!!!");
		onGameEnded.Raise();
	}
}
