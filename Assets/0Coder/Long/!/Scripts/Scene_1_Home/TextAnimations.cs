using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TextAnimations : MonoBehaviour
{
    private Queue<Tween> m_Tweens = new Queue<Tween>();
    
    [SerializeField] private RectTransform firstName;
    [SerializeField] private RectTransform secondName;

    [SerializeField] private Transform beforeFirstName;
    [SerializeField] private Transform beforeSecondName;
    
    private void Awake()
    {
        NextAnimationText(firstName, beforeFirstName, 2f);
        NextAnimationText(secondName, beforeSecondName, 2f);
    }

    private void NextAnimationText(RectTransform rect, Transform beforePos, float duration)
    {
        m_Tweens.Enqueue(rect.DOMove(beforePos.position, duration, false).From()
            .OnComplete(() =>
            {
                rect.DOShakeScale(1f, Vector3.one * 1.2f, 10, 90f, true, ShakeRandomnessMode.Full)
                    .OnComplete(() =>
                    {
                        rect.DOShakeScale(2f, Vector3.one * 0.1f, 2, 90f, true, ShakeRandomnessMode.Full).SetLoops(-1, LoopType.Yoyo);
                    });
            }));
    }

    private void OnDisable()
    {
        while (m_Tweens.Count > 0)
        {
            m_Tweens.Dequeue()?.Kill();
        }
    }
}
