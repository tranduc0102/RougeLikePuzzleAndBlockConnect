using UnityEngine;

public class PlayerAttack : ActorAttack
{
    protected override void SendDamage(Stats stats)
    {
        Debug.Log("Player Send Damage" + gameObject.name + " => " + stats.ToString());
    }
}