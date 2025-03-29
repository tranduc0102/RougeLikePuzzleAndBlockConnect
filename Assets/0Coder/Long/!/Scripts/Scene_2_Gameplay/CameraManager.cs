// Author: Dan_lang_A (DauHang)

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distance;
    private Vector3 newPosition;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            newPosition = transform.position;
            newPosition.x = player.transform.position.x + distance;
            transform.position = newPosition;
        }
    }
}
