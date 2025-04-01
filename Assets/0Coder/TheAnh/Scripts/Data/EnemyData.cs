
using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Enemy Data", fileName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public List<EnemyParam> Enemies;
    public float PositionRight;
    public float PositionUp;
    public GameObject teleport;
    public float timeSpawn;
    public float timeDespawn;
    public GameObject objEnemyDie;
    public float timeDelayAttackPlayer;
    public float distaceAttackPlayer;
}

[Serializable]
public class EnemyParam
{
    public GameObject EnemyPrefab;
    public string EnemyName;
    public float HealthPoint;
    public float PhysicalDamage;
    public float MagicalDamage;
    public float Armor;
}
