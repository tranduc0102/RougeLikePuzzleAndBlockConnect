using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using DesignPattern.Obsever;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;

public enum Gameplay
{
    checkInsertBlock,
    moveToDefault,
    blockIsUse,
    spawnBlock
}

public class BoardController : MonoBehaviour
{
    [SerializeField] private int rowNumber;
    [SerializeField] private int columnNumber;
    [SerializeField] private float distanceBlock;
    [SerializeField] private GameObject gameObjectCell;
    private Transform[ ,] blocks;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 sizeBoxCheck;
    
    private void Awake()
    {
        blocks = new Transform[rowNumber, columnNumber];
        sizeBoxCheck = new Vector3(distanceBlock / 2f - 0.2f,  distanceBlock / 2f - 0.2f, 0f);
    }

    private void OnEnable()
    {
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.checkInsertBlock, param => AddBlockOnCell((Transform) param));
        CreateBoard();
    }

    private void OnDisable()
    {
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.checkInsertBlock, param => AddBlockOnCell((Transform) param));
    }

    private void CreateBoard()
    {
        for (int i = 0; i < rowNumber; ++i)
        {
            for (int j = 0; j < columnNumber; ++j)
            {
                GameObject newCell = Instantiate(gameObjectCell, transform);
                newCell.name = $"Cell {i} : {j}";
                newCell.transform.position = new Vector3(i * distanceBlock, j * distanceBlock, 0f);
            }
        }
    }

    private void AddBlockOnCell(Transform target)
    {
        if (CheckInsertBlock(target))
        {
            Debug.Log("Adding block on cell");
            SetPositionBlockToCell(target);
            ObserverManager<Gameplay>.PostEvent(Gameplay.blockIsUse, target);
            CheckRowAndColumn();
            ObserverManager<Gameplay>.PostEvent(Gameplay.spawnBlock, 1);
        }
        else
        {
            ObserverManager<Gameplay>.PostEvent(Gameplay.moveToDefault);
        }
    }

    private void SetPositionBlockToCell(Transform target)
    {
        int n = target.childCount;
        GameObject obj;
        for (int i = 0; i < n; ++i)
        {
            obj = Physics2D.OverlapPoint(target.GetChild(i).position, layerMask, 0f).gameObject;
            target.GetChild(i).DOMove(obj.transform.position, 0.2f);
            blocks[(int)(obj.transform.position.x / distanceBlock), (int)(obj.transform.position.y / distanceBlock)] = target.GetChild(i);
        }
    }
    
    private bool CheckInsertBlock(Transform target)
    {
        int n = target.childCount;
        for (int i = 0; i < n; ++i)
        {
            if (CheckBlockAndCell(target.GetChild(i)) == false)
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckBlockAndCell(Transform target)
    {
        Collider2D collider2D = Physics2D.OverlapPoint(target.position, layerMask, 0f, 0f);
        return collider2D != null && blocks[(int) (collider2D.transform.position.x / distanceBlock), (int) (collider2D.transform.position.y / distanceBlock)] == null;
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
        // TODO: Erase row and column
        foreach ((int, int) child in eraseRowAndColumn)
        {
            if (child.Item1 != -1)
            {
                for (int i = 0; i < columnNumber; ++i)
                {
                    SetAnimationBlock(blocks[child.Item1, i]);
                    blocks[child.Item1, i] = null;
                }
            }
            else if (child.Item2 != -1)
            {
                for (int i = 0; i < rowNumber; ++i)
                {
                    SetAnimationBlock(blocks[i, child.Item2]);
                    blocks[i, child.Item2] = null;
                }
            }
            else
            {
                Debug.LogError("Error erase row and column");
            }
        }
    }

    private void SetAnimationBlock(Transform tmp)
    {
        tmp.DOShakeScale(0.5f, 0.5f, 10, 90f, true, ShakeRandomnessMode.Full)
            .OnComplete(() =>
            {
                tmp.DOScale(Vector3.zero, 0.5f)
                    .SetEase(Ease.InExpo)
                    .OnComplete(() => tmp.gameObject.SetActive(false));
            });
    }
    
    private bool CheckRow(int row)
    {
        for (int i = 0; i < columnNumber; ++i)
        {
            if (blocks[row, i] == null)
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
            if (blocks[i, column] == null)
            {
                return false;
            }
        }
        return true;
    }

}