// Author: Dan_lang_A (DauHang)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DesignPattern.Obsever;
using DesignPattern.ObjectPool;

[CreateAssetMenu(fileName = "Board Data", menuName = "ScriptableObjects/New Board Data")]
public class BoardData : ScriptableObject
{
    public int rowNumber;
    public int columnNumber;
    public float distanceBlock;
    public GameObject cellPrefab;
    public LayerMask layerMask;
}
