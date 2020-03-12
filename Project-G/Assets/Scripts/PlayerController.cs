using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
  
	[SerializeField] private float _moveSpeed = 4f;
	[SerializeField] private float _moveLatency = 0.05f;

	private float _horizontalInput;
	private float _verticalInput;
	private bool _attackInput;
	private Vector3 _mousePosition;

	private bool _isAttack = false;
	private bool _isRun = false;
	
	private Animator _animator;
  	private Rigidbody2D _rb2D;
	SpriteRenderer _sprite;
	
	private void Start() {
		_animator = gameObject.GetComponent<Animator>();
		_sprite = GetComponent<SpriteRenderer>();
		_rb2D = gameObject.GetComponent<Rigidbody2D>();
	}

	void Update() {
		GetInputs();        // Getting any input
	}

	void FixedUpdate() {    
		PlayerAnimate();    // For Animations
		PlayerMovement();   // For Movement Actions
	}

	void GetInputs(){
		_horizontalInput = Input.GetAxisRaw("Horizontal");   // held direction keys WASD
		_verticalInput = Input.GetAxisRaw("Vertical");

		_mousePosition = Input.mousePosition;				// for determining player direction
		_mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);

		_attackInput = Input.GetMouseButton(0);              // pressed left mouse
		if (_attackInput)                                    // starts attack and its animation
			_isAttack = _attackInput;

		if (Mathf.Abs(_horizontalInput) > 0 ||  Mathf.Abs(_verticalInput) > 0)    // starts run and its animation
			_isRun = true;
		if (Input.GetKeyDown(KeyCode.Space)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	// Animate Player
	void PlayerAnimate() {
		_animator.SetBool("Run", _isRun);         // for run animation    // when holding direction keys WASD
	}

	// Take Action
	void PlayerMovement(){
		/*if (_attackInput)    // when pressed left mouse
			PlayerAttack();*/
		
		// Flipping the sprite vertically with respect to mouse
		float direction = _mousePosition.x - transform.position.x;
		if (direction < 0)
			_sprite.flipX = true;
		else
			_sprite.flipX = false;
		
		// when holding direction keys WASD
		if (_isRun)
			PlayerRun();
	}

	// Player Run State
	void PlayerRun() {
		// adjusting unit vector for moving stable (YOU DO NOT NEED TO LOOK HERE ;) )
		float unitSpeed;
		if(Mathf.Abs(_horizontalInput) > 0 && Mathf.Abs(_verticalInput) > 0)
			unitSpeed = _moveSpeed / Mathf.Sqrt(2);
		else
			unitSpeed = _moveSpeed;


		// Moving in 8 Directions
		if (Mathf.Abs(_horizontalInput) > 0.9f || Mathf.Abs(_verticalInput) > 0.9f){
			_moveLatency -= Time.deltaTime;
			if(_moveLatency <= 0)
				_rb2D.velocity = new Vector2(_horizontalInput, _verticalInput) * unitSpeed;
				//transform.Translate ( new Vector3 (_horizontalInput, _verticalInput, 0f) * unitSpeed * Time.deltaTime);
		}
		else {
			_rb2D.velocity = new Vector2(0f, 0f);
			_moveLatency = 0.05f;
			_isRun = false;
		}
	}

	// Player Attack State
	

}