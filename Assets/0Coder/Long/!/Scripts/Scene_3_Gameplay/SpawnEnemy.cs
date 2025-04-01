// Author: Dan_lang_A (DauHang)

using System;
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
    [SerializeField] private GameObject player;
    [SerializeField] private float _positionRight;
    [SerializeField] private float _positionUp;
    [SerializeField] private GameObject _teleport;
    [SerializeField] private float _timeSpawn;
    [SerializeField] private List<Transform> _currentEnemies;
    
    private List<Transform> objDespawn = new List<Transform>();
    protected override void Awake()
    {
        base.Awake();
        SetupData();
    }
    private void SetupData()
    {
        _enemyData = Resources.Load<EnemyData>("ScriptTableObject/Enemy Data");
        _positionRight = _enemyData.PositionRight;
        _positionUp = _enemyData.PositionUp;
        _teleport = _enemyData.teleport;
        _timeSpawn = _enemyData.timeSpawn;
        _currentEnemies = new List<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError($"{this.GetType().Name}: Player not found");
        }
    }
    public void StartSpawn(Way way)
    {
        int n = way.Enemies.Count;
        int idPos = 0;
        List<Vector3> positionEnemy = new List<Vector3>();
        switch (n)
        {
            case 1:
                positionEnemy.Add(player.transform.position + Vector3.right * _positionRight);
                break;
            case 2:
                positionEnemy.Add(player.transform.position + Vector3.right * _positionRight + Vector3.up * _positionUp);
                positionEnemy.Add(player.transform.position + Vector3.right * _positionRight - Vector3.up * _positionUp);
                break;
            case 3:
                positionEnemy.Add(player.transform.position + Vector3.right * _positionRight + Vector3.up * _positionUp);
                positionEnemy.Add(player.transform.position + Vector3.right * _positionRight);
                positionEnemy.Add(player.transform.position + Vector3.right * _positionRight - Vector3.up * _positionUp);
                break;
            default:
                Debug.LogError($"{this.GetType().Name}: Error set Position Enemy");
                break;
        }
        // TODO: Enemy target
        EnemyInfor _enemyInfor;
        EnemyParam _enemyParam;
        for (int i = 0; i < n; ++i)
        {
            _enemyInfor = way.Enemies[i];
            _enemyParam = _enemyData.Enemies[_enemyInfor.EnemyId];
            _currentEnemies.Add(Instantiate(_enemyParam.EnemyPrefab, positionEnemy[idPos], Quaternion.identity).transform);
            ++idPos;
            _currentEnemies[_currentEnemies.Count - 1].SetParent(transform);
            _currentEnemies[_currentEnemies.Count - 1].localScale = Vector3.one * 7f;
            _currentEnemies[_currentEnemies.Count - 1].DORotate(Vector3.up * -90f + Vector3.forward * 13f, 1f, RotateMode.Fast);
            _currentEnemies[_currentEnemies.Count - 1].gameObject.AddComponent<EnemyStats>()._idEnemy = _enemyInfor.EnemyId;
            objDespawn.Add(Instantiate(_teleport, _currentEnemies[_currentEnemies.Count - 1].position, Quaternion.identity).transform);
            objDespawn[objDespawn.Count - 1].DOScale(Vector3.zero, _timeSpawn).From();
            objDespawn[objDespawn.Count - 1].SetParent(transform);
            _currentEnemies[_currentEnemies.Count - 1].DOScale(Vector3.zero, _timeSpawn).From();
        }
        StartCoroutine(NextGameTurn());
    }
    private IEnumerator NextGameTurn()
    {
        yield return new WaitForSeconds(_timeSpawn);
        GameManager.Instance._enemyTarget = transform.GetChild(0);
        foreach (Transform trans in objDespawn)
        {
            trans.gameObject.SetActive(false);
        }
        GameManager.Instance.ChangeTurn(GameTurn.PlayerTurn);
    }
}
