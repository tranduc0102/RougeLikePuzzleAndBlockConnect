using System;
using UnityEngine;
using DG.Tweening;

public class FlexManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private Tween m_tween;

    private void Awake()
    {
        m_tween = enemy.transform.DOScale(Vector3.zero, 3f).From();
    }

    private void OnDisable()
    {
        m_tween?.Kill();
    }
}
