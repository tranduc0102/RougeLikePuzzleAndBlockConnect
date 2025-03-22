
using System.Collections.Generic;
using DesignPattern;
using DesignPattern.Obsever;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    private int currentLevel = 0;
    [SerializeField] private GameTurn m_CurrentTurn;

    [Header("Manage enemy")] 
    [SerializeField] private List<EnemyStats> m_EnemyTracker;

    public EnemyStats EnemySelected;

    private void Start()
    {
        //When finish spawn way
        ObserverManager<EventID>.RegisterEvent(EventID.OnCompleteSpawnWay, param=>
        {
            SelectRandomEnemy();
            m_CurrentTurn = GameTurn.PlayerTurn;
            ObserverManager<GameTurn>.PostEvent(m_CurrentTurn);
        });
        PlayLevel();
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
        
        ObserverManager<EventID>.PostEvent(EventID.SpawnNextWay);

        
        
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
public enum GameTurn
{
    EnemyTurn,
    PlayerTurn
}

public enum EventID
{
    Win,
    Lose,
    UpdateStatsPlayer,
    SpawnNextWay,
    OnCompleteSpawnWay
}
