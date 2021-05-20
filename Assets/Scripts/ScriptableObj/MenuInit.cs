using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class MenuInit : ScriptableObject
{
    [SerializeField] private IntVar playMode;
    [SerializeField] private IntVar playerDraughtsColor;

    private bool isFirstMenuInit;

	private void OnEnable()
	{
        Debug.Log("enabled");
        isFirstMenuInit = true;
    }

	public void Init()
    {
        Debug.Log($"first time huh? {isFirstMenuInit}");
        if (!isFirstMenuInit) return;

        playMode.SetValue(0);
        playerDraughtsColor.SetValue(0);
        isFirstMenuInit = false;
    }



}
