using DesignPattern.Obsever;
using DG.Tweening;
using Duc;
using System;
using TMPro;
using UIGame;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class Player : Actor
{
    [SerializeField] private PlayerData _PlayerData;
    [SerializeField] private float timePlayerRun;
    [SerializeField] private GameObject teleport;

    [SerializeField] private TextMeshProUGUI textHP;
    [SerializeField] private TextMeshProUGUI textATK;
    [SerializeField] private TextMeshProUGUI textDEF;


    private void Awake()
    {
        SetupPlayerData();
    }
    private void SetupPlayerData()
    {
        _PlayerData = Resources.Load<PlayerData>("ScriptTableObject/Player Data");
        m_ActorStats.HealthPoint = _PlayerData.stats.HealthPoint;
        textHP.text = m_ActorStats.HealthPoint.ToString();
        m_ActorStats.Armor = _PlayerData.stats.Armor;
        textDEF.text = m_ActorStats.Armor.ToString();

        m_ActorStats.PhysicalDamage = _PlayerData.stats.PhysicalDamage;
        m_ActorStats.MagicalDamage = _PlayerData.stats.MagicalDamage;

        textATK.text = (m_ActorStats.PhysicalDamage + m_ActorStats.MagicalDamage).ToString();

        distancePlayerRun = _PlayerData.distancePlayerRun;
        distanceActorRun = -10f;
        timePlayerRun = _PlayerData.timePlayerRun;
        animator = gameObject.GetComponent<Animator>();
        timeSpawn = _PlayerData.timeSpawn;
        timeDespawn = _PlayerData.timeDespawn;
        teleport = _PlayerData.teleport;
        Alive = true;
        target = null;
        gameObject.SetActive(true);
    }
    private Action<object> addStats;
    private Action<object> changeCntABlockErase;
    [SerializeField] private int cntABlockErase; 
    private Action<object> resetGameHandler;
    private float distancePlayerRun;

    private void Start()
    {
        addStats = param => AddStats((Duc.Stats)param);
        changeCntABlockErase = param => ChangeCntABlockErase((int)param);
        resetGameHandler = param => ResetGOB();
        ObserverManager<EventID>.RegisterEvent(EventID.UpdateStatsPlayer, addStats);
        ObserverManager<EventID>.RegisterEvent(EventID.SendCntBlockErase, changeCntABlockErase);
        ObserverManager<EventID>.RegisterEvent(EventID.ResetGame, resetGameHandler);
    }
    private void OnDestroy()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.UpdateStatsPlayer, addStats);
        ObserverManager<EventID>.RemoveEvent(EventID.SendCntBlockErase, changeCntABlockErase);
        ObserverManager<EventID>.RemoveEvent(EventID.ResetGame, resetGameHandler);
        DOTween.Kill(gameObject);
    }
    private void ResetGOB()
    {
        SetupPlayerData();
        this.gameObject.SetActive(true);
    }
    protected override void Attack()
    {
        base.Attack();
    }
    public override void ReceiveDamaged()
    {
        base.ReceiveDamaged();
        textDEF.text = m_ActorStats.Armor.ToString();
        textHP.text = m_ActorStats.HealthPoint.ToString();
    }
    protected override void AddStats(Stats stats) {  
        base.AddStats(stats);
        textDEF.text = m_ActorStats.Armor.ToString();
        textHP.text = m_ActorStats.HealthPoint.ToString();
        textATK.text = (m_ActorStats.PhysicalDamage + m_ActorStats.MagicalDamage).ToString();
    }



    protected override void HandleDead()
    {
        gameObject.SetActive(false);
        ObserverManager<EventID>.PostEvent(EventID.Lose);
    }

    protected override void ProcessTurn(Action actionFinish)
    {
       base.ProcessTurn(actionFinish);
    }
    public override void TakeTurn(bool isMyTurn, Action actionFinish)
    {
        if (isMyTurn) { 
            ProcessTurn(actionFinish);
        }
    }

    private void ChangeCntABlockErase(int newCntABlockErase)
    {
        cntABlockErase = newCntABlockErase;
    }
    public void PlayerMove()
    {
        animator.SetBool("Run", true);
        ObserverManager<StatePlayer>.PostEvent(StatePlayer.PlayerMove, (distancePlayerRun, timePlayerRun));
        transform.DOMoveX(transform.position.x + distancePlayerRun, timePlayerRun).SetEase(Ease.Linear).OnComplete(delegate
        {
            animator.SetBool("Run", false);
            UIController.Instance.UIShowTurn.ShowTurn(true);
            WayEnemyManager.Instance.SpawnNextWay();
        });
    }
}

