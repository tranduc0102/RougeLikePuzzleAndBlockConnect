using System;
using System.Collections.Generic;
using DesignPattern;
using DesignPattern.ObjectPool;
using DesignPattern.Obsever;
using UnityEngine;
using Random = UnityEngine.Random;
public struct BlockStats
{
    public GameObject block;
    public int height;
    public int width;
    public int index;

    public static bool operator ==(BlockStats a, BlockStats b)
    {
        return a.height == b.height && a.width == b.width && a.index == b.index;
    }

    public static bool operator !=(BlockStats a, BlockStats b)
    {
        return !(a == b);
    }
}
public class SpawnBlock : Singleton<SpawnBlock>
{
    public DataSpawnBlock dataSpawnBlock;
    
    private GameObject _aBlock; 
    private List<GameObject> slots;
    private int _maxSizeBlock;
    private float distanceBlock;
    private int countBlock;
    
    private void OnEnable()
    {
        _aBlock = dataSpawnBlock._aBlock;
        slots = dataSpawnBlock.slots;
        _maxSizeBlock = dataSpawnBlock._maxSizeBlock;
        distanceBlock = dataSpawnBlock.distanceBlock;
        countBlock = slots.Count;
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.spawnBlock, param => StartSpawnBlock());
    }

    private void OnDisable()
    {
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.spawnBlock, param => StartSpawnBlock());
    }

    private void Start()
    {
        SpawnerBlock(countBlock);
    }

    private void StartSpawnBlock()
    {
        SpawnerBlock(countBlock);
    }
    
    protected void SpawnerBlock(int n)
    {
        List<BlockStats> blocks = new List<BlockStats> {GetBlockStats()};
        CreateBlock(blocks[0], 0);
        for (int i = 1; i < n; ++i)
        {
            blocks.Add(GetBlockStats());
            while (checkBlock(blocks) == false)
            {
                blocks[i] = GetBlockStats();
            }
            CreateBlock(blocks[i], i);
        }
        // TODO: Log Spawn Block
        // Debug.LogWarning("SpawnBlock finished");
        // foreach (var block in blocks)
        // {
        //     Debug.Log($"{block.height}, {block.width}, {block.index}");
        // }
    }

    protected bool checkBlock(List<BlockStats> blockStats)
    {
        int n = blockStats.Count;
        for (int i = 0; i < n - 1; ++i)
        {
            if (blockStats[n - 1] == blockStats[i])
            {
                return false;
            }
        }
        return true;
    }

    protected void CreateBlock(BlockStats blockStats, int? idPos = null)
    {
        GameObject block = new GameObject() {name = "Block"};
        block.transform.SetParent(transform);
        if (blockStats.block == null)
        {
            for (int i = 0; i < blockStats.height; ++i)
            {
                GameObject newBlock = Instantiate(_aBlock, block.transform);
                SetTypeBlock(newBlock);
                newBlock.transform.position += Vector3.up * i * distanceBlock;
            }
            if (blockStats.index == 0)
            {
                for (int j = 1; j < blockStats.width; ++j)
                {
                    for (int i = 0; i < blockStats.height; ++i)
                    {
                        GameObject newBlock = Instantiate(_aBlock, block.transform);
                        SetTypeBlock(newBlock);
                        newBlock.transform.position += Vector3.up * i * distanceBlock + Vector3.right * j * distanceBlock;
                    }
                }
            }
            else
            {
                int countBlockLeft = blockStats.height >= blockStats.width ? (blockStats.index > 0 ? 0 : blockStats.width - 1) : Math.Abs(blockStats.index) - 1;
                int countBlockRight = blockStats.width - countBlockLeft - 1;
                for (int i = 0; i < countBlockLeft; ++i)
                {
                    GameObject newBlock = Instantiate(_aBlock, block.transform);
                    SetTypeBlock(newBlock);
                    newBlock.transform.position += Vector3.left * (i + 1) * distanceBlock;
                }

                for (int i = 0; i < countBlockRight; ++i)
                {
                    GameObject newBlock = Instantiate(_aBlock, block.transform);
                    SetTypeBlock(newBlock);
                    newBlock.transform.position += Vector3.right * (i + 1) * distanceBlock;
                }
            }
            ObserverManager<Gameplay>.PostEvent(Gameplay.addBlockDontUse, block.transform);
        }
        // change position child
        for (int i = 0; i < block.transform.childCount; ++i)
        {
            block.transform.GetChild(i).position += distanceBlock * 0.5f * (Vector3.up * (blockStats.height - 1) * -1 + Vector3.right * (blockStats.height >= blockStats.width ? (blockStats.index >= 0 ? -1 : 1) * (blockStats.width - 1) : (Math.Abs(blockStats.index) - 0.5f - blockStats.width / 2f) * 2f));
        }
        if (idPos != null)
        {
            block.transform.position = slots[(int) idPos].transform.position;
        }
        Vector3 newPos = block.transform.position;
        newPos.z = -0.5f;
        block.transform.position = newPos;
    }

    protected void SetTypeBlock(GameObject block)
    {
        // TODO: set type block?
    }
    protected int GetRandom(int _maxID, object check = null)
    {
        return Random.Range(check != null ? - _maxID + 1 : 0, _maxID);
    }

    protected BlockStats GetBlockStats()
    {
        BlockStats newBlockStats = new BlockStats();
        newBlockStats.height = GetRandom(_maxSizeBlock) + 1;
        newBlockStats.width = GetRandom(_maxSizeBlock) + 1;
        newBlockStats.index = newBlockStats.height == 1 || newBlockStats.width == 1 ? 1 : GetRandom(1 + (newBlockStats.height >= newBlockStats.width ? newBlockStats.height : newBlockStats.width), -1);
        return newBlockStats;
    }
}
