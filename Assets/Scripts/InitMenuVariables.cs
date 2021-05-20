using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitMenuVariables : MonoBehaviour
{
    [SerializeField] private IntVar playMode;
    [SerializeField] private IntVar playerDraughtsColor;

    private void Awake()
    {
        playMode.SetValue(0);
        playerDraughtsColor.SetValue(0);
    }

}
