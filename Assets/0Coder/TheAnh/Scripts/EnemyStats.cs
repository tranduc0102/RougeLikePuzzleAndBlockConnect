
using DesignPattern.ObjectPool;
using UnityEngine;


public class EnemyStats : ActorStats
{
    protected override void HandleDead()
    {
        Destroy(gameObject);
        // use pooling late
    }
   
}
