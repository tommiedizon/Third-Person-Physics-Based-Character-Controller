using UnityEngine;
using KBCore.Refs;
using System;
using UnityUtils;

namespace CharacterControllerFactory
{
    [RequireComponent(typeof(PlayerMover))]
    public class TPPlayerControllerWithPhysics : MonoBehaviour {
        /* Primary purpose of this class is to calculate the momentum and velocity of the player
         * based on player input and the state that we're in.
         */

        #region Fields
        [SerializeField] InputReader input;

        Transform tr;
        PlayerMover mover;
        //CeilingDetecor ceilingDetector;

        bool jumpInputIsLocked, jumpKeyWasPressed, jumpKeyWasLetGo, jumpKeyIsPressed;

        public float movementSpeed = 7f;
        public float airControlRate = 2f;
        public float jumpSpeed = 10f;
        public float jumpDuration = 0.2f;
        public float airFriction = 0.5f;
        public float groundFriction = 100f;
        public float gravity = 30f;
        public float slideGravity = 5f;
        public float slopeLimit = 30f;
        public bool useLocalMomentum;

        StateMachine stateMachine;
        CountdownTimer jumpTimer;

        [SerializeField] Transform cameraTransform;

        Vector3 momentum, savedVelocity, savedMovementVelocity;

        public event Action<Vector3> OnJump = delegate { };
        public event Action<Vector3> OnLand = delegate { };
        #endregion

        private void Awake() {
            tr = transform;
            mover = GetComponent<PlayerMover>();
            //ceilingDetector = GetComponent<CeilingDetector>

            jumpTimer = new CountdownTimer(jumpDuration);
            SetUpStateMachine();


        }

        public Vector3 GetMomentum() => useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
        private void SetUpStateMachine() {
            // noop
        }

        private void FixedUpdate() {
            mover.CheckForGround();
            HandleMomentum();
            // TODO Calculate Movement Velocity
        }

        private void HandleMomentum() {
            if (useLocalMomentum) momentum = tr.localToWorldMatrix * momentum;

            Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, tr.up); // extracts component of momentum parallel to tr.up
            Vector3 horizontalMomentum = momentum - verticalMomentum;

        }
    }
}
