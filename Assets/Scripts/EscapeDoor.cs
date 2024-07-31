using System;
using Cysharp.Threading.Tasks;
using Pooling;
using Pooling.Interfaces;
using UnityEngine;

public class EscapeDoor : MonoBehaviour, IPoolable
{
    [NonSerialized] public GameManager gameManager;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider2D ;
    [SerializeField] private Sprite doorOpenSprite, doorClosedSprite;
    
    [field: SerializeField] public ObjectType Type { get; set; }

    public bool IsDoorOpen => boxCollider2D.enabled;

    public void OpenTheDoor()
    {
        spriteRenderer.sprite = doorOpenSprite;
        boxCollider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _ = PlayerOnTheExit(0.25f);
        }
    }

    private async UniTaskVoid PlayerOnTheExit(float time)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        gameManager.LoadNextLevel();        // next level
    }

    public void OnSpawn()
    {
        spriteRenderer.sprite = doorClosedSprite;
        boxCollider2D.enabled = false;
    }

    public void OnReset() { }
}