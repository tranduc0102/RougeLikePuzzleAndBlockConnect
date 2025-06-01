using DesignPattern;
using DesignPattern.Obsever;
using DG.Tweening;
using System.Collections.Generic;
using UIGame;
using UnityEngine;

namespace Duc
{
    public class WayEnemyManager : Singleton<WayEnemyManager>
    {
        [Header("----- Auto set up data -----")]
        [SerializeField] private List<Way> _listEnemyInWay;
        [SerializeField] private int _currentLevel;
        [SerializeField] private int _idCurrentWay;
        [SerializeField] private SpawnWayEnemy SpawnWayEnemy;
        protected override void Awake()
        {
            base.Awake();
            SetUpData();
        }
        private void Start()
        {
            ObserverManager<EventID>.RegisterEvent(EventID.ResetGame, param => ResetWayEnemyManager());
        }
        private void OnDestroy()
        {
            ObserverManager<EventID>.RemoveEvent(EventID.ResetGame, param => ResetWayEnemyManager());
        }
        private void SetUpData()
        {
            _currentLevel = LevelManager.Instance.CurrentLevel - 1;
            _listEnemyInWay = Resources.Load<LevelData>("ScriptTableObject/Level Data").Levels[_currentLevel].Ways;
            _idCurrentWay = 0;
        }
        public void SpawnNextWay()
        {
            print("SpawnNextWay");
            if (_idCurrentWay < _listEnemyInWay.Count)
            {
                Debug.Log($"{this.GetType().Name}: Spawning next way");
                SpawnWayEnemy.StartSpawn(_listEnemyInWay[_idCurrentWay]);
                GameManager.Instance.AllEnemyOutWay.Clear();
                DOVirtual.DelayedCall(0.5f, delegate
                {
                    GameManager.Instance.AllEnemyOutWay.AddRange(FindObjectsOfType<Enemy>());

                });
                ++_idCurrentWay;
            }
        }
        public void ResetWayEnemyManager()
        {
            _idCurrentWay = 0;

            _currentLevel = LevelManager.Instance.CurrentLevel - 1;
            _listEnemyInWay = Resources.Load<LevelData>("ScriptTableObject/Level Data").Levels[_currentLevel].Ways;
            GameManager.Instance.AllEnemyOutWay.Clear();

            Debug.Log("WayEnemyManager has been reset.");
        }
        public bool CheckNextWay => _idCurrentWay < _listEnemyInWay.Count;

    }

}