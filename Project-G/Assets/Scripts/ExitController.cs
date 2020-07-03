using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public Sprite ExitDoorOpen;
    private BoxCollider2D _collider2D = null;
    public bool IsDoorOpen {
        get { return _collider2D.enabled; }
    }

    private void Start() {
        _collider2D = GetComponent<BoxCollider2D>();
    }

    public void OpenExitDoor() {
        _spriteRenderer.sprite = ExitDoorOpen;
        _collider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            // new level
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player") {
            // new level
        }
    }
}
