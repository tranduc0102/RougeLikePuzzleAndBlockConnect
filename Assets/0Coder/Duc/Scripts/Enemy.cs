using DesignPattern.ObjectPool;
using DesignPattern.Obsever;
using DG.Tweening;
using Duc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Actor
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Image hp;
    public Image ImgChoice;
    public int _idEnemy;
    private float totalHP;
    private void OnEnable()
    {
        SetupData();
        ImgChoice.gameObject.SetActive(false);
        totalHP = m_ActorStats.HealthPoint + m_ActorStats.Armor;
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
        /* _enemyDie = _enemyData.objEnemyDie;
         timeDelayAttackPlayer = _enemyData.timeDelayAttackPlayer;;*/
        distanceActorRun = 10f;
        Alive = true;
    }
    protected override void Attack()
    {
        base.Attack();
    }

    protected override void HandleDead()
    {
        animator.SetBool("Die", true);
        DOVirtual.DelayedCall(1.5f, delegate
        {
            GameManager.Instance.AllEnemyOutWay.Remove(this);
            Destroy(this.gameObject);
        });
    }

    protected override void ProcessTurn(Action actionFinish)
    {
        base.ProcessTurn(actionFinish);
    }
    public override void TakeTurn(bool isMyTurn, Action actionFinish)
    {
        if (isMyTurn)
        {
            ProcessTurn(actionFinish);
        }
    }
    public override void TargetReceiverDamage()
    {
        base.TargetReceiverDamage();
    }
    public override void ReceiveDamaged()
    {
        base.ReceiveDamaged();
        hp.fillAmount = (m_ActorStats.HealthPoint + m_ActorStats.Armor) / totalHP;
        if (ImgChoice)
        {
            ImgChoice.gameObject.SetActive(false);
        }
    }

}
