using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextManager : MonoBehaviour
{
    private Queue<Tween> m_Tweens = new Queue<Tween>();
    
    [SerializeField] private RectTransform m_firstName;
    [SerializeField] private RectTransform m_secondName;

    private void Awake()
    {
        FlexText(m_firstName, -500f, new Color {r = 0f, g = 0.8078431f, b = 0.8196079f, a = 1f});
        FlexText(m_secondName, 500f, new Color {r = 0.5764706f, g = 0.4392157f, b = 0.8588235f, a = 1f});
    }

    private void OnDisable()
    {
        while (m_Tweens.Count > 0)
        {
            m_Tweens.Dequeue()?.Kill();
        }
    }

    private void FlexText(RectTransform rect, float duration, Color color)
    {
        TMP_Text text = rect.gameObject.GetComponent<TMP_Text>();
        m_Tweens.Enqueue(rect.DOAnchorPosX(duration, 1.25f, false).From()
            .OnComplete(() =>
            {
                rect.DOShakeScale(0.5f, Vector3.one * 1.25f, 10, 90f, true, ShakeRandomnessMode.Full)
                    .OnComplete(() =>
                    {
                        text.DOFade(0f, 1f)
                            .OnComplete(() =>
                            {
                                text.DOFade(1f, 1f);
                                text.DOBlendableColor(color, 3f);
                            });
                    });
            }));
    }
}
