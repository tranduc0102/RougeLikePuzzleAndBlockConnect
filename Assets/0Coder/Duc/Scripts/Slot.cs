using UnityEngine;

namespace BlockConnectGame
{
    public class Slot : MonoBehaviour
    {
        private Block _block;

        public bool IsEmpty() => _block == null;

        public void Release()
        {
            _block = null;
        }

        public void SetBlock(Block current)
        {
            _block = current;
        }

        public Block GetBlock()
        {
            return _block;
        }
    }
}