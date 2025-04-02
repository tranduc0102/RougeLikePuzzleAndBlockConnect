using System;
using DG.Tweening;
using UnityEngine;

public class TextAnimations : MonoBehaviour
{
    [SerializeField] private RectTransform firstName;
    [SerializeField] private RectTransform secondName;

    [SerializeField] private Transform beforeFirstName;
    [SerializeField] private Transform beforeSecondName;
    
    private Tween m_tween;
    private void Awake()
    {
        NextAnimationText(firstName, beforeFirstName, 2f);
        NextAnimationText(secondName, beforeSecondName, 2f);
    }

    private void NextAnimationText(RectTransform rect, Transform beforePos, float duration)
    {
        m_tween = rect.DOMove(beforePos.position, duration, false).From()
            .OnComplete(() =>
            {
                rect.DOShakeScale(1f, Vector3.one * 1.2f, 10, 90f, true, ShakeRandomnessMode.Full)
                    .OnComplete(() =>
                    {
                        rect.DOShakeScale(2f, Vector3.one * 0.1f, 2, 90f, true, ShakeRandomnessMode.Full).SetLoops(-1, LoopType.Yoyo);
                    });
            });
    }

    private void OnDisable()
    {
        m_tween?.Kill();
    }
}
