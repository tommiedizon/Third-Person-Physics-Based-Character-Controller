using NUnit.Framework;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovementStateManager : StateManager<PlayerMovementStateManager.EMoveState>
{
    [SerializeField] Player player;
    [SerializeField] PlayerVisual playerVisual;
    [SerializeField] CinemachineCamera playerCamera;

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
        _context = new PlayerMovementContext(player, playerVisual, playerCamera);
        InitializeStates();
    }

    private void ValidatePlayer() {
        Assert.IsNotNull(player, "Player is not assigned.");
    }

    private void InitializeStates() {
        states.Add(EMoveState.Idle, new IdleState(_context, EMoveState.Idle));
        states.Add(EMoveState.Walk, new WalkState(_context, EMoveState.Walk));
        states.Add(EMoveState.Sprint, new SprintState(_context, EMoveState.Sprint));
        currentState = states[EMoveState.Idle];
    }

}
