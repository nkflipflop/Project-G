using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    [NonSerialized] public GameManager GameManager;
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    public Sprite ExitDoorOpen;
    private BoxCollider2D _collider2D = null;

    public bool IsDoorOpen => _collider2D.enabled;

    private void Start()
    {
        _collider2D = GetComponent<BoxCollider2D>();
    }

    public void OpenTheDoor()
    {
        _spriteRenderer.sprite = ExitDoorOpen;
        _collider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerOnTheExit(0.25f);
        }
    }

    private async UniTaskVoid PlayerOnTheExit(float time)
    {
        await UniTask.Delay(Mathf.RoundToInt(time * 1000));
        GameManager.LoadNextLevel();        // next level
    }
}