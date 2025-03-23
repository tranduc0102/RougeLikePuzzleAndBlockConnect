using System.Collections.Generic;
using UnityEngine;

public enum TypeBlock
{
    health,
    shield,
    physicDamage,
    magicDamage
}
[System.Serializable]
public struct DataBlock
{
    public TypeBlock typeBlock;
    public Color colorBlock;
    public float valueBlock;
}

[CreateAssetMenu(fileName = "DataAllBlock", menuName = "ScriptableObjects/DataAllBlock")]
public class DataAllBlock : ScriptableObject
{
    public List<DataBlock> blocks;
    public float moveSpeed;
}