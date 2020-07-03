using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public Sprite ExitDoorOpen;
    [SerializeField] private BoxCollider2D _collider2D;
    public bool IsDoorOpen {
        get { return _collider2D.enabled; }
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
