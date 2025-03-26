using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Level Data", fileName = "Level Data")]
public class LevelData : ScriptableObject
{
    public List<LevelParam> Levels;
}

[Serializable]
public class LevelParam
{
    public List<Way> Ways;
}
[Serializable]
public class Way
{
    public List<EnemyInfor> Enemies;
}
[Serializable]
public class EnemyInfor
{
    [Header("Id")]
    public int EnemyId;
    [Header("Position")]
    public Vector3 EnemyPosition;
}
