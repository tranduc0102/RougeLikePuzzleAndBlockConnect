using System;
using System.Collections;
using UnityEngine;
[Serializable]
public struct Stats
{
    public float HealthPoint;
    public float PhysicalDamage;
    public float MagicalDamage;
    public float Armor;

    public Stats(float healthPoint, float physicalDamage, float magicalDamage, float armor)
    {
        HealthPoint = healthPoint;
        PhysicalDamage = physicalDamage;
        MagicalDamage = magicalDamage;
        Armor = armor;
    }
}
public abstract class ActorStats : MonoBehaviour
{
    [SerializeField] protected Stats m_ActorStats;
    [SerializeField] protected float timeDespawn;
    
    
    public void AddStats(Stats stats)
    {
        m_ActorStats.HealthPoint += stats.HealthPoint;
        m_ActorStats.PhysicalDamage += stats.PhysicalDamage;
        m_ActorStats.MagicalDamage += stats.MagicalDamage;
        m_ActorStats.Armor += stats.Armor;
        
        if(m_ActorStats.HealthPoint <= 0) StartCoroutine(HandleDead());
    }

    protected abstract IEnumerator HandleDead();

}
