using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : ActorStats
{
    protected override void HandleDead()
    {
        Debug.Log("Post event lose");
    }

   
}
