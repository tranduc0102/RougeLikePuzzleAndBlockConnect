using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBehavior : MonoBehaviour
{
    protected virtual void LoadComponent()
    {
        
    }

    protected virtual void Awake()
    {
        LoadComponent();
    }

    protected void Reset()
    {
        LoadComponent();
    }
}
