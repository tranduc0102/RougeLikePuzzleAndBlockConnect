// Author: Dan_lang_A (DauHang)

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DesignPattern;
using UnityEngine.SceneManagement;
using DesignPattern.Obsever;
using DesignPattern.ObjectPool;
using Unity.Collections;

public class WayManager : Singleton<WayManager>
{
    [Header("----- Auto set up data -----")]
    [SerializeField] private List<Way> _listEnemyInWay;
    [SerializeField] private int _currentLevel;
    [SerializeField] private int _idCurrentWay;
    protected override void Awake()
    {
        base.Awake();
        SetUpData();
    }
    private void SetUpData()
    {
        _currentLevel = GameManager.Instance.currentLevel;
        _listEnemyInWay = Resources.Load<LevelData>("ScriptTableObject/Level Data").Levels[_currentLevel].Ways;
        _idCurrentWay = 0;
    }
    public void SpawnNextWay()
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
