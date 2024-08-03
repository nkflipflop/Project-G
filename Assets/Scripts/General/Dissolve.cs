using Pooling.Interfaces;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public Material DissolveMaterial;

    private bool initted = false;
    private bool childDissolveStarted = false;
    public Dissolve _weaponDissolve = null;

    private float fade = 1f;
    private static readonly int FadePropertyID = Shader.PropertyToID("_Fade");

    public bool IsDissolving { get; set; }

    private Material defaultMaterial;

    public void Reset()
    {
        defaultMaterial ??= Renderer.material;
        IsDissolving = false;
        Renderer.material = defaultMaterial;
        initted = false;
        childDissolveStarted = false;
        fade = 1f;
        _weaponDissolve?.Reset();
    }

    private void Update()
    {
        if (IsDissolving)
        {
            // Assigning dissolve material to renderer
            if (!initted)
            {
                Renderer.material = DissolveMaterial;
                initted = true;
            }

            if (!childDissolveStarted && _weaponDissolve != null)
            {
                childDissolveStarted = true;
                _weaponDissolve.IsDissolving = true;
            }

            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                IsDissolving = false;
                if (TryGetComponent(out IPoolable poolObject))
                {
                    poolObject.ResetObject();
                }
            }

            Renderer.material.SetFloat(FadePropertyID, fade); // setting the property
        }
    }
}