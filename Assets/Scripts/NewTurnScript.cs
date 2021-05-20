using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewTurnScript : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI NewTurnTextObj;
	[SerializeField] private string[] playerTurnTexts;
	[SerializeField] private string[] opponentTurnTexts;
	[SerializeField] private BoolRef isPlayerTurn;

	public void UpdateNewTurnText()
	{
		string[] currTexts;
		if (isPlayerTurn.Value) currTexts = playerTurnTexts;
		else currTexts = opponentTurnTexts;
		NewTurnTextObj.text = currTexts[Random.Range(0, currTexts.Length - 1)];
	}
}
