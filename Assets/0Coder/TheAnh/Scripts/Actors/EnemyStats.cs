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
    [SerializeField] private float timeDelayAttackPlayer;
    [SerializeField] private float distancePlayerRun;
    public int _idEnemy;
    private void Awake()
    {
        SetupData();
    }
    private void OnEnable()
    {
        ObserverManager<GameTurn>.RegisterEvent(GameTurn.EnemyTurn, param => EnableTurnEnemy((Stats) param));
        ObserverManager<EventID>.RegisterEvent(EventID.EnemyAttack, param => AllEnemyAttack((Transform) param));
    }
    private void OnDisable()
    {
        ObserverManager<GameTurn>.RemoveEvent(GameTurn.EnemyTurn, param => EnableTurnEnemy((Stats) param));
        ObserverManager<EventID>.RemoveEvent(EventID.EnemyAttack, param => AllEnemyAttack((Transform) param));
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
        timeDelayAttackPlayer = _enemyData.timeDelayAttackPlayer;
        distancePlayerRun = _enemyData.distaceAttackPlayer;
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
            GameManager.Instance.ChangeTurn(GameTurn.EnemyTurn);
        }
        else
        {
            base.AddStats(stats);
        }
    }
    private void AllEnemyAttack(Transform trans)
    {
        if (trans == transform)
        {
            Stats statsAttackPlayer = new Stats { PhysicalDamage = m_ActorStats.PhysicalDamage * -1, MagicalDamage = m_ActorStats.MagicalDamage * -1 };
            StartCoroutine(DelayAttackPlayer(statsAttackPlayer, timeDelayAttackPlayer));
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
        StartCoroutine(EnemyMoveToAttack(stats));
    }
    private IEnumerator EnemyMoveToAttack(Stats stats)
    {
        Vector3 tam = transform.position;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        transform.DOMove(player.transform.position + Vector3.right * distancePlayerRun, 1f);
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Attack");
        ObserverManager<GameTurn>.PostEvent(GameTurn.PlayerTurn, stats);
        yield return new WaitForSeconds(3f);
        transform.DOMove(tam, 1f, false);
    }
    protected override IEnumerator HandleDead()
    {
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(timeDespawn);
        //use pooling later
        GameObject enemyDieObj = Instantiate(_enemyDie, transform.position, Quaternion.identity);
        enemyDieObj.transform.DOScale(Vector3.zero, 2f).From();
        enemyDieObj.transform.SetParent(transform.parent);
        SpawnEnemy.Instance._currentEnemies.Remove(transform);
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