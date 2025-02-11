using UnityEngine;

public class WalkState : PlayerMovementState {
    public WalkState(PlayerMovementContext context, PlayerMovementStateManager.EMoveState stateKey) : base(context, stateKey) {
        PlayerMovementContext Context = context;
    }

    public override void EnterState() {
        Context.PlayerVisual.TransitionToWalkAnimation(0.1f);
    }

    public override void ExitState() {
        Debug.Log("Exiting Walk State");
    }

    public override PlayerMovementStateManager.EMoveState GetNextState() {
        if (Context.Player.isWalking) {
            return StateKey;
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
        //
    }
}
