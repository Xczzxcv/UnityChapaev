using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "String Variable")]
public class StringVar : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public string Value;

    public void SetValue(string value)
    {
        Value = value;
    }

    public void SetValue(StringVar value)
    {
        Value = value.Value;
    }
}