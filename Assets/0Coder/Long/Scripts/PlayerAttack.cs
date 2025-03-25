using System;
using System.Net.NetworkInformation;
using DesignPattern.Obsever;
using UnityEngine;

public class PlayerAttack : ActorAttack
{
    private Animator animator;

    private void OnEnable()
    {
        ObserverManager<EventID>.RegisterEvent(EventID.PlayerAttackEnemy, param => SendDamage((Stats) param));
    }

    private void OnDisable()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.PlayerAttackEnemy, param => SendDamage((Stats) param));
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    protected override void SendDamage(Stats stats)
    {
        Debug.Log("Player Send Damage" + gameObject.name + " => " + stats.ToString());
        animator.SetTrigger("Attack");
    }
}