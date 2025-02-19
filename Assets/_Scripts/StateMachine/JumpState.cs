using UnityEngine;

namespace CharacterControllerFactory {
    public class JumpState : BaseState {
        public JumpState(TPPlayerControllerWithPhysicsDraft controller, Animator animator) : base(controller, animator) {
            
        }

        public override void OnEnter() {
            Debug.Log("Entering Jump State");
            animator.CrossFade(JumpHash, crossFadeDuration);
        }

        public override void FixedUpdate() {
            // call player's jump logic and move logic
            controller.HandleMovement();
            controller.HandleJump();
        }

    }

}

