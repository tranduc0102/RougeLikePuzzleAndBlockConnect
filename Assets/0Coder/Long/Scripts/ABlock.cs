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
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = dataAllBlock.moveSpeed;
        int idType = Random.Range(0, Enum.GetValues(typeof(TypeBlock)).Length);
        switch (idType)
        {
            case 0:
                InitBlock(idType);
                break;
            case 1:
                InitBlock(idType);
                break;
            case 2:
                InitBlock(idType);
                break;
            case 3:
                InitBlock(idType);
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
    }

    private void OnEnable()
    {
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.moveToDefault, param => MoveToDefault());
        ObserverManager<Gameplay>.RegisterEvent(Gameplay.blockIsUse, param => IsUse((Transform) param));
    }

    private void OnDisable()
    {
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.moveToDefault, param => MoveToDefault());
        ObserverManager<Gameplay>.RemoveEvent(Gameplay.blockIsUse, param => IsUse((Transform) param));
    }

    private void IsUse(Transform target)
    {
        if (transform.parent == target)
        {
            ObserverManager<Gameplay>.RemoveEvent(Gameplay.moveToDefault, param => MoveToDefault());
            isUse = true;
            transform.parent.position += Vector3.back * 0.5f;
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
                posDefault.z = 0;
            }
            excess = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;
            Vector3 newPos = transform.parent.position;
            newPos.z = 0;
            transform.parent.position = newPos;
        }
    }

    private void OnMouseDrag()
    {
        if (isUse == false)
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
                transform.parent.DOMove(posDefault, moveSpeed * 6);
            }
        }
    }

    private void Move(Vector3 target)
    {
        target = Camera.main.ScreenToWorldPoint(target);
        target -= excess;
        target.z = 0f;
        transform.parent.DOMove(target, moveSpeed);
    }
}