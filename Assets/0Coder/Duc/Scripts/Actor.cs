using DesignPattern.Obsever;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
namespace Duc
{
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
    public abstract class Actor : MonoBehaviour
    {
        [Header("----- Auto set up data -----")]
        [SerializeField] protected Stats m_ActorStats;
        [SerializeField] protected float timeSpawn;
        [SerializeField] protected float timeDespawn;
        [SerializeField] protected float distanceActorRun;
        [SerializeField] private SpriteRenderer m_SpriteRenderer;
        protected Actor target;
        [SerializeField] protected Animator animator;
        private Sequence sequence;
        public bool Alive {  get; protected set; }


        protected virtual void AddStats(Stats stats)
        {
            m_ActorStats.HealthPoint += stats.HealthPoint;
            m_ActorStats.PhysicalDamage += stats.PhysicalDamage;
            m_ActorStats.MagicalDamage += stats.MagicalDamage;
            m_ActorStats.Armor += stats.Armor;
        }
        protected virtual void Destroy()
        {
            sequence?.Kill(true);
            DOTween.Kill(this);
        }
        protected abstract void HandleDead();
        public abstract void TakeTurn(bool isMyTurn, Action actionFinish);

        protected virtual void ProcessTurn(Action actionFinish)
        {
            if (!target.Alive) return;
            sequence?.Kill();

            sequence = DOTween.Sequence();

            Vector3 startPos = transform.position;
            Vector3 attackPos = target.transform.position + Vector3.right * distanceActorRun;
            if(m_SpriteRenderer == null)
            {
                m_SpriteRenderer = GetComponent<SpriteRenderer>();
            }

            sequence.AppendCallback(() =>
            {
                animator.SetBool("Run", true);
                transform.DOMove(attackPos, 1f).OnComplete(delegate
                {
                    animator.SetBool("Run", false);
                });
            })
                .AppendInterval(0.3f)
                .AppendCallback(() => Attack())
                .AppendInterval(2f)
                .AppendCallback(delegate
                {
                    m_SpriteRenderer.flipX = true;
                    animator.SetBool("Run", true);
                    transform.DOMove(startPos, 1f).OnComplete(delegate
                    {
                        m_SpriteRenderer.flipX = false;
                        animator.SetBool("Run", false);
                    });
                })
                .OnComplete(delegate {
                    DOVirtual.DelayedCall(0.5f, delegate
                    {
                        actionFinish?.Invoke();
                    });
                });
        }
        float damage = 0;
        protected virtual void Attack()
        {
            animator.SetTrigger("Attack");
            damage = target.m_ActorStats.MagicalDamage + target.m_ActorStats.PhysicalDamage;
        }
        public virtual void TargetReceiverDamage()
        {
            target.ReceiveDamaged();
        }
        public void PlayFXAttack()
        {
            AudioManager.PlaySFX(SoundType.FXAttack);
        }
        public virtual void ReceiveDamaged()
        {
            animator.SetTrigger("GetHit");
            AudioManager.PlaySFX(SoundType.FXHit);
            if (damage >= m_ActorStats.Armor) {
                float tmpDamage = damage - m_ActorStats.Armor;
                if (m_ActorStats.Armor > 0) {
                    m_ActorStats.Armor -= damage;
                    if(m_ActorStats.Armor < 0)
                    {
                        m_ActorStats.Armor = 0;
                    }
                }
                m_ActorStats.HealthPoint -= tmpDamage;
            }
            else
            {
                m_ActorStats.Armor -= damage;
                if (m_ActorStats.Armor < 0)
                {
                    m_ActorStats.Armor = 0;
                }
            }
            if (m_ActorStats.HealthPoint <= 0)
            {
                m_ActorStats.HealthPoint = 0;
                Alive = false;
                HandleDead();
            }
        }
        public void SetTarget(Actor target)
        {
            this.target = target;
        }
    }
}