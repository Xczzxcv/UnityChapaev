using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoreScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI whiteDraughtsText;
    [SerializeField] private TextMeshProUGUI blackDraughtsText;
    [SerializeField] IntRef whites;
    [SerializeField] IntRef blacks;


	private void Start()
	{
        UpdateDraughtsTexts();
    }

	public void UpdateDraughtsTexts()
	{
        whiteDraughtsText.text = whites.Value.ToString();
        blackDraughtsText.text = blacks.Value.ToString();
    }
}
