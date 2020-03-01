using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthSortByY : MonoBehaviour {
	private const int IsometricRangePerYUnit = 10;

	void Update() {
		Renderer renderer = GetComponent<Renderer>();
		renderer.sortingOrder = -(int)(transform.position.y * IsometricRangePerYUnit);      // setting its order according to its y position
	}
}
