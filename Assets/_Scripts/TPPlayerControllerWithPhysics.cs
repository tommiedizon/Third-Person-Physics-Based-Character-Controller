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
    public partial class TPPlayerControllerWithPhysics : ValidatedMonoBehaviour {

        const float Zerof = 0f;

        [Header("References")]
        [SerializeField, Self] Rigidbody rb;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineCamera freeLookVCam;
        [SerializeField, Anywhere] InputReader input;
        [SerializeField, Self] GroundChecker groundChecker;

        [Header("Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f; // How fast the animator will change speed

        Transform mainCam;
        Vector3 movement;
        List<Timer> timers;

        float currentSpeed, velocity; // Why is velocity a float??

        // Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake() {
            mainCam = Camera.main.transform;

            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            // Invoke event when observed transform is teleported, adjusting freeLookVCam's position accordingly
            freeLookVCam.OnTargetObjectWarped(transform, transform.position - freeLookVCam.transform.position - Vector3.forward);

            rb.freezeRotation = true; // make sure char doesn't tip over
        }

        private void Start() {
            input.EnablePlayerActions();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update() {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            UpdateAnimator();
        }

        private void FixedUpdate() {
            HandleMovement();
        }

        private void UpdateAnimator() {
            animator.SetFloat(Speed, currentSpeed);
        }

        private void HandleMovement() {
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