
using System.Collections;
using DesignPattern.Obsever;
using UnityEngine;

public class PlayerStats : ActorStats
{
    protected override IEnumerator HandleDead()
    {
        //DoAnim
        yield return new WaitForSeconds(timeDespawn);
        ObserverManager<EventId>.PostEvent(EventId.Lose);
    }
}
