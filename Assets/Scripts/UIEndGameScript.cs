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
	[SerializeField] private GameEvent restartButtonClickedEvent;
	
	public void UpdateWinnerText()
	{
		resultsTMPro.text = endText;
	}

	public void RestartButtonClick()
	{
		DestroyChildren(playerDraughtsParent);
		DestroyChildren(opponentDraughtsParent);

		drSpawnerScript.SpawnDraughts();
		restartButtonClickedEvent.Raise();
	}

	private void DestroyChildren(GameObject parent)
	{
		foreach (Transform childT in parent.transform)
		{
			Destroy(childT.gameObject);
		}
	}
}
