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
        animator = gameObject.GetComponent<Animator>();
        cntABlockErase = -1;
        ObserverManager<EventID>.RegisterEvent(EventID.UpdateStatsPlayer, param => AddStats((Stats) param));
        ObserverManager<EventID>.RegisterEvent(EventID.SendCntBlockErase, param => ChangeCntABlockErase((int) param));
        ObserverManager<GameTurn>.RegisterEvent(GameTurn.PlayerTurn, param => AddStats((Stats) param));
    }

    protected void OnDisable()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.UpdateStatsPlayer, param => AddStats((Stats) param));
        ObserverManager<EventID>.RemoveEvent(EventID.SendCntBlockErase, param => ChangeCntABlockErase((int) param));
        ObserverManager<GameTurn>.RemoveEvent(GameTurn.PlayerTurn, param => AddStats((Stats) param));
    }
    
    private void ChangeCntABlockErase(int newCntABlockErase)
    {
        cntABlockErase = newCntABlockErase;
    }

    protected override void AddStats(Stats stats)
    {
        if (GameManager.Instance.gameTurn == GameTurn.PlayerTurn)
        {
            -- cntABlockErase;
            if (cntABlockErase == 0)
            {
                -- cntABlockErase;
                // TODO: Xu ly stats player gui toi cho enemy
                Stats statsAttackEnemy = new Stats { PhysicalDamage = m_ActorStats.PhysicalDamage * -1, MagicalDamage = m_ActorStats.MagicalDamage * -1 };
                PlayerAttackEnemy(statsAttackEnemy);
            }
            base.AddStats(stats);
        }
        else
        {
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
            print(stats.MagicalDamage + stats.PhysicalDamage);
            print(isAttack.HealthPoint);
            print(isAttack.Armor);
            base.AddStats(isAttack);
            GameManager.Instance.gameTurn = GameTurn.PlayerTurn;
        }
    }
    
    protected void PlayerAttackEnemy(Stats stats)
    {
        animator.SetTrigger("Attack");
        ObserverManager<GameTurn>.PostEvent(GameTurn.EnemyTurn, stats);
    }

    protected override IEnumerator HandleDead()
    {
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(timeDespawn);
        ObserverManager<EventID>.PostEvent(EventID.Lose);
        // use pooling later
        Destroy(gameObject);
    }
}