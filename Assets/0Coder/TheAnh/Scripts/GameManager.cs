
using DesignPattern;
using DesignPattern.Obsever;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
   
    
    [SerializeField] private GameTurn m_CurrentTurn;
    public void TakeTurn()
    {
        m_CurrentTurn = (m_CurrentTurn == GameTurn.EnemyTurn ? GameTurn.PlayerTurn : GameTurn.EnemyTurn);
        ObserverManager<GameTurn>.PostEvent(m_CurrentTurn);

    }
    
}


public enum GameTurn
{
    EnemyTurn,
    PlayerTurn
}

public enum EventId
{
    Win,
    Lose
}
