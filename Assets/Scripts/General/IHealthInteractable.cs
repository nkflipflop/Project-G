using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace General
{
    public interface IHealthInteractable
    {
        int CurrentHealth { get; set; }
        int MaxHealth { get; set; }
        bool IsDead => CurrentHealth <= 0;
        Dissolve DissolveEffect { get; set; }
        SpriteRenderer HealthEffectRenderer { get; set; }
        CapsuleCollider2D HitBoxCollider { get; set; }
        SoundManager.Sound HitSound { get; set; }

        void TakeDamage(int amount)
        {
            CurrentHealth -= amount;

            if (IsDead)
            {
                CurrentHealth = 0;
                DissolveEffect.IsDissolving = true;
                HitBoxCollider.enabled = false;
            }

            _ = DamageEffect();
            SoundManager.PlaySound(HitSound, ((MonoBehaviour)this).transform.position);
        }

        bool GainHealth(int amount)
        {
            if (CurrentHealth == MaxHealth)
            {
                return false;
            }
            
            CurrentHealth += amount;

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        
            _ = HealEffect();
            return true;
        }
        
        async UniTaskVoid DamageEffect()
        {
            HealthEffectRenderer.color = new Color32(255, 55, 0, 255);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            HealthEffectRenderer.color = Color.white;
        }
    
        async UniTaskVoid HealEffect()
        {
            HealthEffectRenderer.color = new Color32(55, 255, 0, 255);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            HealthEffectRenderer.color = Color.white;
        }
    }
}