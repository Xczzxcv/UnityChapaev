using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsMenuRefresh : MonoBehaviour
{
	[SerializeField] private TMP_Dropdown GameModeOption;
	[SerializeField] private IntRef GameModeValue;
	[Space]
	[SerializeField] private TMP_Dropdown DraughtsColorOption;
	[SerializeField] private IntRef DraughtsColorValue;

	private void OnEnable()
	{
		StartCoroutine(RefreshCoroutine());
	}

	private IEnumerator RefreshCoroutine()
	{
		yield return new WaitForEndOfFrame();
		GameModeOption.value = GameModeValue.Value;
		DraughtsColorOption.value = DraughtsColorValue.Value;
	}
}
