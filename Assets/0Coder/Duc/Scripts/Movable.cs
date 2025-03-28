using System;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
namespace BlockConnectGame
{
    public class Movable : MonoBehaviour
    {
        private Vector3 _offset;
        [SerializeField] private LayerMask mask;

        private BlockType _blockTile;
        private Block block;
        private Transform _currentMovable;
        private Vector3 _homePosition;
        private Vector3 originPosition;
        private bool reSpawn = false;

        private void Start()
        {
            _currentMovable = transform.parent;
            _homePosition = transform.position;
            _blockTile = GetComponent<BlockType>();
            block = transform.parent.GetComponent<Block>();
          
        }

        private void OnEnable()
        {
            if (reSpawn)
            {
                transform.localPosition = originPosition;
            }
        }

        #region Pointer
        
        public void OnMouseDown()
        {
            SetOffset(Input.mousePosition);
            block.SortingGroup.sortingOrder = 100;
        }
        public void OnMouseDrag()
        {
            Move(Input.mousePosition);
        }

        public void OnMouseUp()
        {
            block.OnPointerUp();
            block.SortingGroup.sortingOrder = 10;
        }

        private void SetOffset(Vector3 position)
        {
            Vector3 pos = position;
            pos.z = 0f;
            var target = Camera.main.ScreenToWorldPoint(pos);
            _offset = _currentMovable.position - target;
        }
        private void Move(Vector3 eventData)
        {
            Vector3 pos = eventData;
            pos.z = 0f;
            var target = Camera.main.ScreenToWorldPoint(pos);
            target += _offset;
            target.z = 0;
            _currentMovable.position = target;
        }
        #endregion
        public void SetPositionToHit()
        {
            var hit = Hit();
            var baseTile = hit.transform.GetComponent<MyTiles>();
            var target = hit.transform.position;
            target.z = 0f;
            _blockTile.SetActiveCollider(false);
            baseTile.IsEmpty = false;
            baseTile.Child = _blockTile;
            AnimationPlaced(target, 0.3f, baseTile);
        }

        public void BackHome()
        { 
            AnimationPlaced(_homePosition, 0.3f);

        }
        public RaycastHit2D Hit()
        {
            var origin = transform.position;
            return Physics2D.Raycast(origin, Vector3.forward, 10, mask);
        }
        
        private void AnimationPlaced(Vector3 target, float duration, MyTiles tile = null)
        {
            if (!reSpawn)
            {
                originPosition = transform.localPosition;
                reSpawn = true;
            }
            transform.DOMove(target, duration).SetEase(Ease.Linear).OnComplete(delegate
            {
               if(tile)
               {
                   BoardManager.Instance.CheckGrid(tile.x, tile.y); 
               }
            });
        }

        public void CheckDespawnAll()
        {
            gameObject.SetActive(false);
            block.CheckDespawnAll();
        }

    }
}