using UnityEngine;

public class ExplosiveEnemyController : EnemyController {
    public SpriteRenderer Renderer;
    public Material EmissionMaterial;
	public void DestroyObject() {
		Destroy(gameObject);
	}

    public void ChangeMaterial() {
        Renderer.material = EmissionMaterial;
    }
}
