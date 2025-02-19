using UnityEngine;

namespace CharacterControllerFactory {
    public class LocomotionState : BaseState {
        public LocomotionState(TPPlayerControllerWithPhysicsDraft controller, Animator animator) : base(controller, animator) {

        }

        public override void OnEnter() {
            Debug.Log("Entering Locomotion State");
            animator.CrossFade(LocomotionHash, crossFadeDuration);
        }

        public override void FixedUpdate() {
            controller.HandleMovement();
        }
    }

}

