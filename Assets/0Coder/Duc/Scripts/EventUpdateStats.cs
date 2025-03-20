using System;
using DesignPattern.Obsever;
using UnityEngine;
namespace BlockConnectGame
{
    public class TestUpdateStats:MonoBehaviour
    {
        public Stats stats;
        private void OnEnable()
        {
            ObserverManager<Type>.RegisterEvent(Type.Giap, param => UpdateStats(Type.Giap, (float)param));
            ObserverManager<Type>.RegisterEvent(Type.AttackDamage, param => UpdateStats(Type.AttackDamage, (float)param));
            ObserverManager<Type>.RegisterEvent(Type.HealthPoint, param => UpdateStats(Type.HealthPoint, (float)param));
            ObserverManager<Type>.RegisterEvent(Type.PhepDamage, param => UpdateStats(Type.PhepDamage, (float)param));
        }

        private void UpdateStats(Type type, float value)
        {
            switch (type)
            {
                case Type.AttackDamage:
                    stats.damage += value;
                    break;
                case Type.PhepDamage:
                    stats.magicDamage += value;
                    break;
                case Type.Giap:
                    stats.Giap += value;
                    break;
                case Type.HealthPoint:
                    stats.healthPoint += value;
                    break;
            }
        }
        [Serializable]
        public struct Stats
        {
            public float damage;
            public float magicDamage;
            public float Giap;
            public float healthPoint;
        }
    }
}