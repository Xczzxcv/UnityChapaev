using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitVariables : MonoBehaviour
{
    [SerializeField] private BoolVar isMoveDone;
    [SerializeField] private BoolVar isPlayerTurn;
    [SerializeField] private BoolVar isAIThinking;
    [SerializeField] private BoolVar IsCameraToOpponentAnimOver;
    [SerializeField] private IntVar activeDraughtID;
    [SerializeField] private IntVar PlayMode;
    [SerializeField] private IntVar playerDraughtsColorChoice;

    private void Awake()
    {
        Initialize();
    }

	public void Initialize()
	{
        isMoveDone.SetValue(false);
        activeDraughtID.SetValue(0);

        if (playerDraughtsColorChoice.Value == 0) isPlayerTurn.SetValue(true);
        else isPlayerTurn.SetValue(false);

        if (PlayMode.Value == 1 && playerDraughtsColorChoice.Value == 1)
        {
            IsCameraToOpponentAnimOver.SetValue(true);
        }
        else IsCameraToOpponentAnimOver.SetValue(false);
    }

}
