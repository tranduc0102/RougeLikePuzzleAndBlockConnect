using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Block Data", menuName = "ScriptableObjects/New Spawn Block Data")]
public class SpawnBlockData : ScriptableObject
{
    public GameObject aBlock;
    public int maxSizeBlock;
    public float distanceBlock;
}