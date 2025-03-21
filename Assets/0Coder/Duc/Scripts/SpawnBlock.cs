using System.Collections.Generic;
using DesignPattern;
using DesignPattern.ObjectPool;
using UnityEngine;
using Random = UnityEngine.Random;
namespace BlockConnectGame
{
    public class SpawnBlock : Singleton<SpawnBlock>
    {
        [SerializeField] private List<Block> items;
        [SerializeField] private List<Slot> slots;

        private void Start()
        {
            Check();
        }

        public void Check()
        {
            bool canSpawn = true;
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty())
                {
                    canSpawn = false;
                    break;
                }
            }

            if (canSpawn)
            {
                foreach (var slot in slots)
                {
                    var index = Random.Range(0, items.Count);
                    var item = items[index];
                    var block = PoolingManager.Spawn(item, slot.transform.position,Quaternion.identity, slot.transform);
                    block.transform.localPosition = Vector3.zero;
                    block.OnSpawn(slot);
                    slot.SetPiece(block);
                }
            }
        }
    }
}