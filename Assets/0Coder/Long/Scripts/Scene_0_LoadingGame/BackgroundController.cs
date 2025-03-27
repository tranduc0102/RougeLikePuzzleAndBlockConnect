using System;
using UnityEngine;

[System.Serializable]
public enum SetLayerBackground
{
    layer_0, layer_1, layer_2, layer_3, layer_4, layer_5, layer_6, layer_7, layer_8, layer_9, layer_10, layer_11, layer_12
}
public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float m_maxSpeedMove;
    [SerializeField] private float m_distanceSpeedMove;
    [SerializeField] private float m_curSpeedMove;
    [SerializeField] private SetLayerBackground m_setLayerBackground;
    [SerializeField] private int layerID;
    private void OnEnable()
    {
        layerID = GetNumberOfLayer(m_setLayerBackground.ToString());
        m_curSpeedMove = m_maxSpeedMove - layerID * m_distanceSpeedMove;
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.right * m_curSpeedMove * Time.fixedDeltaTime;
    }

    private int GetNumberOfLayer(string layerName)
    {
        int id = layerName.Length - 1;
        int dem = 1;
        int ans = 0;
        while (id >= 0 && '0' <= layerName[id] && layerName[id] <= '9')
        {
            ans += dem * (layerName[id] - '0');
            dem *= 10;
            -- id;
        }
        return ans;
    }
}
