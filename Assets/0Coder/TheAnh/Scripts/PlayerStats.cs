using System;
using System.Collections;
using System.Data;
using DesignPattern.Obsever;
using UnityEngine;

public class PlayerStats : ActorStats
{
    private int cntABlockErase;
    protected void OnEnable()
    {
        cntABlockErase = -1;
        ObserverManager<EventID>.RegisterEvent(EventID.UpdateStatsPlayer, param => AddStats((Stats) param));
        ObserverManager<EventID>.RegisterEvent(EventID.SendCntBlockErase, param => ChangeCntABlockErase((int) param));
    }

    protected void OnDisable()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.UpdateStatsPlayer, param => AddStats((Stats) param));
        ObserverManager<EventID>.RemoveEvent(EventID.SendCntBlockErase, param => ChangeCntABlockErase((int) param));
    }

    private void ChangeCntABlockErase(int newCntABlockErase)
    {
        cntABlockErase = newCntABlockErase;
    }

    protected override void AddStats(Stats stats)
    {
        base.AddStats(stats);
        -- cntABlockErase;
        if (cntABlockErase == 0)
        {
            -- cntABlockErase;
            ObserverManager<EventID>.PostEvent(EventID.PlayerAttackEnemy, stats);
        }
    }

    protected override IEnumerator HandleDead()
    {
        //DoAnim
        yield return new WaitForSeconds(timeDespawn);
        ObserverManager<EventID>.PostEvent(EventID.Lose);
    }
}