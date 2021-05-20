using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScoreManagerScript : MonoBehaviour
{
	[SerializeField] private GameObject whiteParent;
	[SerializeField] private GameObject blackParent;
	[Space]
	[SerializeField] private IntVar whiteDraughts;
	[SerializeField] private IntVar blackDraughts;
	[Space]
	[SerializeField] private GameEvent scoreChangeEvent;

	private void Awake()
	{
		UpdateDraughtsNum();
	}

	public void DraughtsDeath()
	{
		UpdateDraughtsNum();
	}

	private void UpdateDraughtsNum()
	{
		StartCoroutine(UpdateDraughtsNumCoroutine());
	}

	private IEnumerator UpdateDraughtsNumCoroutine()
	{
		yield return new WaitForEndOfFrame();
		whiteDraughts.Value = whiteParent.transform.childCount;
		blackDraughts.Value = blackParent.transform.childCount;
		scoreChangeEvent.Raise();
	}
}
