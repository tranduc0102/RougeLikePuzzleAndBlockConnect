using System;
using System.Collections;
using DesignPattern.Obsever;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class EnemyStats : ActorStats
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject _enemyDie;
    public int _idEnemy;
    private void Awake()
    {
        SetupData();
    }
    private void OnEnable()
    {
        ObserverManager<GameTurn>.RegisterEvent(GameTurn.EnemyTurn, param => EnableTurnEnemy((Stats) param));
    }
    private void OnDisable()
    {
        ObserverManager<GameTurn>.RemoveEvent(GameTurn.EnemyTurn, param => EnableTurnEnemy((Stats) param));
    }
    private void SetupData()
    {
        _enemyData = Resources.Load<EnemyData>("ScriptTableObject/Enemy Data");
        animator = gameObject.GetComponent<Animator>();
        m_ActorStats.HealthPoint = _enemyData.Enemies[_idEnemy].HealthPoint;
        m_ActorStats.Armor = _enemyData.Enemies[_idEnemy].Armor;
        m_ActorStats.PhysicalDamage = _enemyData.Enemies[_idEnemy].PhysicalDamage;
        m_ActorStats.MagicalDamage = _enemyData.Enemies[_idEnemy].MagicalDamage;
        timeSpawn = _enemyData.timeSpawn;
        timeDespawn = _enemyData.timeDespawn;
        _enemyDie = _enemyData.objEnemyDie;
    }
    private void EnableTurnEnemy(Stats stats)
    {
        if (transform == GameManager.Instance._enemyTarget)
        {
            AddStats(stats);
        }
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
        Instantiate(_enemyDie, transform.position, Quaternion.identity).transform.DOScale(Vector3.zero, 2f).From();
        gameObject.SetActive(false);
        for (int i = 0; i < transform.parent.childCount; ++i)
        {
            if (transform.parent.GetChild(i).gameObject.activeSelf == true)
            {
                GameManager.Instance._enemyTarget = transform.parent.GetChild(i);
                break;
            }
        }
        if (GameManager.Instance._enemyTarget == null || GameManager.Instance._enemyTarget.gameObject.activeSelf == false)
        {
            // TODO: Hết enemy nên chuyển sang way mới
            GameManager.Instance.ChangeTurn(GameTurn.EmptyTimeTurn);
        }
    }
    private void OnMouseDown()
    {
        GameManager.Instance._enemyTarget = transform;
    }
}