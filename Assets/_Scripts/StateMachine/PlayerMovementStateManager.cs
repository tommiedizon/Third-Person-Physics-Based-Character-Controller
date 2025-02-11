using NUnit.Framework;
using UnityEngine;

public class PlayerMovementStateManager : StateManager<PlayerMovementStateManager.EMoveState>
{
    [SerializeField] Player player;
    [SerializeField] PlayerVisual playerVisual;

    private PlayerMovementContext _context;
    public enum EMoveState {
        Idle,
        Walk,
        Sprint,
        Fall,
        Jump  
    }

    private void Awake() {
        ValidatePlayer();
        _context = new PlayerMovementContext(player, playerVisual);
        InitializeStates();
    }

    private void ValidatePlayer() {
        Assert.IsNotNull(player, "Player is not assigned.");
    }

    private void InitializeStates() {
        states.Add(EMoveState.Idle, new IdleState(_context, EMoveState.Idle));
        states.Add(EMoveState.Walk, new WalkState(_context, EMoveState.Walk));
        currentState = states[EMoveState.Idle];
    }

}
