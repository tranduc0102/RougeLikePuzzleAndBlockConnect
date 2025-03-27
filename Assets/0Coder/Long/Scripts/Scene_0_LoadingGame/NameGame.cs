using System;
using UnityEngine;
using DG.Tweening;

public class NameGame : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 curPosition;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        curPosition = rectTransform.position;
    }

    private void OnEnable()
    {
        Vector3 pos = curPosition;
        pos.z = 1000;
        pos.x += 200;
        pos.y += 200;
        rectTransform.position = pos;
        rectTransform.localScale = Vector3.one * 1.5f;
        rectTransform.DOJump(curPosition, 10, 3, 3f)
            .OnComplete(() =>
            {
                rectTransform.DOScale(Vector3.one, 1f)
                    .OnComplete(() =>
                    {
                        rectTransform.DOShakeScale(0.5f, Vector3.one * 1.5f, 10, 90f, true, ShakeRandomnessMode.Full);
                    });
            });
    }
}
