// Author: Dan_lang_A (DauHang)

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DesignPattern.Obsever;
using DesignPattern.ObjectPool;
using DG.Tweening;

public class EnvironmentManager : MonoBehaviour
{
    private void OnEnable()
    {
        ObserverManager<EventID>.RegisterEvent(EventID.PlayerMove, param =>
        {
            if (param is (float distancePlayerRun, float timePlayerRun))
            {
                Move(distancePlayerRun, timePlayerRun);
            }
            else
            {
                Debug.LogError($"{this.GetType().Name}: Error registerEvent");
            }
        });
    }
    private void OnDisable()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.PlayerMove, param =>
        {
            if (param is (float distancePlayerRun, float timePlayerRun))
            {
                Move(distancePlayerRun, timePlayerRun);
            }
            else
            {
                Debug.LogError($"{this.GetType().Name}: Error registerEvent");
            }
        });
        DOTween.Kill(transform);
    }

    private void Move(float distancePlayerRun, float timePlayerRun)
    {
        transform.DOMoveX(transform.position.x +distancePlayerRun, timePlayerRun).SetEase(Ease.Linear);
    }
}
