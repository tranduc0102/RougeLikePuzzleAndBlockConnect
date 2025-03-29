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

        private void OnEnable()
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
                    grid[row, j].IsEmpty = true;
                    BlockType blockType = grid[row, j].Child;
                    blockType.transform.DOScale(Vector3.zero, duration * (j + 1)).OnComplete(delegate
                    {
                        blockType.Movable.CheckDespawnAll();
                    });
                    grid[row, j].Child.Use();
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
                    grid[i, column].IsEmpty = true;
                    BlockType blockType = grid[i, column].Child;
                    blockType.transform.DOScale(Vector3.zero, duration * (i + 1)).OnComplete(delegate
                    {
                        blockType.Movable.CheckDespawnAll();
                    });
                    grid[i, column].Child.Use();
                }
            }
        }
        public bool CanPlaceAnyBlock(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (CanPlaceBlock(block)) return true;
            }
            return false;
        }

        private bool CanPlaceBlock(Block block)
        {
            foreach (var tile in grid)
            {
                if (tile.IsEmpty && CanFitBlockAt(block, tile.x, tile.y))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CanFitBlockAt(Block block, int startX, int startY)
        {
            foreach (var piece in block.GetTiles())
            {
                int targetX = startX + piece.x;
                int targetY = startY + piece.y;

                if (!IsValidTile(targetX, targetY) || !grid[targetX, targetY].IsEmpty)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidTile(int x, int y)
        {
            return x >= 0 && x < amountRow && y >= 0 && y < amountColum;
        }

    }
}
