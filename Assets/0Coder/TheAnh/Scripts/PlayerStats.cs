
using System;
using System.Collections;
using System.Data;
using DesignPattern.Obsever;
using UnityEngine;

public class PlayerStats : ActorStats
{
    protected void OnEnable()
    {
       ObserverManager<EventID>.RegisterEvent(EventID.UpdateStatsPlayer, param => AddStats((Stats) param));
    }

    protected override IEnumerator HandleDead()
    {
        //DoAnim
        yield return new WaitForSeconds(timeDespawn);
        ObserverManager<EventID>.PostEvent(EventID.Lose);
    }
}
