using UnityEngine;

public class EnemyAttack : ActorAttack
{
    protected override void SendDamage(Stats stats)
    {
        Debug.Log("Enemy Send Damage" + gameObject.name + " => " + stats.ToString());
    }
}