// Author: Dan_lang_A (DauHang)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DesignPattern.Obsever;
using DesignPattern.ObjectPool;

[CreateAssetMenu(fileName = "DataPlayer", menuName = "ScriptableObjects/New DataPlayer")]
public class DataPlayer : ScriptableObject
{
    [Header("Player Stats")]
    public Stats stats;
    
    [Header("Player on Empty Time Turn")]
    public float distancePlayerRun;
    public float timePlayerRun;
    
}
