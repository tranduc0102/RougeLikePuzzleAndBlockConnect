using UnityEngine;
using DesignPattern.Obsever;
using UnityEditor.Experimental.GraphView;

public abstract class ActorAttack : MonoBehaviour
{
    protected abstract void SendDamage(Stats stats);
}
