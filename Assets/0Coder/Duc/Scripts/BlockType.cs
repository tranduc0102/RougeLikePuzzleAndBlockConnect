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
           //Post Event to Actor 
           ObserverManager<Type>.PostEvent(type, value);
        }
        public void SetActiveCollider(bool active)
        {
            _collider.enabled = active;
        }
    }
    public enum Type
    {
        HealthPoint,
        AttackDamage,
        PhepDamage,
        Giap
    }
}