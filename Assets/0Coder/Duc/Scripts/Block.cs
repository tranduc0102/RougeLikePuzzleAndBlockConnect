using System.Collections.Generic;
using System.Linq;
using DesignPattern.ObjectPool;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace BlockConnectGame
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private Shape shape;
        [SerializeField] private List<DataBlock> datas;
        [SerializeField] private List<BlockType> listBlock;

        [SerializeField] private Slot _slot;
        [SerializeField] private SortingGroup _sortingGroup;

        public SortingGroup SortingGroup
        {
            get => _sortingGroup;
            set => _sortingGroup = value;
        }

        private void OnEnable()
        {
            InitBlock();
        }

        private void InitBlock()
        {
            foreach (var block in listBlock)
            {
                block.InitData(datas[Random.Range(0, datas.Count)]);
                block.gameObject.SetActive(true);
                block.SetActiveCollider(true);
                block.transform.localScale = Vector3.one;
            }
        }

        public void OnSetToGrid()
        {
            _slot?.Release();
        }

        public void OnSpawn(Slot slot)
        {
            _slot = slot;
        }

        private bool AllowSetToGrid()
        {
            foreach (var tile in listBlock)
            {
                if (!tile.gameObject.activeSelf) continue;

                RaycastHit2D hit = tile.Movable.Hit();
                if (hit.collider == null || !hit.collider.GetComponent<MyTiles>().IsEmpty)
                {
                    return false;
                }
            }
            return true;
        }

        private void SetPositionAll()
        {
            foreach (var tile in listBlock)
            {
                if (!tile.gameObject.activeSelf) continue;
                tile.Movable.SetPositionToHit();
            }
        }

        private void BackHomeAll()
        {
            foreach (var tile in listBlock)
            {
                if (!tile.gameObject.activeSelf) continue;
                tile.Movable.BackHome();
            }
        }
        public void OnPointerUp()
        {
            if (AllowSetToGrid())
            {
                SetPositionAll();
                OnSetToGrid();
                DOVirtual.DelayedCall(0.5f, delegate
                {
                    SpawnBlock.Instance.Check();
                });
            }
            else
            {
                BackHomeAll();
            }
        }

        public void CheckDespawnAll()
        {
            if (!listBlock.Any(blk => blk.gameObject.activeSelf))
            {
                PoolingManager.Despawn(this.gameObject);
            }
        }
        public List<Vector2Int> GetTiles()
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            switch (shape)
            {
                case Shape.Square:
                    positions.Add(new Vector2Int(0, 0));
                    positions.Add(new Vector2Int(1, 0));
                    positions.Add(new Vector2Int(0, 1));
                    positions.Add(new Vector2Int(1, 1));
                    break;

                case Shape.Dot:
                    positions.Add(new Vector2Int(0, 0)); 
                    break;

                case Shape.T:
                    positions.Add(new Vector2Int(0, 0)); 
                    positions.Add(new Vector2Int(-1, 0));
                    positions.Add(new Vector2Int(1, 0));
                    positions.Add(new Vector2Int(0, 1));
                    break;
            }

            return positions;
        }

    }

    public enum Shape
    {
        T,
        Square,
        Dot
    }
}
