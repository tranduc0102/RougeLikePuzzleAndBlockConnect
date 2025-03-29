using System;
using UnityEngine;
using DG.Tweening;

public class FlexManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    private void Awake()
    {
        enemy.transform.DOScale(Vector3.zero, 3f).From();
    }
}
