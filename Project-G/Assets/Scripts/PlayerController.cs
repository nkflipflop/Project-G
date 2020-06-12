using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
  
	[SerializeField] private float _moveSpeed = 4f;
	[SerializeField] private float _moveLatency = 0.05f;

	private float _horizontalInput;
	private float _verticalInput;
	private Vector3 _mousePosition;

	public bool isRun = false;
	
	private Animator _animator;
  	private Rigidbody2D _rb2D;
	private SpriteRenderer _sprite;
	public GameObject DustParticles;
	
	private void Start() {
		_animator = gameObject.GetComponent<Animator>();
		_sprite = GetComponent<SpriteRenderer>();
		_rb2D = gameObject.GetComponent<Rigidbody2D>();
		DustParticles.SetActive(false);
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

		if (Mathf.Abs(_horizontalInput) > 0 ||  Mathf.Abs(_verticalInput) > 0)    // starts run and its animation
			isRun = true;
		if (Input.GetKeyDown(KeyCode.Space)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	// Animate Player
	void PlayerAnimate() {
		_animator.SetBool("Run", isRun);         			// for run animation    // when holding direction keys WASD
		
		Vector2 mouseDir = _mousePosition - transform.position;
		_animator.SetFloat("Horizontal", mouseDir.x); 
		_animator.SetFloat("Vertical", mouseDir.y);
	}

	// Take Action
	void PlayerMovement(){
		// when holding direction keys WASD
		if (isRun)
			PlayerRun();
		else
			StartCoroutine(DisableParticles());
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
			isRun = false;
		}

		DustParticles.SetActive(true);
	}

	IEnumerator DisableParticles() {
		yield return new WaitForSeconds(0.3f);
		DustParticles.SetActive(false);
	}
}