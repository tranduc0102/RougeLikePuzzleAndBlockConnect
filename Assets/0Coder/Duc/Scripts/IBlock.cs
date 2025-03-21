using UnityEngine;

namespace BlockConnectGame
{
    interface IBlock
    {
        public void InitData(DataBlock data);
        public void Use();
    }
}
