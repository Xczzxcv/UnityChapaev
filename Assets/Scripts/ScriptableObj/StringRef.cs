using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StringRef
{
    public bool UseConstant;
    public string ConstantValue;
    public StringVar Variable;

    public StringRef()
    { }

    public StringRef(string value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public string Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator string(StringRef reference)
    {
        return reference.Value;
    }
}
