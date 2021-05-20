using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitMenuVariables : MonoBehaviour
{
    [SerializeField] private MenuInit initializer;
 
    private void Awake()
    {
        initializer.Init();
    }


}
