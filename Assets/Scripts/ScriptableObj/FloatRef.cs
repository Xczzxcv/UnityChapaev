using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FloatRef
{
    public bool UseConstant;
    public float ConstantValue;
    public FloatVar Variable;

    public FloatRef()
    { }

    public FloatRef(float value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator float(FloatRef reference)
    {
        return reference.Value;
    }
}
