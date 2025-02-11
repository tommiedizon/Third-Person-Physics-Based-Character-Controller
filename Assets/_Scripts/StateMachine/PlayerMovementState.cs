using UnityEngine;

public abstract class PlayerMovementState : BaseState<PlayerMovementStateManager.EMoveState> {

    protected PlayerMovementContext Context;
    public PlayerMovementState(PlayerMovementContext context, PlayerMovementStateManager.EMoveState stateKey) : base(stateKey) {
        Context = context;
    }

    // Helpers
    protected Vector3 ComputeForwardDirection() {
        Vector3 vect = Context.Player.transform.position - Context.Camera.transform.position;
        return new Vector3(vect.x, 0, vect.z).normalized;
    }

    protected Vector3 ComputePerpDirection(Vector3 forwardDir) {
        return Vector3.Cross(forwardDir, Vector3.up).normalized;
    }

    protected Vector3 ComputeMoveDirFromGameInput() {
        Vector2 inputVector = GameInput.GameInputInstance.GetMovementVectorNormalized();
        return (inputVector.y * ComputeForwardDirection() - inputVector.x * ComputePerpDirection(ComputeForwardDirection())).normalized;
    }

}
