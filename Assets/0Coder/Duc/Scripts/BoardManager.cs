using System.Collections;
using System.Collections.Generic;
using DesignPattern;
using DesignPattern.ObjectPool;
using DG.Tweening;
using UnityEngine;

namespace BlockConnectGame
{
    public class BoardManager : Singleton<BoardManager>
    {
        [SerializeField] private MyTiles tilePrefab;
        [SerializeField] private int amountColum, amountRow;
        private MyTiles[,] grid;

        private void Start()
        {
            grid = new MyTiles[amountRow, amountColum];
            SpawnBoardGame();
        }

        public void SpawnBoardGame()
        {
            for (int i = 0; i < amountRow; i++)
            {
                for (int j = 0; j < amountColum; j++)
                {
                    MyTiles tile = PoolingManager.Spawn(tilePrefab, new Vector3(i * 6f, j * 6f, 0f), Quaternion.identity, transform);
                    tile.name = $"{i}, {j}";
                    tile.SetXY(i, j);
                    grid[i, j] = tile;
                }
            }

          
        }

        public void CheckGrid(int row, int column)
        {
            if (IsFullRow(row))
            {
                ClearRow(row);
                Debug.Log("Full Row");
            }

            if (IsFullColumn(column))
            {
                ClearColumn(column);
                Debug.Log("Full Colum");
            }
        }

        private bool IsFullRow(int row)
        {
            for (int j = 0; j < amountColum; j++)
            {
                if (grid[row, j].IsEmpty) return false;
            }
            return true;
        }

        private bool IsFullColumn(int column)
        {
            for (int i = 0; i < amountRow; i++)
            {
                if (grid[i, column].IsEmpty) return false;
            }
            return true;
        }

        private void ClearRow(int row)
        {
            float duration = 0.2f;
            for (int j = 0; j < amountColum;j++)
            {
                if (grid[row, j])
                {
                    BlockType blockType = grid[row, j].Child;
                    blockType.transform.DOScale(Vector3.zero, duration * (j + 1)).OnComplete(delegate
                    {
                        blockType.Movable.CheckDespawnAll();
                    });
                    grid[row, j].Child.Use();
                    grid[row, j].IsEmpty = true;
                }
            }
        }

        private void ClearColumn(int column)
        {
            float duration = 0.2f;
            for (int i = 0; i < amountRow; i++ )
            {
                if (grid[i, column])
                {
                    BlockType blockType = grid[i, column].Child;
                    blockType.transform.DOScale(Vector3.zero, duration * (i + 1)).OnComplete(delegate
                    {
                        blockType.Movable.CheckDespawnAll();
                    });
                    grid[i, column].Child.Use();
                    grid[i, column].IsEmpty = true;
                }
            }
        }
    }
}
