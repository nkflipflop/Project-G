using Pooling.Interfaces;
using UnityEngine;

public class ExplosiveEnemy : Enemy
{
	public SpriteRenderer spiteRenderer;
	public Material emissionMaterial;

	public void DestroyObject()
	{
		this.ResetObject();
	}

	public void ChangeMaterial()
	{
		spiteRenderer.material = emissionMaterial;
	}
}