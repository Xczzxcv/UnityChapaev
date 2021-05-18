using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEndGameScript : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI resultsTMPro;
	[SerializeField] private StringRef endText;
	
	public void UpdateWinnerText()
	{
		resultsTMPro.text = endText;
	}
}
