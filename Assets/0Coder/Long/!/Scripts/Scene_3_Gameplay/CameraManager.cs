// Author: Dan_lang_A (DauHang)

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DesignPattern.Obsever;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distance;
    private Vector3 newPosition;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        ObserverManager<EventID>.RegisterEvent(EventID.PlayerMove, param =>
        {
            if (param is (float distance, float duration))
            {
                CameraMove(distance, duration);
            }
            else
            {
                Debug.LogError($"{this.GetType().Name}: Error camera move event");
            }
        });
        
    }
    private void OnDisable()
    {
        ObserverManager<EventID>.RemoveEvent(EventID.PlayerMove, param =>
        {
            if (param is (float distance, float duration))
            {
                CameraMove(distance, duration);
            }
            else
            {
                Debug.LogError($"{this.GetType().Name}: Error camera move event");
            }
        });
        DOTween.Kill(transform);
    }
    private void CameraMove(float distance, float duration)
    {
        transform.DOMoveX(transform.position.x + distance, duration).SetEase(Ease.Linear);
    }
}
