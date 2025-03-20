using System.Collections.Generic;
using UnityEngine;

namespace BlockConnectGame
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private List<DataBlock> datas;
        [SerializeField] private List<BlockType> listBlock;

        [SerializeField] private Slot _slot;

        private void OnEnable()
        {
            InitBlock();
        }

        private void InitBlock()
        {
            foreach (var block in listBlock)
            {
                block.InitData(datas[Random.Range(0, datas.Count)]);
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
                    Debug.LogError($"{tile.name}: Không thể đặt lên lưới!");
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
                SpawnBlock.Instance.Check();
            }
            else
            {
                BackHomeAll();
            }
        }
    }
}
