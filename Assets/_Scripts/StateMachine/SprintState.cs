using UnityEngine;

public class SprintState : PlayerMovementState {
    public SprintState(PlayerMovementContext context, PlayerMovementStateManager.EMoveState stateKey) : base(context, stateKey) {
        PlayerMovementContext Context = context;
    }

    public override void EnterState() {
        Context.PlayerVisual.TransitionToSprintAnimation(0.1f);
        Debug.Log("starting sprint");
    }

    public override void ExitState() {
        Debug.Log("exiting sprint");
    }

    public override PlayerMovementStateManager.EMoveState GetNextState() {
        if(Context.Player.isSprinting) {
            return StateKey;
        } else if(Context.Player.isWalking) { 
            return PlayerMovementStateManager.EMoveState.Walk;
        } else {
            return PlayerMovementStateManager.EMoveState.Idle;
        }
    }

    public override void OnTriggerEnter(Collider other) {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit(Collider other) {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other) {
        throw new System.NotImplementedException();
    }

    public override void UpdateState() {
        Debug.Log("Sprinting");
    }
}
