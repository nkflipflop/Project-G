using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int _health = 25;

    private bool _isDead;

    public Dissolve DissolveEffect;
    public SpriteRenderer Renderer;
    public CapsuleCollider2D HitboxCollider;
    public SoundManager.Sound SoundEffect = SoundManager.Sound.CharacterHit;

    public bool IsDead => _isDead;

    public int Health
    {
        get => _health;
        set => _health = value;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;
            DissolveEffect.IsDissolving = true;
            _isDead = true;
            HitboxCollider.enabled = false;
        }

        // Damage effect on sprite
        DamageEffect();
        // Sound Effect
        SoundManager.PlaySound(SoundEffect, transform.position);
    }

    private async UniTaskVoid DamageEffect()
    {
        Renderer.color = new Color32(255, 55, 0, 255);
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        Renderer.color = Color.white;
    }

    public void Heal(int healValue)
    {
        _health += healValue;

        if (_health > 100)
        {
            _health = 100;
        }

        // Heal effect on sprite
        HealEffect();
    }

    private async UniTaskVoid HealEffect()
    {
        Renderer.color = new Color32(55, 255, 0, 255);
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        Renderer.color = Color.white;
    }
}