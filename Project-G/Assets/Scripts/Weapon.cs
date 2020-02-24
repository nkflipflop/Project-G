using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	private Vector3 _mousePosition;
    private float _angle;
    private float _distance = .5f;
    private float _offset = 180;
    // Update is called once per frame
	void Update() {
		GetInputs();        // Getting any input
	}

    void FixedUpdate() {
        _angle = Mathf.Atan2(_mousePosition.y, _mousePosition.x) * Mathf.Rad2Deg;

        // Flipping the sprite vertically with respect to mouse
		float direction = _mousePosition.x - transform.parent.position.x;
        _mousePosition = _mousePosition - transform.parent.position;


        Debug.Log(direction);
        if( direction < 0f)
            transform.localRotation = Quaternion.Euler(0f, 0f, -_angle);
        else
            transform.localRotation = Quaternion.Euler(0f, 0f, _angle);

        float xPos = Mathf.Cos(Mathf.Deg2Rad * _angle) * _distance;
        float yPos = Mathf.Sin(Mathf.Deg2Rad * _angle) * _distance;

       // transform.localPosition = new Vector3(transform.parent.position.x - .1f + xPos, transform.parent.position.y - .1f + yPos, 0);
    }

	void GetInputs(){
		_mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
	}
}
