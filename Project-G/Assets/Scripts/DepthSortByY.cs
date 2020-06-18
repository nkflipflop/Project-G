using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthSortByY : MonoBehaviour {
	private const int _isometricRangePerYUnit = 12;

	[SerializeField] private bool _walkingObject = false;

	private Renderer _renderer;

	private void Start() {
		_renderer = GetComponent<Renderer>();
		_renderer.sortingOrder = -(int)(transform.position.y * _isometricRangePerYUnit);      // setting its order according to its y position
	}

	void Update() {
		if (_walkingObject)
			_renderer.sortingOrder = -(int)(transform.position.y * _isometricRangePerYUnit);      // setting its order according to its y position
	}
}
