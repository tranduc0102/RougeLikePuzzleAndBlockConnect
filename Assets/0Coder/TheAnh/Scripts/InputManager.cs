
using System;
using DesignPattern;
using DesignPattern.Obsever;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private GameObject m_EnemyPrefab;

    private void Start()
    {
        //demo spawn
        ObserverManager<EventID>.RegisterEvent(EventID.SpawnNextWay, param =>
        {
            for (int i = 0; i <= 3; ++i)
            {
                EnemyStats enemy = Instantiate(m_EnemyPrefab, new Vector3(2 * i, 0, 0), Quaternion.identity).GetComponent<EnemyStats>();
                if(enemy != null) GameManager.Instance.HandleAddEnemy(enemy);
            }
            //if spawn finish post event
            ObserverManager<EventID>.PostEvent(EventID.OnCompleteSpawnWay);
        });
    }

    private void Update()
    {
        HandleClickEnemy();
        //Demo handle enemydead
        if(Input.GetKeyDown(KeyCode.D)) GameManager.Instance.HandleEnemyDead(GameManager.Instance.EnemySelected);
    }

    private void HandleClickEnemy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    EnemyStats enemy = hit.collider.GetComponent<EnemyStats>();
                    if(enemy != null) GameManager.Instance.SelectEnemy(enemy);
                }
            }
        }
    }
    
}
