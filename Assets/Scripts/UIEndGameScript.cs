using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEndGameScript : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI resultsTMPro;
	[SerializeField] private StringRef endText;
	[Space]
	[SerializeField] private GameObject playerDraughtsParent;
	[SerializeField] private GameObject opponentDraughtsParent;
	[Space]
	[SerializeField] private DraughtsSpawnerScript drSpawnerScript;
	[SerializeField] private UIScoreScript scoreObj;
	[SerializeField] private InitVariables initvarObj;
	[SerializeField] private GameStateManagerScript gametateObj;
	[SerializeField] private GameEvent firstTurnEvent;
	
	public void UpdateWinnerText()
	{
		resultsTMPro.text = endText;
	}

	public void RestartButtonClick()
	{
		DestroyChildren(playerDraughtsParent);
		DestroyChildren(opponentDraughtsParent);

		StartCoroutine(RestartValues());
	}

	private void DestroyChildren(GameObject parent)
	{
		foreach (Transform childT in parent.transform)
		{
			Destroy(childT.gameObject);
		}
	}

	private IEnumerator RestartValues()
	{
		initvarObj.Initialize();

		gametateObj.InitManager();
		drSpawnerScript.SpawnDraughts();

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		scoreObj.UpdateDraughtsTexts();

		GetComponent<Animator>().SetTrigger("RestartGameTrigger");
	}
}
