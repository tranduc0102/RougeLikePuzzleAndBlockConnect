using UnityEngine;

namespace BlockConnectGame
{
    public class Slot : MonoBehaviour
    {
        private Block _piece;

        public bool IsEmpty() => _piece == null;
    
        public void Release()
        {
            _piece = null;
        }

        public void SetPiece(Block current)
        {
            _piece = current;
        }
    }
}