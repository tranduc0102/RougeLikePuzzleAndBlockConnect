using UnityEngine;

namespace BlockConnectGame
{
    public class MyTiles : MonoBehaviour
    {
        public int x, y;
        public bool IsEmpty;
        public BlockType Child;

        public void SetXY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}