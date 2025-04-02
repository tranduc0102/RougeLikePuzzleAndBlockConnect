using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ChangeBackground : MonoBehaviour
{
    [SerializeField] private List<RectTransform> oldBackground;
    [SerializeField] private List<RectTransform> newBackground;
    [SerializeField] private float duration;
    [SerializeField] private float distance;
    private Tween m_tween;
    private void Awake()
    {
        foreach (RectTransform rect in oldBackground)
        {
            m_tween = rect.DOAnchorPosX(-distance, duration);
        }
        foreach (RectTransform rect in newBackground)
        {
            m_tween = rect.DOAnchorPosX(distance, duration).From();
        }
    }

    private void OnDisable()
    {
        m_tween?.Kill();
    }
}
