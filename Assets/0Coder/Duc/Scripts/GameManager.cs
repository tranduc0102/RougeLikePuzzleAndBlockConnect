using DesignPattern;
using DesignPattern.Obsever;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UIGame;
using UnityEngine;

namespace Duc
{
    public enum EventID
    {
        Win,
        ResetGame,
        Lose,
        SendCntBlockErase,
        UpdateStatsPlayer,
    }
    public enum StatePlayer
    {
        PlayerMove
    }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private TextMeshProUGUI textAmountMove;
        public bool IsTurnPlayer { get; set; }
        private Player player;
        public Enemy TargetEnemy { get; set; }
        private List<Enemy> allEnemyInWay = new List<Enemy>();
        public List<Enemy> AllEnemyOutWay
        {
            get
            {
                return allEnemyInWay;
            }
            set
            {
                allEnemyInWay = value;
            }
        }
        private int amountMovementBlock = 6; //Kiểm tra xem có di chuyển đủ số lượt chưa, sau khi di chuyển đủ thì mới đc tấn công
        public int AmountMovementBlock
        {
            get
            {
                return amountMovementBlock;
            }
            set
            {
                amountMovementBlock = value;
                textAmountMove.text = amountMovementBlock.ToString();
            }
        }

        private void Start()
        {
            AmountMovementBlock = 6;
            player = FindObjectOfType<Player>();
            IsTurnPlayer = false;
            UIController.Instance.UIPause.SetActionResart(() => ResetGameManager());
            UIController.Instance.UILose.SetActionReplay(() => ResetGameManager());
            UIController.Instance.UIWin.SetActionReplay(() => ResetGameManager());
            UIController.Instance.UIWin.SetActionNextLevel(() => ResetGameManager());
            ObserverManager<EventID>.RegisterEvent(EventID.Win, param => Win());
            ObserverManager<EventID>.RegisterEvent(EventID.Lose, param => Lose());
            player.PlayerMove();

        }
        private void OnDestroy()
        {
            ObserverManager<EventID>.RemoveEvent(EventID.Win, param => Win());
            ObserverManager<EventID>.RemoveEvent(EventID.Lose, param => Lose());
        }

        public void EnemyTurn()
        {
            if (allEnemyInWay.Any(e => e.Alive))
            {
                StartTurnEnemy();
            }
            else
            {
                IsTurnPlayer = true;
                if (WayEnemyManager.Instance.CheckNextWay)
                {
                    player.PlayerMove();
                }
                else
                {
                    Debug.LogWarning($"{this.GetType().Name}: Win game");
                    ObserverManager<EventID>.PostEvent(EventID.Win);
                }
            }
        }
        int enemyIndex = 0;
        private void StartTurnEnemy()
        {
            if (IsTurnPlayer) return;
            if (enemyIndex >= allEnemyInWay.Count)
            {
                enemyIndex = 0;
                IsTurnPlayer = true;
                UIController.Instance.UIShowTurn.ShowTurn(true);
                return;
            }
            else
            {
                if (allEnemyInWay[enemyIndex].Alive)
                {
                    allEnemyInWay[enemyIndex].TakeTurn(true, () => StartTurnEnemy());
                    enemyIndex++;
                }
                else
                {
                    enemyIndex++;
                    StartTurnEnemy();
                }
            }
        }
        public void PlayerTurn(Actor target)
        {
            if (IsTurnPlayer && AmountMovementBlock <= 0)
            {
                player.SetTarget(target);
                player.TakeTurn(true, () =>
                {
                    AmountMovementBlock = 6;
                    IsTurnPlayer = false;
                    UIController.Instance.UIShowTurn.ShowTurn(false);
                });
            }
        }

        public List<Enemy> GetEnemiesInCurrentWay()
        {
            return allEnemyInWay;
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        GameManager.Instance.PlayerTurn(enemy);
                    }
                }
            }
        }
        private void ResetGameManager()
        {
            ObserverManager<EventID>.PostEvent(EventID.ResetGame);
            IsTurnPlayer = false;
            AmountMovementBlock = 6;
            TargetEnemy = null;
            if (player == null)
                player = FindObjectOfType<Player>();
            enemyIndex = 0;
            player.PlayerMove();
            Debug.Log("GameManager has been reset.");
        }
        private void Win()
        {
            UIController.Instance.UIWin.ShowDisplay(true);
        }
        private void Lose()
        {
            UIController.Instance.UILose.ShowDisplay(true);

        }
    }
}
