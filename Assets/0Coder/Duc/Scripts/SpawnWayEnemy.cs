using DesignPattern;
using DesignPattern.ObjectPool;
using DesignPattern.Obsever;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duc
{
    public class SpawnWayEnemy : MonoBehaviour
    {
        private Queue<Tween> m_Tweens = new Queue<Tween>();

        [Header("----- Auto set up data -----")]
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private int _level;
        [SerializeField] private Player player;
        [SerializeField] private float _positionRight;
        [SerializeField] private float _positionUp;
        [SerializeField] private GameObject _teleport;
        [SerializeField] private float _timeSpawn;
        public List<Transform> _currentEnemies;
        public float timeDelayAttackPlayer;
        private List<Transform> objDespawn = new List<Transform>();
        protected void Awake()
        {
            SetupData();
        }
        private void Start()
        {
            ObserverManager<EventID>.RegisterEvent(EventID.ResetGame, param => ResetSpawnWayEnemy());
        }

        protected void OnDisable()
        {
            while (m_Tweens.Count > 0)
            {
                m_Tweens.Dequeue()?.Kill();
            }
            ObserverManager<EventID>.RemoveEvent(EventID.ResetGame, param => ResetSpawnWayEnemy());
        }

        private void SetupData()
        {
            _enemyData = Resources.Load<EnemyData>("ScriptTableObject/Enemy Data");
            _positionRight = _enemyData.PositionRight;
            _positionUp = _enemyData.PositionUp;
            _teleport = _enemyData.teleport;
            _timeSpawn = _enemyData.timeSpawn;
            _currentEnemies = new List<Transform>();
            timeDelayAttackPlayer = _enemyData.timeDelayAttackPlayer;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
                _currentEnemies[_currentEnemies.Count - 1].localScale = Vector3.one;
                m_Tweens.Enqueue(_currentEnemies[_currentEnemies.Count - 1].DORotate(Vector3.up * -180f, 1f, RotateMode.Fast));
                _currentEnemies[_currentEnemies.Count - 1].gameObject.GetComponent<Enemy>()._idEnemy = _enemyInfor.EnemyId;
                _currentEnemies[_currentEnemies.Count - 1].GetComponent<Enemy>().SetTarget(player);
                m_Tweens.Enqueue(_currentEnemies[_currentEnemies.Count - 1].DOScale(Vector3.zero, _timeSpawn).From());
            }
            AudioManager.PlaySFX(SoundType.FXSpawnEnemy);
        }
        public void ResetSpawnWayEnemy()
        {
            if (_currentEnemies != null)
            {
                foreach (var enemyTrans in _currentEnemies)
                {
                    if (enemyTrans != null)
                    {
                        Destroy(enemyTrans.gameObject);
                    }
                }
                _currentEnemies.Clear();
            }
        }

    }
}
