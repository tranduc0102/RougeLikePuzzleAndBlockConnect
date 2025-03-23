using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : ActorStats
{
<<<<<<< Updated upstream
    protected override void HandleDead()
    {
        Debug.Log("Post event lose");
=======
    protected void OnEnable()
    {
        ObserverManager<EventID>.RegisterEvent(EventID.UpdateStatsPlayer, param => AddStats((Stats) param));
    }

    protected void OnDisable()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.UpdateStatsPlayer, param => AddStats((Stats) param));
    }
    
    protected override IEnumerator HandleDead()
    {
        //DoAnim
        yield return new WaitForSeconds(timeDespawn);
        ObserverManager<EventID>.PostEvent(EventID.Lose);
>>>>>>> Stashed changes
    }

   
}
