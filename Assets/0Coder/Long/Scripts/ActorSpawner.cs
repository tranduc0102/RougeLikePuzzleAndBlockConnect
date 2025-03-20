using System;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct BlockStats
{
    public int lenght;
    public int width;
}

[System.Serializable]
public struct BlockType
{
    public string hcnNgang;
    public string hcnDoc;
    public string hinhVuong;
}
public class ActorSpawner : MonoBehaviour
{
    private void Start()
    {
        SpawnerBlock();
    }

    protected void SpawnerEnemy()
    {
        Debug.Log("Spawner Enemy");
    }

    protected void SpawnerBlock()
    {
        BlockStats blockStats = new BlockStats();
        int id = 0;
        for (int i = 0; i < 3; ++ i)
        {
            switch (i)
            {
                case 0:
                    id = Random.Range(0, 3);
                    blockStats = GetBlockStats(id);
                    break;
                case 1:
                    int new_id = Random.Range(0, 3);
                    while (new_id == id)
                    {
                        new_id = Random.Range(0, 3);
                    }
                    id += new_id;
                    blockStats = GetBlockStats(new_id);
                    break;
                case 2:
                    blockStats = GetBlockStats(3 - id);
                    break;
                default:
                    Debug.Log("Error Spawner Block: " + gameObject.name);
                    return;
                    break;
            }
            Debug.Log("Spawer Block Infor id = " + (i + 1) + " have: " + blockStats.lenght + " : " +  blockStats.width);
        }
    }

    protected BlockStats GetBlockStats(int id)
    {
        BlockStats blockStats = new BlockStats();
        blockStats.lenght = Random.Range(0, 5) + 1;
        blockStats.width = blockStats.lenght;
        switch (id)
        {
            case 0:
            case 1:
                while (blockStats.width == blockStats.lenght)
                {
                    blockStats.width = Random.Range(0, 5) + 1;
                }
                if (id == 1 && blockStats.width > blockStats.lenght)
                {
                    (blockStats.width, blockStats.lenght) = (blockStats.lenght, blockStats.width);
                }
                break;
            case 2:
                break;
            default:
                Debug.Log("Error spawner block " + gameObject.name);
                blockStats.lenght = -1;
                blockStats.width = -1;
                break;
        }
        return blockStats;
    }
}