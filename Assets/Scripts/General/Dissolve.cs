using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public Material DissolveMaterial;

    private bool initted = false;
    private bool childDissolveStarted = false;
    [SerializeField] private Dissolve _weaponDissolve = null;

    private float fade = 1f;
    private static readonly int FadePropertyID = Shader.PropertyToID("_Fade");

    public bool IsDissolving { get; set; }
    
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
                Destroy(gameObject);
            }

            Renderer.material.SetFloat(FadePropertyID, fade); // setting the property
        }
    }
}