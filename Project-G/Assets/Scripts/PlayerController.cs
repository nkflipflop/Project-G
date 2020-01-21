using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private float _moveSpeed = 3f;
    private float _moveLatency = 0.05f;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _attackInput;

    private bool _isAttack = false;
    private bool _isRun = false;
    
    public Animator Animator;

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

        _attackInput = Input.GetMouseButton(0);              // pressed left mouse
        if (_attackInput)                                    // starts attack and its animation
            _isAttack = _attackInput;

        if (Mathf.Abs(_horizontalInput) > 0 ||  Mathf.Abs(_verticalInput) > 0)    // starts run and its animation
            _isRun = true;
    }

    // Animate Player
    void PlayerAnimate() {
        Animator.SetBool("Attack", _isAttack);   // for attack animation // when pressed left mouse
        Animator.SetBool("Run", _isRun);         // for run animation    // when holding direction keys WASD
    }

    // Take Action
    void PlayerMovement(){
        /*if (_attackInput)    // when pressed left mouse
            PlayerAttack();*/
        if (_isRun)          // when holding direction keys WASD
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

        // Flipping the sprite vertically with respect to the player's direction
        if (Input.GetAxis("Horizontal") < -0.1f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetAxis("Horizontal") > 0.1f) {
            transform.localScale = new Vector3(1, 1, 1);
        }      

        // Moving with 8 Directions
        if (Mathf.Abs(_horizontalInput) > 0.9f || Mathf.Abs(_verticalInput) > 0.9f){
            _moveLatency -= Time.deltaTime;
            if(_moveLatency <= 0)
                transform.Translate ( new Vector3 (_horizontalInput, _verticalInput, 0f) * unitSpeed * Time.deltaTime);
        }
        else{
            _moveLatency = 0.05f;
            _isRun = false;
        }
    }

    // Player Attack State
    void PlayerAttack(){
        Animator.SetBool("Attack", _isAttack);   
    }
    // When Attack Animation stops
    void PlayerAttackEnd() {
        _isAttack = false;
		Animator.SetBool("Attack", _isAttack);
	}

}