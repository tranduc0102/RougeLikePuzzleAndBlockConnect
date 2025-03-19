


using System.Collections;
using UnityEngine;

public class EnemyStats : ActorStats
{
    protected override IEnumerator HandleDead()
    {
        // do Anim
        yield return new WaitForSeconds(timeDespawn);
        //use pooling later
        Destroy(gameObject);
    }
}
