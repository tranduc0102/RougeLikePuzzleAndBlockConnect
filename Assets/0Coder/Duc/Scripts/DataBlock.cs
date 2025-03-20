using UnityEngine;

namespace BlockConnectGame
{
    [CreateAssetMenu(menuName = "Data/Block", fileName = "DataBlock")]
    public class DataBlock : ScriptableObject
    {
        public Type type;
        public float value;
        public Color color;
    }
}