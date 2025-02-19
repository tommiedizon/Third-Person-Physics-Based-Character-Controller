using UnityEngine;
using KBCore.Refs;
using Unity.Cinemachine;
using System.Collections.Generic;
using System;
using NUnit.Framework;

namespace CharacterControllerFactory {

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent (typeof(CapsuleCollider))]
    [RequireComponent (typeof(GroundChecker))]
    [RequireComponent (typeof(PlatformCollisionHandler))] // optional if you want moving platforms
    public partial class TPPlayerControllerWithPhysicsDraft : ValidatedMonoBehaviour {

        const float Zerof = 0f;

        [Header("References")]
        [SerializeField, Self] Rigidbody rb;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineCamera freeLookVCam;
        [SerializeField, Anywhere] InputReader input;
        [SerializeField, Self] GroundChecker groundChecker;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f; // How fast the animator will change speed

        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float jumpDuration = 0.5f;
        [SerializeField] float gravityMultiplier = 1f;

        [Header("Debug Variables")]
        private Transform mainCam;

        private Vector3 movement;
        private float currentSpeed, velocity; // Why is velocity a float??
        private float jumpVelocity;


        private List<Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;

        StateMachine stateMachine;

        // Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake() {
            mainCam = Camera.main.transform;

            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            // Invoke event when observed transform is teleported, adjusting freeLookVCam's position accordingly
            freeLookVCam.OnTargetObjectWarped(transform, transform.position - freeLookVCam.transform.position - Vector3.forward);

            rb.freezeRotation = true; // make sure char doesn't tip over

            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            timers = new List<Timer>(2) {jumpTimer, jumpCooldownTimer };

            jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

            // State machine initialisation
            stateMachine = new StateMachine();

            // Declare states
            IState locomotionState = new LocomotionState(this, animator);
            IState jumpState = new JumpState(this, animator);

            // Define transitions
            At(locomotionState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
            At(jumpState, locomotionState, new FuncPredicate(() => true));

            // Set initial state
            stateMachine.SetState(locomotionState);

        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        //void Any(IState to, IPredicate condition) => stateMachine.AddAnyTranstiion(to, condition);

        private void OnEnable() {
            input.Jump += OnJump;
        }

        private void OnDisable() {
            input.Jump -= OnJump;
        }

        private void OnJump(bool performed) {
            if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded) {
                jumpTimer.Start();
            } else if(!performed && jumpTimer.IsRunning) {
                jumpTimer.Stop();
            }
        }

        private void Start() {
            input.EnablePlayerActions();
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update() {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            stateMachine.Update();
            UpdateAnimator();
            HandleTimers();
        }

        private void HandleTimers() {
            foreach (var timer in timers) {
                timer.Tick(Time.deltaTime);
            }
        }

        public void HandleJump() {
            // If not jumping and grounded, keep jump vel at 0
            if (!jumpTimer.IsRunning && groundChecker.IsGrounded) {
                jumpVelocity = Zerof;
                jumpTimer.Stop();
                return;
            }

            // If jumping or falling, calculate velocity
            if (jumpTimer.IsRunning) {
                // Gravity takes over
                jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }

            // Apply velocity to the rigidbody
            rb.linearVelocity = new Vector3 (rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }

        private void FixedUpdate() {
            stateMachine.FixedUpdate();
        }
        private void UpdateAnimator() {
            animator.SetFloat(Speed, currentSpeed);
        }
        public void HandleMovement() {
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement; // basically [camera coords] - [transform] * moveDir in 1 line. 

            if (adjustedDirection.magnitude > Zerof) {
                HandleRotation(adjustedDirection);
                HandleHorizontalMovement(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            } else {
                SmoothSpeed(Zerof);

                // Reset horizontal velocity for a snappy stop
                rb.linearVelocity = new Vector3(Zerof, rb.linearVelocity.y, Zerof);
            }

        }
        private void HandleRotation(Vector3 adjustedDirection) {
            // Adjust rotation to match movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection); // Creates rotation from world up (0,1,0) to specified rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        }
        private void HandleHorizontalMovement(Vector3 adjustedDirection) {
            Vector3 velocity = adjustedDirection * moveSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }
        private void SmoothSpeed(float value) {
            // smoothly transitions currentspeed to match adjustedMovement.magnitude over time. 
            // ref velocity is a reference variable to store velocity changes
            // smoothTime is the time taken to reach the target speed
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }
    }

}