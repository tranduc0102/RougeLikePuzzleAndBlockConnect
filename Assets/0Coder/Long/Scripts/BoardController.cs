using System;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern.Obsever;
using DG.Tweening;

public enum Gameplay
{
    checkInsertBlock,
    moveToDefault,
    blockIsUse
}

public class BoardController : MonoBehaviour
{
    [SerializeField] private int rowNumber;
    [SerializeField] private int columnNumber;
    [SerializeField] private float distanceBlock;
    [SerializeField] private GameObject gameObjectCell;
    private Cell[ ,] cells;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 sizeBoxCheck;
    
    private void Awake()
    {
        sizeBoxCheck = new Vector3(distanceBlock / 2f - 0.2f,  distanceBlock / 2f - 0.2f, 0f);
    }

    private void OnEnable()
    {
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.checkInsertBlock, param => AddBlockOnCell((Transform) param));
        cells = new Cell[rowNumber, columnNumber];
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
                Cell newCellScript = newCell.GetComponent<Cell>();
                newCellScript.SetPosition(i * distanceBlock, j * distanceBlock);
                cells[i, j] =  newCellScript;
            }
        }
    }

    private bool CheckRow(int row)
    {
        for (int i = 0; i < columnNumber; ++i)
        {
            if (cells[row, i].IsEmpty())
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
            if (cells[i, column].IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    private void AddBlockOnCell(Transform target)
    {
        if (CheckInsertBlock(target))
        {
            Debug.Log("Adding block on cell");
            SetPositionBlockToCell(target);
            ObserverManager<Gameplay>.PostEvent(Gameplay.blockIsUse, target);
        }
        else
        {
            ObserverManager<Gameplay>.PostEvent(Gameplay.moveToDefault);
        }
    }

    private void SetPositionBlockToCell(Transform target)
    {
        int n = target.childCount;
        Collider2D collider2D;
        for (int i = 0; i < n; ++i)
        {
            collider2D = Physics2D.OverlapPoint(target.GetChild(i).position, layerMask, 0f);
            target.GetChild(i).DOMove(collider2D.gameObject.transform.position, 0.2f);
            // TODO: set cell.isempty = false
            collider2D.gameObject.GetComponent<Cell>().SetIsEmpty(false);
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
        return collider2D != null && collider2D.gameObject.GetComponent<Cell>().IsEmpty();
    }

    // TODO: Test va cham block va cell
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(Vector3.zero, new Vector3(distanceBlock / 2f, distanceBlock / 2f, 0f));
    // }
}