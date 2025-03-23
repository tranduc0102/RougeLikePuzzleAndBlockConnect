using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Collider2D _collider2D;
    private bool isEmpty;

    private void OnEnable()
    {
        isEmpty = true;
    }

    public void SetPosition(float x, float y)
    {
        transform.position = Vector2.right * x + Vector2.up * y;
    }
    
    public void SetActiveCollider2D(bool active)
    {
        _collider2D.enabled = active;
    }

    public bool IsEmpty()
    {
        return isEmpty;
    }

    public void SetIsEmpty(bool isEmpty)
    {
        this.isEmpty = isEmpty;
    }
}