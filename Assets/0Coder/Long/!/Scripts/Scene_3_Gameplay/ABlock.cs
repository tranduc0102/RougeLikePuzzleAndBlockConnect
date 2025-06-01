using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern.Obsever;
using DG.Tweening;
using Duc;
using TMPro;
using Random = UnityEngine.Random;

public class ABlock : MonoBehaviour
{
    private Queue<Tween> m_Tweens = new Queue<Tween>();
    private Action<object> m_MoveToDefault;
    private Action<object> m_BlockIsUse;
    private Action<object> m_BlockAddStats;
    
    [SerializeField] private DataAllBlock dataAllBlock;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private TypeBlock typeBlock;
    private float moveSpeed;
    private Vector3 excess;
    private Vector3 posDefault;
    private bool isSetDefault;
    private bool isUse;
    private float valueBlock;
    private Stats statsBlock;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = dataAllBlock.moveSpeed;
        statsBlock = new Stats();
        int idType = Random.Range(0, Enum.GetValues(typeof(TypeBlock)).Length);
        switch (idType)
        {
            case 0:
                InitBlock(idType);
                statsBlock.HealthPoint = valueBlock;
                break;
            case 1:
                InitBlock(idType);
                statsBlock.Armor = valueBlock;
                break;
            case 2:
                InitBlock(idType);
                statsBlock.PhysicalDamage = valueBlock;
                break;
            case 3:
                InitBlock(idType);
                statsBlock.MagicalDamage = valueBlock;
                break;
            default:
                InitBlock(idType);
                break;
        }
        isSetDefault = false;
        isUse = false;
    }

    private void InitBlock(int id)
    {
        typeBlock = dataAllBlock.blocks[id].typeBlock;
        spriteRenderer.sprite = dataAllBlock.blocks[id].sprite;
        valueBlock = dataAllBlock.blocks[id].valueBlock;
    }

    private void OnEnable()
    {
        m_MoveToDefault = param => MoveToDefault();
        m_BlockIsUse = param => IsUse((Transform)param);
        m_BlockAddStats = param => AddStatsPlayer((Transform)param);
        
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.moveToDefault, m_MoveToDefault);
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.blockIsUse, m_BlockIsUse);
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.blockAddStats, m_BlockAddStats);
    }

    private void OnDisable()
    {
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.moveToDefault, m_MoveToDefault);
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.blockIsUse, m_BlockIsUse);
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.blockAddStats, m_BlockAddStats);

        while (m_Tweens.Count > 0)
        {
            m_Tweens.Dequeue()?.Kill();
        }
    }

    private void AddStatsPlayer(Transform target)
    {
        if (target == transform)
        {
            ObserverManager<EventID>.PostEvent(EventID.UpdateStatsPlayer, statsBlock);
        }
    }
    
    private void IsUse(Transform target)
    {
        if (transform.parent == target)
        {
            isUse = true;
            ObserverManager<Gameplay>.RemoveEvent(Gameplay.moveToDefault, param => MoveToDefault());
        }
    }

    private void OnMouseDown()
    {
        if (/*GameManager.Instance._GameTurn == GameTurn.PlayerTurn &&*/ isUse == false && GameManager.Instance.AmountMovementBlock > 0 && GameManager.Instance.IsTurnPlayer)
        {
            if (isSetDefault == false)
            {
                isSetDefault = true;
                posDefault = transform.parent.position;
                posDefault.z = -0.5f;
            }
            excess = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;
        }
    }

    private void OnMouseDrag()
    {
        if (/*GameManager.Instance._GameTurn == GameTurn.PlayerTurn &&*/ isUse == false && GameManager.Instance.AmountMovementBlock > 0 && GameManager.Instance.IsTurnPlayer)
        {
            Move(Input.mousePosition);
        }
    }
    private void OnMouseUp()
    {
        if (isUse == false)
        {
            ObserverManager<Gameplay>.PostEvent(Gameplay.checkInsertBlock, transform.parent);
        }
    }

    private void MoveToDefault()
    {
        if (isUse == false)
        {
            if (isSetDefault)
            {
                m_Tweens.Enqueue(transform.parent.DOMove(posDefault, moveSpeed * 6));
            }
        }
    }

    private void Move(Vector3 target)
    {
        target = Camera.main.ScreenToWorldPoint(target);
        target -= excess;
        target.z = -1f;
        m_Tweens.Enqueue(transform.parent.DOMove(target, moveSpeed));
    }
    
}