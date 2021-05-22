using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class MenuInit : ScriptableObject
{
    private bool isFirstMenuInit;
	
	private void OnDisable()
	{
		isFirstMenuInit = true;
	}

	public void Init(IntVar playMode, IntVar playerDraughtsColor)
    {
        Debug.Log($"first time huh? {isFirstMenuInit}");
        if (!isFirstMenuInit) return;

		playMode.SetValue(0);
		playerDraughtsColor.SetValue(0);
        isFirstMenuInit = false;
    }
}
