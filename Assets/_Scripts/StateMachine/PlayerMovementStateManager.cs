using UnityEngine;

public class PlayerMovementStateManager : StateManager<PlayerMovementStateManager.EMoveState>
{
    public enum EMoveState {
        Idle,
        Walk,
        Sprint,
        Fall,
        Jump  
    }
}
