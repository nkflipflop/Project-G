using System;
using Cysharp.Threading.Tasks;
using General;
using Pooling;
using Pooling.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trap : MonoBehaviour, IPoolable
{
    [field: SerializeField] public ObjectType Type { get; set; }
    [SerializeField] private int damage = 2;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Animator animator;
    [SerializeField] private SoundManager.Sound sound;
    private const float COOLDOWN = 0.8f;
    private bool didDamage = false;

    private async UniTaskVoid StartAnimation(float time)
    {
        await UniTask.Delay(Mathf.RoundToInt(time * 1000));
        animator.SetBool("Start", true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!didDamage && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IHealthInteractable>().TakeDamage(damage);
            didDamage = true;
            _ = ResetDidDamage();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!didDamage && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IHealthInteractable>().TakeDamage(damage);
            didDamage = true;
            _ = ResetDidDamage();
        }
    }

    public void EnableCollider()
    {
        boxCollider2D.enabled = true;
    }

    public void DisableCollider()
    {
        boxCollider2D.enabled = false;
    }

    public void PlaySoundEffect()
    {
        SoundManager.PlaySound(sound, transform.position);
    }

    private async UniTaskVoid ResetDidDamage()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(COOLDOWN));
        didDamage = false;
    }
    
    #region Pooling
    
    public void OnSpawn()
    {
        float startDelay = Random.Range(0f, 1.517f);
        _ = StartAnimation(startDelay);
    }

    public void OnReset() { }
    
    #endregion
}