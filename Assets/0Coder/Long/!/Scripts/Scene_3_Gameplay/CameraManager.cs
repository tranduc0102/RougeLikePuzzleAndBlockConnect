// Author: Dan_lang_A (DauHang)

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
        if (player != null)
        {
            transform.DOMoveX(transform.position.x + 100f, 4f)
                .SetEase(Ease.Linear);
        }
    }
}
