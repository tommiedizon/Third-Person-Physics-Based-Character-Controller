using UnityEngine;

public abstract class PlayerMovementState : BaseState<PlayerMovementStateManager.EMoveState> {

    protected PlayerMovementContext Context;
    public PlayerMovementState(PlayerMovementContext context, PlayerMovementStateManager.EMoveState stateKey) : base(stateKey) {
        Context = context;
    }

}
