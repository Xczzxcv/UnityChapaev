using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoreScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerDraughtsText;
    [SerializeField] private TextMeshProUGUI opponentDraughtsText;
    [SerializeField] IntRef playerDraughts;
    [SerializeField] IntRef opponentDraughts;

	private void Start()
	{
        UpdateDraughtsTexts();
    }

	public void UpdateDraughtsTexts()
	{
        playerDraughtsText.text = playerDraughts.Value.ToString();
        opponentDraughtsText.text = opponentDraughts.Value.ToString();
    }
}
