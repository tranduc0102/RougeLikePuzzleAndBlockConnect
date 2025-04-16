using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlexManager : MonoBehaviour
{
    private Queue<Tween> m_Tweens = new Queue<Tween>();
    
    [Header("----- Auto Select -----")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        enemy = transform.GetChild(0).gameObject;
        player = transform.GetChild(1).gameObject;
        
        m_Tweens.Enqueue(enemy.transform.DOScale(Vector3.zero, 3f).From());
        m_Tweens.Enqueue(player.transform.DOScale(Vector3.zero, 3f).From());
    }

    private void OnDisable()
    {
        while (m_Tweens.Count > 0)
        {
            m_Tweens.Dequeue()?.Kill();
        }
    }
}
