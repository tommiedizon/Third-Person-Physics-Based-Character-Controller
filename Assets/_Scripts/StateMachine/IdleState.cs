using UnityEngine;

public class IdleState : PlayerMovementState {
    public IdleState(PlayerMovementContext context, PlayerMovementStateManager.EMoveState stateKey) : base(context, stateKey) {
        PlayerMovementContext Context = context;
    }

    public override void EnterState() {
        Context.PlayerVisual.TransitionToIdleAnimation(0.1f);
    }

    public override void ExitState() {
        Debug.Log("Exiting IdleState!");
    }

    public override PlayerMovementStateManager.EMoveState GetNextState() {
        if (Context.Player.isWalking)
            return PlayerMovementStateManager.EMoveState.Walk;
        else
            return StateKey;
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
    }
}
