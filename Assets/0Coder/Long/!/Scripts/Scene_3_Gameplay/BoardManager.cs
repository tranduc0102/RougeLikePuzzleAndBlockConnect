using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using DesignPattern.Obsever;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;
using Duc;

public enum Gameplay
{
    checkInsertBlock,
    moveToDefault,
    blockIsUse,
    spawnBlock,
    blockAddStats,
    addBlockDontUse
}

public class BoardManager : MonoBehaviour
{
    private Queue<Tween> m_Tweens = new Queue<Tween>();
    private Action<object> m_CheckInsertBlock;
    private Action<object> m_AddBlockDontUse;
    
    [Header("----- Auto set up data -----")]
    [SerializeField] private BoardData _boardData;
    [SerializeField] private int rowNumber;
    [SerializeField] private int columnNumber;
    [SerializeField] private float distanceBlock;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private List<Transform> blockDontUse;
    [SerializeField] private LayerMask layerMask;
    
    private List<List<Transform>> blocks;
    private void Awake()
    {
        SetupData();
    }
    private Action<object> resetHandler;

    private void Start()
    {
        resetHandler = param => ResetBoard();
        ObserverManager<EventID>.RegisterEvent(EventID.ResetGame, resetHandler);
    }
    private void OnEnable()
    {
        m_CheckInsertBlock = param => AddBlockOnCell((Transform)param);
        m_AddBlockDontUse = param => AddBlockDontUse((Transform)param);
        
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.checkInsertBlock, m_CheckInsertBlock);
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.addBlockDontUse, m_AddBlockDontUse);
        CreateBoard();
    }

    private void OnDisable()
    {
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.checkInsertBlock, m_CheckInsertBlock);
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.addBlockDontUse, m_AddBlockDontUse);

        while (m_Tweens.Count > 0)
        {
            m_Tweens.Dequeue()?.Kill();
        }
        ObserverManager<EventID>.RemoveEvent(EventID.ResetGame, resetHandler);
    }
    private void SetupData()
    {
        _boardData = Resources.Load<BoardData>("ScriptTableObject/Board Data");
        rowNumber = _boardData.rowNumber;
        columnNumber = _boardData.columnNumber;
        distanceBlock = _boardData.distanceBlock;
        cellPrefab = _boardData.cellPrefab;
        layerMask = _boardData.layerMask;
        blocks = new List<List<Transform>>();
        for (int i = 0; i < rowNumber; ++i)
        {
            blocks.Add(new List<Transform>());
            for (int j = 0; j < columnNumber; ++j)
            {
                blocks[i].Add(null);
            }
        }
        blockDontUse = new List<Transform>();
    }
    private void AddBlockDontUse(Transform block)
    {
        blockDontUse.Add(block);
    }

    private void RemoveBlockIsUse(Transform block)
    {
        blockDontUse.Remove(block);
    }

    private void CreateBoard()
    {
        for (int i = 0; i < rowNumber; ++i)
        {
            for (int j = 0; j < columnNumber; ++j)
            {
                GameObject newCell = Instantiate(cellPrefab, transform);
                newCell.name = $"Cell {i} : {j}";
                newCell.transform.localPosition = new Vector3(i * distanceBlock, j * distanceBlock, 0f);
            }
        }
    }

    private void AddBlockOnCell(Transform block)
    {
        if (CheckInsertBlock(block))
        {
            // TODO: Adding block on cell
            SetPositionBlockToCell(block);
            ObserverManager<Gameplay>.PostEvent(Gameplay.blockIsUse, block);
            // remove block is use on TODO: Block don't use
            RemoveBlockIsUse(block);
            if (blockDontUse.Count == 0)
            {
                ObserverManager<Gameplay>.PostEvent(Gameplay.spawnBlock);
            }
            CheckRowAndColumn();
            if (CheckGameContinue())
            {
                // TODO: Continue game
            }
            else
            {
                // TODO: Game lose
                ObserverManager<EventID>.PostEvent(EventID.Lose);
            }
        }
        else
        {
            ObserverManager<Gameplay>.PostEvent(Gameplay.moveToDefault);
        }
    }

    private bool CheckGameContinue()
    {
        bool continueGame;
        float _r, _c;
        foreach (Transform block in blockDontUse)
        {
            for (int i = 0; i < rowNumber; ++i)
            {
                for (int j = 0; j < columnNumber; ++j)
                {
                    continueGame = true;
                    if (blocks[i][j] != null)
                    {
                        continueGame = false;
                        continue;
                    }
                    float r = block.GetChild(0).position.x, c = block.GetChild(0).position.y;
                    for (int k = 1; k < block.childCount; ++k)
                    {
                        _r = block.GetChild(k).position.x - r;
                        _c = block.GetChild(k).position.y - c;
                        int id1 = (int) (_r / distanceBlock) + i, id2 = (int) (_c / distanceBlock) + j;
                        if (id1 < 0 || id1 >= rowNumber || id2 < 0 || id2 >= columnNumber || blocks[id1][id2] != null)
                        {
                            continueGame = false;
                            break;
                        }
                    }
                    if (continueGame)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void SetPositionBlockToCell(Transform block)
    {
        int n = block.childCount;
        GameObject obj;
        Vector3 newPos;
        int check = 0;
        for (int i = 0; i < n; ++i)
        {
            obj = Physics2D.OverlapPoint(block.GetChild(i).position, layerMask, 0f).gameObject;
            newPos = obj.transform.position;
            newPos.z = -0.5f;
            m_Tweens.Enqueue(block.GetChild(i).DOMove(newPos, 0.2f));
            blocks[(int)((newPos.x - transform.GetChild(0).position.x) / distanceBlock)][(int)((newPos.y - transform.GetChild(0).position.y) / distanceBlock)] = block.GetChild(i);
            check++;
            if(check == n)
            {
                GameManager.Instance.AmountMovementBlock -= 1;
            }
        }
    }
    
    private bool CheckInsertBlock(Transform block)
    {
        int n = block.childCount;
        for (int i = 0; i < n; ++i)
        {
            if (CheckBlockAndCell(block.GetChild(i)) == false)
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckBlockAndCell(Transform block)
    {
        Collider2D collider2D = Physics2D.OverlapPoint(block.position, layerMask, 0f, 0f);
        return collider2D != null && blocks[(int) ((collider2D.transform.position.x - transform.GetChild(0).position.x) / distanceBlock)][(int) ((collider2D.transform.position.y - transform.GetChild(0).position.y) / distanceBlock)] == null;
    }
    
    private void CheckRowAndColumn()
    {
        List<(int, int)> eraseRowAndColumn = new List<(int, int)>();
        for (int i = 0; i < rowNumber; ++i)
        {
            if (CheckRow(i))
            {
                eraseRowAndColumn.Add((i, -1));
            }
        }

        for (int i = 0; i < columnNumber; ++i)
        {
            if (CheckColumn(i))
            {
                eraseRowAndColumn.Add((-1, i));
            }
        }
        List<Transform> removeRowAndColumn = new List<Transform>();
        // TODO: Erase row and column
        // TODO: Add block ad stats
        foreach ((int, int) child in eraseRowAndColumn)
        {
            if (child.Item1 != -1)
            {
                for (int i = 0; i < columnNumber; ++i)
                {
                    if (blocks[child.Item1][i] != null)
                    {
                        SetAnimationBlock(blocks[child.Item1][i]);
                        removeRowAndColumn.Add(blocks[child.Item1][i]);
                        blocks[child.Item1][i] = null;
                    }
                }
            }
            else if (child.Item2 != -1)
            {
                for (int i = 0; i < rowNumber; ++i)
                {
                    if (blocks[i][child.Item2] != null)
                    {
                        SetAnimationBlock(blocks[i][child.Item2]);
                        removeRowAndColumn.Add(blocks[i][child.Item2]);
                        blocks[i][child.Item2] = null;
                    }
                }
            }
            else
            {
                Debug.LogError("Error erase row and column");
            }
        }
        // TODO: Gửi số lượng a block bị xóa cho player stats để kiểm tra số lượng gửi đã đủ chưa
        ObserverManager<EventID>.PostEvent(EventID.SendCntBlockErase, removeRowAndColumn.Count);
        int count = 0;
        foreach (Transform child in removeRowAndColumn)
        {
            AddStats(child);
            count++;
            if(count == removeRowAndColumn.Count)
            {
                AudioManager.PlaySFX(SoundType.FXConnect);
            }
        }
    }

    private void AddStats(Transform target)
    {
        ObserverManager<Gameplay>.PostEvent(Gameplay.blockAddStats, target);
    }
    
    private void SetAnimationBlock(Transform tmp)
    {
        m_Tweens.Enqueue(tmp.DOShakeScale(0.5f, 0.5f, 10, 90f, true, ShakeRandomnessMode.Full)
            .OnComplete(() =>
            {
                tmp.DOScale(Vector3.zero, 0.5f)
                    .SetEase(Ease.InExpo)
                    .OnComplete(() => tmp.gameObject.SetActive(false));
            }));
    }
    
    private bool CheckRow(int row)
    {
        for (int i = 0; i < columnNumber; ++i)
        {
            if (blocks[row][i] == null)
            {
                return false;
            }
        }
        return true;
    }
    
    private bool CheckColumn(int column)
    {
        for (int i = 0; i < rowNumber; ++i)
        {
            if (blocks[i][column] == null)
            {
                return false;
            }
        }
        return true;
    }
    public void ResetBoard()
    {
        while (m_Tweens.Count > 0)
        {
            m_Tweens.Dequeue()?.Kill();
        }

        for (int i = 0; i < rowNumber; ++i)
        {
            for (int j = 0; j < columnNumber; ++j)
            {
                if (blocks[i][j] != null)
                {
                    blocks[i][j].gameObject.SetActive(false);
                    blocks[i][j] = null;
                }
            }
        }
        foreach (var block in blockDontUse)
        {
            if (block != null)
            {
                block.gameObject.SetActive(false);
            }
        }
        blockDontUse.Clear();

        // Optionally: Xóa các block là con của bảng (nếu cần reset cứng)
        // foreach (Transform child in transform)
        // {
        //     if (child.CompareTag("Block")) // giả sử các block gắn tag này
        //     {
        //         Destroy(child.gameObject);
        //     }
        // }
        ObserverManager<Gameplay>.PostEvent(Gameplay.spawnBlock);

        Debug.Log("Board has been reset.");
    }


}