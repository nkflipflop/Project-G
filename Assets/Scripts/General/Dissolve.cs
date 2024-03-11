using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public Material DissolveMaterial;

    private bool _initted = false;
    private bool _childDissolveStarted = false;
    [SerializeField] private Dissolve _weaponDissolve = null;

    private float _fade = 1f;
    private static readonly int FadePropertyID = Shader.PropertyToID("_Fade");

    public bool IsDissolving { get; set; }
    
    private void Update()
    {
        if (IsDissolving)
        {
            // Assigning dissolve material to renderer
            if (!_initted)
            {
                Renderer.material = DissolveMaterial;
                _initted = true;
            }

            if (!_childDissolveStarted && _weaponDissolve != null)
            {
                _childDissolveStarted = true;
                _weaponDissolve.IsDissolving = true;
            }

            _fade -= Time.deltaTime;

            if (_fade <= 0f)
            {
                _fade = 0f;
                IsDissolving = false;
                Destroy(gameObject);
            }

            Renderer.material.SetFloat(FadePropertyID, _fade); // setting the property
        }
    }
}