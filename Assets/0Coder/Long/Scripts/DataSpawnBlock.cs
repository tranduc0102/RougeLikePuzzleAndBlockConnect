using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSpawnBlock", menuName = "ScriptableObjects/NewDataSpawnBlock")]
public class DataSpawnBlock : ScriptableObject
{
    public GameObject _aBlock;
    public List<GameObject> slots;
    public int _maxSizeBlock;
    public float distanceBlock;
}