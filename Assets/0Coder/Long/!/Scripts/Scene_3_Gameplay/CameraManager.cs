// Author: Dan_lang_A (DauHang)

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DesignPattern.Obsever;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    private Queue<Tween> m_Tweens = new Queue<Tween>();
    private Action<object> m_PlayerMove;
    
    [SerializeField] private Transform player;
    [SerializeField] private float distance;
    private Vector3 newPosition;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        m_PlayerMove = param =>
        {
            if (param is (float distance, float duration))
            {
                CameraMove(distance, duration);
            }
            else
            {
                Debug.LogError($"{this.GetType().Name}: Error camera move event");
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
    private void CameraMove(float distance, float duration)
    {
        m_Tweens.Enqueue(transform.DOMoveX(transform.position.x + distance, duration).SetEase(Ease.Linear));
    }
}
