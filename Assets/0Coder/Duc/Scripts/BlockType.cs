using DesignPattern.Obsever;
using UnityEngine;

namespace BlockConnectGame
{
    [RequireComponent(typeof(Movable))]
    public class BlockType : MonoBehaviour, IBlock
    {
        private Type type;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Movable _movable;
        public Movable Movable => _movable;
        private float value;
        public void InitData(DataBlock data)
        {
            type = data.type;
            _spriteRenderer.color = data.color;
            value = data.value;
        }
        public void Use()
        {
           ObserverManager<EventID>.PostEvent(EventID.UpdateStatsPlayer, UpdateValueStats());
        }
        public void SetActiveCollider(bool active)
        {
            _collider.enabled = active;
        }

        private Stats UpdateValueStats()
        {
            Stats stats = new Stats();
            switch (type)
            {
                case Type.HealthPoint:
                    stats.HealthPoint += value;
                    break;
                case Type.Armor:
                    stats.Armor += value;
                    break;
                case Type.MagicalDamage:
                    stats.MagicalDamage += value;
                    break;
                case Type.PhysicalDamage:
                    stats.PhysicalDamage += value;
                    break;
            }

            return stats;
        }
    }
    public enum Type
    {
        HealthPoint,
        PhysicalDamage,
        MagicalDamage,
        Armor
    }
}