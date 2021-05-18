using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BoolRef
{
    public bool UseConstant;
    public bool ConstantValue;
    public BoolVar Variable;

    public BoolRef()
    { }

    public BoolRef(bool value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public bool Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator bool(BoolRef reference)
    {
        return reference.Value;
    }
}
