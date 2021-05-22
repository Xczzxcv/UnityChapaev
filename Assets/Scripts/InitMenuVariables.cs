using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitMenuVariables : MonoBehaviour
{
    [SerializeField] MenuInit initializer;
    [SerializeField] private IntVar playMode;
    [SerializeField] private IntVar playerDraughtsColor;
//    MenuInit initializer;

    private void Awake()
    {
        Debug.Log("InitMenuVariables awake bef init call");
        //initializer = ScriptableObject.CreateInstance<MenuInit>();
        initializer.Init(playMode, playerDraughtsColor);
    }
}
