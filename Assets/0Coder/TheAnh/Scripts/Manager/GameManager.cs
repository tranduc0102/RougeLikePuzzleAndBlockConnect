
using System;
using System.Collections;
using System.Collections.Generic;
using DesignPattern;
using DesignPattern.Obsever;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum GameTurn
{
    EnemyTurn,
    PlayerTurn,
    EmptyTimeTurn,
    SpawnPlayer,
    SpawnEnemy
}

public enum EventID
{
    TurnManager,
    Win,
    Lose,
    SendCntBlockErase,
    UpdateStatsPlayer,
    OnCompleteSpawnWay,
    PlayerMove,
    EnemyAttack
}

public class GameManager : Singleton<GameManager>
{
    public GameTurn _GameTurn;
    public int currentLevel = 0;
    public Transform _enemyTarget;
    
    
    
    
    [SerializeField] private GameTurn m_CurrentTurn;

    [Header("Manage enemy")] 
    [SerializeField] private List<EnemyStats> m_EnemyTracker;

    public EnemyStats EnemySelected;

    private void OnEnable()
    {
        ObserverManager<EventID>.RegisterEvent(EventID.Lose, _ => HandleGameOver());
        ObserverManager<EventID>.RegisterEvent(EventID.Win, _ => HandleWin());
    }
    private void OnDisable()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.Lose, _ => HandleGameOver());
        ObserverManager<EventID>.RemoveEvent(EventID.Win, _ => HandleWin());
    }
    private void Start()
    {
        ChangeTurn(GameTurn.SpawnPlayer);
        
        //When finish spawn way
        ObserverManager<EventID>.RegisterEvent(EventID.OnCompleteSpawnWay, param=>
        {
            SelectRandomEnemy();
            m_CurrentTurn = GameTurn.PlayerTurn;
            ObserverManager<GameTurn>.PostEvent(m_CurrentTurn);
        });
        PlayLevel();
    }

    public void ChangeTurn(GameTurn turn)
    {
        _GameTurn = turn;
        switch (turn)
        {
            case GameTurn.SpawnPlayer:
                
                break;
            case GameTurn.SpawnEnemy:
                WayManager.Instance.SpawnNextWay();
                break;
            case GameTurn.EmptyTimeTurn:
                
                break;
            case GameTurn.PlayerTurn:
                
                break;
            case GameTurn.EnemyTurn:
                StartCoroutine(EnemyAttackPlayer());
                break;
            default:
                Debug.LogError($"{this.GetType().Name}: Error change turn");
                break;
        }
        ObserverManager<EventID>.PostEvent(EventID.TurnManager, turn);
    }
    private IEnumerator EnemyAttackPlayer()
    {
        for (int i = 0; i < SpawnEnemy.Instance._currentEnemies.Count; ++i)
        {
            if (SpawnEnemy.Instance._currentEnemies[i].gameObject.activeSelf)
            {
                ObserverManager<EventID>.PostEvent(EventID.EnemyAttack, SpawnEnemy.Instance._currentEnemies[i]);
                yield return new WaitForSeconds(SpawnEnemy.Instance.timeDelayAttackPlayer);
            }
        }
        ChangeTurn(GameTurn.PlayerTurn);
    }
    private void HandleGameOver()
    {
        // TODO: Xu ly game over
        Debug.LogWarning("Game Over");
    }

    private void HandleWin()
    {
        PlayerPrefs.SetInt("IsLevelCompleted" + currentLevel.ToString(),1);
        PlayerPrefs.Save();
    }
    public void TakeTurn()
    {
        m_CurrentTurn = (m_CurrentTurn == GameTurn.EnemyTurn ? GameTurn.PlayerTurn : GameTurn.EnemyTurn);
        ObserverManager<GameTurn>.PostEvent(m_CurrentTurn);
    }
    
    private void PlayLevel()
    {
        if (m_EnemyTracker == null) m_EnemyTracker = new List<EnemyStats>();
        m_EnemyTracker.Clear();
        
        // TODO: Fix ham nay sau
        // ObserverManager<EventID>.PostEvent(EventID.SpawnNextWay);
    }
    private void SelectRandomEnemy()
    {
        if (m_EnemyTracker.Count > 0)
        {
            int enemyIndex = Random.Range(0, m_EnemyTracker.Count);
            SelectEnemy(m_EnemyTracker[enemyIndex]);
        }
        else EnemySelected = null;
    }
    //handle in inputmanager
    public void SelectEnemy(EnemyStats enemy)
    {
        EnemySelected = enemy;
        Debug.Log(enemy.transform.position);
    }
    public void HandleAddEnemy(EnemyStats enemy)
    {
        if(m_EnemyTracker.Contains(enemy)) return;
        m_EnemyTracker.Add(enemy);
    }

    public void HandleEnemyDead(EnemyStats enemy)
    {
        m_EnemyTracker.Remove(enemy);
        if(enemy == EnemySelected) SelectRandomEnemy();
    }
}
