using System;
using System.Collections;
using System.Data;
using DesignPattern.Obsever;
using DG.Tweening;
using UnityEngine;

public class PlayerStats : ActorStats
{
    private Action<object> turnManager;
    private Action<object> addStats;
    private Action<object> changeCntABlockErase;
    
    [SerializeField] private PlayerData _PlayerData;
    [SerializeField] private float distancePlayerRun;
    [SerializeField] private float timePlayerRun;
    [SerializeField] private int cntABlockErase;
    [SerializeField] private GameObject teleport;
    
    private void Awake()
    {
        SetupPlayerData();
    }

    protected void OnEnable()
    {
        turnManager = param => TurnManager((GameTurn) param);
        addStats = param => AddStats((Stats)param);
        changeCntABlockErase = param => ChangeCntABlockErase((int)param);
        
        ObserverManager<EventID>.RegisterEvent(EventID.TurnManager, turnManager);
        
        cntABlockErase = -1;
        ObserverManager<EventID>.RegisterEvent(EventID.UpdateStatsPlayer, addStats);
        ObserverManager<EventID>.RegisterEvent(EventID.SendCntBlockErase, changeCntABlockErase);
        ObserverManager<GameTurn>.RegisterEvent(GameTurn.PlayerTurn, addStats);
    }
    protected void OnDisable()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.TurnManager, turnManager);
        
        ObserverManager<EventID>.RemoveEvent(EventID.UpdateStatsPlayer, addStats);
        ObserverManager<EventID>.RemoveEvent(EventID.SendCntBlockErase, changeCntABlockErase);
        ObserverManager<GameTurn>.RemoveEvent(GameTurn.PlayerTurn, addStats);
        DOTween.Kill(transform);
    }

    protected void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void SetupPlayerData()
    {
        _PlayerData = Resources.Load<PlayerData>("ScriptTableObject/Player Data");
        m_ActorStats = _PlayerData.stats;
        distancePlayerRun = _PlayerData.distancePlayerRun;
        timePlayerRun = _PlayerData.timePlayerRun;
        animator = gameObject.GetComponent<Animator>();
        timeSpawn = _PlayerData.timeSpawn;
        timeDespawn = _PlayerData.timeDespawn;
        teleport =  _PlayerData.teleport;
    }
    private void TurnManager(GameTurn turn)
    {
        switch (turn)
        {
            case GameTurn.SpawnPlayer:
                PlayerSpawn();
                break;
            case GameTurn.SpawnEnemy:

                break;
            case GameTurn.EmptyTimeTurn:
                PlayerRun();
                break;
            case GameTurn.PlayerTurn:

                break;
            case GameTurn.EnemyTurn:
                
                break;
            default:
                Debug.LogError($"{this.GetType().Name}: Error TurnManager");
                break;
        }
    }
    private void PlayerSpawn()
    {
        StartCoroutine(PlayerSpawner());
    }
    private IEnumerator PlayerSpawner()
    {
        transform.DOScale(Vector3.zero, timeSpawn).From();
        GameObject tele = Instantiate(teleport, transform.position, Quaternion.identity);
        tele.transform.DOScale(Vector3.zero, timeSpawn).From();
        yield return new WaitForSeconds(timeSpawn);
        Debug.Log($"{this.GetType().Name}: Player spawn done");
        // TODO: Turn off teleport
        tele.SetActive(false);
        GameManager.Instance.ChangeTurn(GameTurn.EmptyTimeTurn);
    }
    private void PlayerRun()
    {
        StartCoroutine(PlayerRunning());
    }

    private IEnumerator PlayerRunning()
    {
        transform.DOMoveX(transform.position.x + distancePlayerRun, timePlayerRun).SetEase(Ease.Linear);
        animator.SetBool("Run", true);
        ObserverManager<EventID>.PostEvent(EventID.PlayerMove, (distancePlayerRun, timePlayerRun));
        yield return new WaitForSeconds(timePlayerRun);
        animator.SetBool("Run", false);
        GameManager.Instance.ChangeTurn(GameTurn.SpawnEnemy);
    }
    private void ChangeCntABlockErase(int newCntABlockErase)
    {
        cntABlockErase = newCntABlockErase;
    }
    protected override void AddStats(Stats stats)
    {
        if (GameManager.Instance._GameTurn == GameTurn.PlayerTurn)
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
            base.AddStats(isAttack);
            GameManager.Instance._GameTurn = GameTurn.PlayerTurn;
        }
    }
    
    protected void PlayerAttackEnemy(Stats stats)
    {
        StartCoroutine(PlayerAttack(stats));
    }

    private IEnumerator PlayerAttack(Stats stats)
    {
        Vector3 tam = transform.position;
        transform.DOMove(GameManager.Instance._enemyTarget.position - Vector3.right * 10f, 1f, false);
        animator.SetTrigger("Attack");
        ObserverManager<GameTurn>.PostEvent(GameTurn.EnemyTurn, stats);
        yield return new WaitForSeconds(1f);
        transform.DOMove(tam, 1f, false);
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