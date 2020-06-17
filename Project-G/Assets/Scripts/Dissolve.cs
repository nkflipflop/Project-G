using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public Material DissolveMaterial;
    
    private bool _initted = false;
    private bool _isDissolving = false;
    private bool _childDissolveStarted = false;
    [SerializeField] private bool _hasChild = false;

    private float _fade = 1f;

    public bool IsDissolving {
        get { return _isDissolving; }
        set { _isDissolving = value; }
    }

    // Update is called once per frame
    private void Update() {
        if (_isDissolving) {
            // Assigning dissolve material to renderer
            if (!_initted){
                Renderer.material = DissolveMaterial;
                _initted = true;
            }
            
            if(!_childDissolveStarted && _hasChild) {
                _childDissolveStarted = true;
                transform.GetChild (0).gameObject.GetComponent<Dissolve>().IsDissolving = true;
            }
            _fade -= Time.deltaTime;

            if (_fade <= 0f) {
                _fade = 0f;
                _isDissolving = false;
                Destroy(gameObject);
            }

            Renderer.material.SetFloat("_Fade", _fade);      // setting the property
        }
    }
}
