// Author: Dan_lang_A (DauHang)

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DesignPattern.Obsever;
using DesignPattern.ObjectPool;
using Unity.Collections;

public class WayManager : MonoBehaviour
{
    [Header("----- Auto set up data -----")]
    [SerializeField] private List<Way> _listEnemyInWay;
    [SerializeField] private int _currentLevel;
    [SerializeField] private int _idCurrentWay;
    private void Awake()
    {
        SetUpData();
    }
    private void OnEnable()
    {
        ObserverManager<EventID>.RegisterEvent(EventID.SpawnNextWay, param => SpawnNextWay());
    }
    private void OnDisable()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.SpawnNextWay, param => SpawnNextWay());
    }

    private void SetUpData()
    {
        _currentLevel = GameManager.Instance.currentLevel;
        _listEnemyInWay = Resources.Load<LevelData>("ScriptTableObject/Level Data").Levels[_currentLevel].Ways;
        _idCurrentWay = 0;
    }
    private void SpawnNextWay()
    {
        if (_idCurrentWay >= _listEnemyInWay.Count)
        {
            Debug.LogWarning($"{this.GetType().Name}: Win game");
        }
        else
        {
            // TODO: Spawn Enemy
            Debug.Log($"{this.GetType().Name}: Spawning next way");
            SpawnEnemy.Instance.StartSpawn(_listEnemyInWay[_idCurrentWay]);
            ++ _idCurrentWay;
        }
    }
}
