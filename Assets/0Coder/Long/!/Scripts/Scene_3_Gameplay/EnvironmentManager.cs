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
    private Queue<Tween> m_Tweens = new Queue<Tween>();
    private Action<object> m_PlayerMove;
    
    private void OnEnable()
    {
        m_PlayerMove = param =>
        {
            if (param is (float distancePlayerRun, float timePlayerRun))
            {
                Move(distancePlayerRun, timePlayerRun);
            }
            else
            {
                Debug.LogError($"{this.GetType().Name}: Error registerEvent");
            }
        };
        
        ObserverManager<EventID>.RegisterEvent(EventID.PlayerMove, m_PlayerMove);
    }
    private void OnDisable()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.PlayerMove, m_PlayerMove);
        
        while (m_Tweens.Count > 0)
        {
            m_Tweens.Dequeue()?.Kill();
        }
    }

    private void Move(float distancePlayerRun, float timePlayerRun)
    {
        m_Tweens.Enqueue(transform.DOMoveX(transform.position.x +distancePlayerRun, timePlayerRun).SetEase(Ease.Linear));
    }
}
