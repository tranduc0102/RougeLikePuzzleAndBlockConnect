// Author: Dan_lang_A (DauHang)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DesignPattern.Obsever;
using DesignPattern.ObjectPool;

[CreateAssetMenu(fileName = "Player Data", menuName = "ScriptableObjects/New Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Player Stats")]
    public Stats stats;
    public float timeSpawn;
    public float timeDespawn;
    public GameObject teleport;
    
    [Header("Player on Empty Time Turn")]
    public float distancePlayerRun;
    public float timePlayerRun;
    
}