using System;
using UnityEngine;

[System.Serializable]
public enum LayerID
{
    layer_0, layer_1, layer_2, layer_3, layer_4, layer_5, layer_6, layer_7, layer_8, layer_9, layer_10, layer_11, layer_12
}

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private LayerID m_layerID;
    [SerializeField] private long m_ID;
    [SerializeField] private float m_maxSpeedMove;
    [SerializeField] private float m_curSpeedMove; 
    [SerializeField] private float m_distanceMoveSpeed;

    private void Awake()
    {
        GetLayerID(m_layerID.ToString());
        m_curSpeedMove = m_maxSpeedMove - m_ID * m_distanceMoveSpeed;
    }

    private void FixedUpdate()
    {
        transform.position -= Vector3.right * m_curSpeedMove * Time.fixedDeltaTime;
    }

    private void GetLayerID(string layerName)
    {
        m_ID = 0;
        long dem = 1;
        int id = layerName.Length - 1;
        while (0 <= id && '0' <= layerName[id] && layerName[id] <= '9')
        {
            m_ID += dem * (layerName[id] - '0');
            dem *= 10;
            --id;
        }
    }
}
