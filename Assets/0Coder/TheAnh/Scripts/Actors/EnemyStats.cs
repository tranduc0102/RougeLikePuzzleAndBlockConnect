using System;
using System.Collections;
using DesignPattern.Obsever;
using UnityEngine;

public class EnemyStats : ActorStats
{
    private void OnEnable()
    {
        animator = gameObject.GetComponent<Animator>();
        ObserverManager<GameTurn>.RegisterEvent(GameTurn.EnemyTurn, param => EnableTurnEnemy((Stats) param));
    }

    private void OnDisable()
    {
        ObserverManager<GameTurn>.RemoveEvent(GameTurn.EnemyTurn, param => EnableTurnEnemy((Stats) param));
    }

    private void EnableTurnEnemy(Stats stats)
    {
        AddStats(stats);
    }

    protected override void AddStats(Stats stats)
    {
        if (GameManager.Instance._GameTurn == GameTurn.PlayerTurn)
        {
            GameManager.Instance._GameTurn = GameTurn.EnemyTurn;
            animator.SetTrigger("GetHit");
            Stats isAttack = new Stats();
            if ((stats.MagicalDamage + stats.PhysicalDamage) * -1 <= m_ActorStats.Armor)
            {
                isAttack.Armor = stats.MagicalDamage + stats.PhysicalDamage;
            }
            else
            {
                isAttack.Armor = m_ActorStats.Armor * -1;
                isAttack.HealthPoint = stats.MagicalDamage + stats.PhysicalDamage + isAttack.Armor;
            }
            base.AddStats(isAttack);
            Stats statsAttackPlayer = new Stats { PhysicalDamage = m_ActorStats.PhysicalDamage * -1, MagicalDamage = m_ActorStats.MagicalDamage * -1 };
            StartCoroutine(DelayAttackPlayer(statsAttackPlayer, 2f));
        }
        else
        {
            base.AddStats(stats);
        }
    }

    private IEnumerator DelayAttackPlayer(Stats stats, float timeDelayAttackPlayer)
    {
        // TODO: Delay attack player 
        yield return new WaitForSeconds(timeDelayAttackPlayer);
        EnemyAttackPlayer(stats);
    }
    
    private void EnemyAttackPlayer(Stats stats)
    {
        animator.SetTrigger("Attack");
        ObserverManager<GameTurn>.PostEvent(GameTurn.PlayerTurn, stats);
    }
    
    protected override IEnumerator HandleDead()
    {
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(timeDespawn);
        //use pooling later
        Destroy(gameObject);
    }
}