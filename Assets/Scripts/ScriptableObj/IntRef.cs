using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IntRef
{
    public bool UseConstant;
    public int ConstantValue;
    public IntVar Variable;

    public IntRef()
    { }

    public IntRef(int value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public int Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator int(IntRef reference)
    {
        return reference.Value;
    }
}
