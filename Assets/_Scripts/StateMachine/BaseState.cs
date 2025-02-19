using UnityEngine;

namespace CharacterControllerFactory {
    public abstract class BaseState : IState {

        // Maybe require a StateMachineContext Class instead of explicitly declaring types here?
        // Should make this a generic class (IHeartGameDev might have a slightly more general implementation)
        protected readonly TPPlayerControllerWithPhysicsDraft controller;
        protected readonly Animator animator;

        // Always hash strings. String comparisons for retrieval is not ideal.  
        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");

        protected const float crossFadeDuration = 0.1f; // time to transition between animations

        protected BaseState(TPPlayerControllerWithPhysicsDraft controller, Animator animator) {
            this.controller = controller;
            this.animator = animator;
        }
        public virtual void FixedUpdate() {
            // noop
        }
        public virtual void OnEnter() {
            // noop
        }
        public virtual void OnExit() {
            // noop
        }
        public virtual void Update() {
            // noop
        }
    }

}

