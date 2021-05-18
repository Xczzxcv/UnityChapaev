using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitVariables : MonoBehaviour
{
    [SerializeField] private BoolVar isMoveDone;
    [SerializeField] private BoolVar isPlayerTurn;
    [SerializeField] private BoolVar isAIThinking;
    [SerializeField] private IntVar activeDraughtID;

    private void Awake()
    {
        isMoveDone.SetValue(false);
        isPlayerTurn.SetValue(true);
        isAIThinking.SetValue(false);
        activeDraughtID.SetValue(0);

        Vector3 startPoint = new Vector3(-1.5f, 1, -3.7f);
        Vector3 endPoint = new Vector3(-1.5f, 1, 2.8f);
        var forwardVector = endPoint - startPoint;
        Quaternion rotation = Quaternion.AngleAxis(45, Vector3.up);
        var rotatedVector = rotation * forwardVector;
        //Debug.DrawLine(startPoint, startPoint + rotatedVector, Color.red, 1000);
    }

}
