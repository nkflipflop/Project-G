using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour {

	[SerializeField] private int _damage = 2;
	private float _coolDown = 0.8f;
	private bool _didDamage = false;
	private BoxCollider2D _boxCollider2D;
	private Animator _animator;
	[SerializeField] private SoundManager.Sound _sound = SoundManager.Sound.SpikeTrap;
	
	void Start() {
		_boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
		_animator = gameObject.GetComponent<Animator>();
		float startDelay = Random.Range(0f, 1.517f);
		StartCoroutine(StartAnimation(startDelay));
	}

	private IEnumerator StartAnimation(float time) {
		yield return new WaitForSeconds(time);
		_animator.SetBool("Start", true);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!_didDamage && other.gameObject.CompareTag("Player")) {
			other.gameObject.GetComponent<HealthController>().TakeDamage(_damage);
			_didDamage = true;
			StartCoroutine(ResetDidDamage());
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (!_didDamage && other.gameObject.CompareTag("Player")) {
			other.gameObject.GetComponent<HealthController>().TakeDamage(_damage);
			_didDamage = true;
			StartCoroutine(ResetDidDamage());
		}
	}

	public void EnableCollider() {
		_boxCollider2D.enabled = true;
	}

	public void DisableCollider() {
		_boxCollider2D.enabled = false;
	}

	public void PlaySoundEffect() {
		SoundManager.PlaySound(_sound, transform.position);
	}

	IEnumerator ResetDidDamage() {
		yield return new WaitForSeconds(_coolDown);
		_didDamage = false;
	}
}
