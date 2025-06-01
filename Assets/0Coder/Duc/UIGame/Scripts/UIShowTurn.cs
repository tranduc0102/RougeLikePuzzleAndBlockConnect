using DG.Tweening;
using Duc;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShowTurn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextMeshProUGUI;
    [SerializeField] private Transform transform;
    public void ShowTurn(bool isTurnPlayer)
    {
        if (isTurnPlayer)
        {
            TextMeshProUGUI.text = "Your Turn";
        }
        else
        {
            TextMeshProUGUI.text = "Enemy Turn";
        }
        transform.DOScaleX(2f, 1f).SetEase(Ease.Linear).OnComplete(delegate
        {
            DOVirtual.DelayedCall(0.5f, delegate
            {
                transform.DOScaleX(0f, 1f).SetEase(Ease.Linear).OnComplete(delegate
                {
                    if (isTurnPlayer)
                    {
                        GameManager.Instance.IsTurnPlayer = true;
                    }
                    else
                    {
                        GameManager.Instance.EnemyTurn();
                    }
                });
            });
        });
    }
}
