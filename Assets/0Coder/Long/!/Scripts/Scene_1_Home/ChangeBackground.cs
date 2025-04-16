using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ChangeBackground : MonoBehaviour
{
    private Queue<Tween> m_Tweens = new Queue<Tween>();
    
    [SerializeField] private List<RectTransform> oldBackground;
    [SerializeField] private List<RectTransform> newBackground;
    [SerializeField] private float duration;
    [SerializeField] private float distance;
    
    private void Awake()
    {
        foreach (RectTransform rect in oldBackground)
        {
            m_Tweens.Enqueue(rect.DOAnchorPosX(-distance, duration));
        }
        foreach (RectTransform rect in newBackground)
        {
            m_Tweens.Enqueue(rect.DOAnchorPosX(distance, duration).From());
        }
    }

    private void OnDisable()
    {
        while (m_Tweens.Count > 0)
        {
            m_Tweens.Dequeue()?.Kill();
        }
    }
}
