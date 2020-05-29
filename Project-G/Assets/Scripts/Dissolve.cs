using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private Material _material;

    private bool _isDissolving = false;

    private float _fade = 1f;

    public bool IsDissolving {
        get { return _isDissolving; }
        set { _isDissolving = value; }
    }

    private void Start() {
        _material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    private void Update() {
        if (_isDissolving) {
            _fade -= Time.deltaTime;

            if (_fade <= 0f) {
                _fade = 0f;
                _isDissolving = false;
                Destroy(gameObject);
            }

            _material.SetFloat("_Fade", _fade);      // setting the property
        }
    }
}
