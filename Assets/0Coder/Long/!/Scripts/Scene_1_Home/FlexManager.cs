using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlexManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private Queue<Tween> m_Tweens = new Queue<Tween>();

    private void Awake()
    {
        m_Tweens.Enqueue(enemy.transform.DOScale(Vector3.zero, 3f).From());
    }

    private void OnDisable()
    {
        while (m_Tweens.Count > 0)
        {
            m_Tweens.Dequeue()?.Kill();
        }
    }
}
