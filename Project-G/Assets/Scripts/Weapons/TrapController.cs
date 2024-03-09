using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private int _damage = 2;
    private float _coolDown = 0.8f;
    private bool _didDamage = false;
    private BoxCollider2D _boxCollider2D;
    private Animator _animator;
    [SerializeField] private SoundManager.Sound _sound = SoundManager.Sound.SpikeTrap;

    private void Start()
    {
        _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        _animator = gameObject.GetComponent<Animator>();
        float startDelay = Random.Range(0f, 1.517f);
        StartAnimation(startDelay);
    }

    private async UniTaskVoid StartAnimation(float time)
    {
        await UniTask.Delay(Mathf.RoundToInt(time * 1000));
        _animator.SetBool("Start", true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_didDamage && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HealthController>().TakeDamage(_damage);
            _didDamage = true;
            ResetDidDamage();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_didDamage && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HealthController>().TakeDamage(_damage);
            _didDamage = true;
            ResetDidDamage();
        }
    }

    public void EnableCollider()
    {
        _boxCollider2D.enabled = true;
    }

    public void DisableCollider()
    {
        _boxCollider2D.enabled = false;
    }

    public void PlaySoundEffect()
    {
        SoundManager.PlaySound(_sound, transform.position);
    }

    private async UniTaskVoid ResetDidDamage()
    {
        await UniTask.Delay(Mathf.RoundToInt(_coolDown * 1000));
        _didDamage = false;
    }
}