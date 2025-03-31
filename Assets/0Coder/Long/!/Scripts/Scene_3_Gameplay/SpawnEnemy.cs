// Author: Dan_lang_A (DauHang)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DesignPattern;
using UnityEngine.SceneManagement;
using DesignPattern.Obsever;
using DesignPattern.ObjectPool;
using DG.Tweening;

public class SpawnEnemy : Singleton<SpawnEnemy>
{
    [Header("----- Auto set up data -----")] 
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private int _level;
    protected override void Awake()
    {
        base.Awake();
        SetupData();
    }
    private void SetupData()
    {
        _enemyData = Resources.Load<EnemyData>("ScriptTableObject/Enemy Data");
    }
    public void StartSpawn(Way way)
    {
        int n = way.Enemies.Count;
        EnemyInfor _enemyInfor;
        EnemyParam _enemyParam;
        for (int i = 0; i < n; ++i)
        {
            _enemyInfor = way.Enemies[i];
            _enemyParam = _enemyData.Enemies[_enemyInfor.EnemyId];
            GameObject _enemyObj = Instantiate(_enemyParam.EnemyPrefab, _enemyInfor.EnemyPosition, Quaternion.identity);
            _enemyObj.transform.localScale = Vector3.one * 11f;
            _enemyObj.transform.DORotate(Vector3.up * -90f, 1f, RotateMode.Fast);
        }
    }
}
