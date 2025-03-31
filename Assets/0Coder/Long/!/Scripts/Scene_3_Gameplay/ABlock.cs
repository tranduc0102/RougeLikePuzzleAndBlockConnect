using System;
using UnityEngine;
using DesignPattern.Obsever;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

public class ABlock : MonoBehaviour
{
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
        spriteRenderer.color = dataAllBlock.blocks[id].colorBlock;
        valueBlock = dataAllBlock.blocks[id].valueBlock;
    }

    private void OnEnable()
    {
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.moveToDefault, param => MoveToDefault());
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.blockIsUse, param => IsUse((Transform) param));
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.blockAddStats, param => AddStatsPlayer((Transform) param));
    }

    private void OnDisable()
    {
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.moveToDefault, param => MoveToDefault());
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.blockIsUse, param => IsUse((Transform) param));
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.blockAddStats, param => AddStatsPlayer((Transform) param));
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
        if (isUse == false)
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
        if (GameManager.Instance._GameTurn == GameTurn.PlayerTurn)
        {
            if (isUse == false)
            {
                Move(Input.mousePosition);
            }
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
                transform.parent.DOMove(posDefault, moveSpeed * 6);
            }
        }
    }

    private void Move(Vector3 target)
    {
        target = Camera.main.ScreenToWorldPoint(target);
        target -= excess;
        target.z = -1f;
        transform.parent.DOMove(target, moveSpeed);
    }
}