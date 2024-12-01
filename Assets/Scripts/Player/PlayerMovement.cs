using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Player
{
	public partial class Player
	{
        [Foldout("Movement"), SerializeField] private Animator animator;
        [Foldout("Movement"), SerializeField] private Rigidbody2D rb2D;
        [Foldout("Movement"), SerializeField] private GameObject dustParticles;
        
		[Foldout("Movement"), SerializeField] private float moveSpeed = 4f;
        [Foldout("Movement"), SerializeField] private float moveLatency = 0.05f;

        private float horizontalInput;
        private float verticalInput;
        private Vector3 mousePosition;
        public bool IsRunning { get; private set; }
        
        private readonly int runAnimHash = Animator.StringToHash("Run");
        private readonly int horizontalAnimHash = Animator.StringToHash("Horizontal");
        private readonly int verticalAnimHash = Animator.StringToHash("Vertical");

        private void Start()
        {
            dustParticles.SetActive(false);
        }

        private void FixedUpdate()
        {
            PlayerAnimate(); // For Animations
            PlayerMovement(); // For Movement Actions
        }

        private void GetInputs()
        {
            // Inputs for Movement
            horizontalInput = Input.GetAxisRaw("Horizontal"); // held direction keys WASD
            verticalInput = Input.GetAxisRaw("Vertical");

            mousePosition = Input.mousePosition; // for determining player direction
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);        // bro WTF? calling Camera.main in Update. Fix this ASAP !!!

            if (horizontalInput != 0 || verticalInput != 0)
            {
                IsRunning = true;
            }
        }

        // Animate Player
        private void PlayerAnimate()
        {
            animator.SetBool(runAnimHash, IsRunning); // for run animation    // when holding direction keys WASD

            Vector2 mouseDir = mousePosition - transform.position;
            animator.SetFloat(horizontalAnimHash, mouseDir.x);
            animator.SetFloat(verticalAnimHash, mouseDir.y);
        }

        // Take Action
        private void PlayerMovement()
        {
            // when holding direction keys WASD
            if (IsRunning)
            {
                PlayerRun();
            }
            else
            {
                _ = DisableParticles();
            }
        }

        // Player Run State
        private void PlayerRun()
        {
            // adjusting unit vector for moving stable
            float unitSpeed = Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(verticalInput) > 0
                ? moveSpeed / Mathf.Sqrt(2)
                : moveSpeed;


            // Moving in 8 Directions
            if (Mathf.Abs(horizontalInput) > 0.9f || Mathf.Abs(verticalInput) > 0.9f)
            {
                moveLatency -= Time.deltaTime;
                if (moveLatency <= 0)
                {
                    rb2D.linearVelocity = new Vector2(horizontalInput, verticalInput) * unitSpeed;
                    IsRunning = true;
                }
            }
            else
            {
                rb2D.linearVelocity = new Vector2(0f, 0f);
                moveLatency = 0.05f;
                IsRunning = false;
            }

            dustParticles.SetActive(true);
            SoundManager.PlaySound(SoundManager.Sound.PlayerMove);
        }

        private async UniTaskVoid DisableParticles()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: this.GetCancellationTokenOnDestroy());
            dustParticles.SetActive(false);
        }
	}
}